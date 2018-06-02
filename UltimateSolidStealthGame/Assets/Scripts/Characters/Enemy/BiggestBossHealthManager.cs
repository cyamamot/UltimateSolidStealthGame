using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggestBossHealthManager : HealthManager {

    [SerializeField]
    List<string> damagerOrder;
    [SerializeField]
    List<float> damageAmounts;
    [SerializeField]
    GameObject bossUIObject;

    string currDamager;
    float currDamageAmount;
    BossHealthUI bossUI;

    protected override void Awake () {
        base.Awake();
        currDamager = damagerOrder[0];
        currDamageAmount = damageAmounts[0];
        bossUI = bossUIObject.GetComponent<BossHealthUI>();
        bossUI.SetMaxHealth(health);
	}

    public override void Attack(float damage, string damager = "Normal") {
        manager.OnTakeDamage(damage);
        if (damager == currDamager) {
            currDamageAmount -= damage;
            if (damager == "Normal") {
                health -= damage;
                bossUI.SetHealth(health);
            } else {
                bossUI.SpecialDamage();
            }
            if (currDamageAmount <= 0.0f) {
                damagerOrder.RemoveAt(0);
                damageAmounts.RemoveAt(0);
                if (damagerOrder.Count > 0) {
                    currDamager = damagerOrder[0];
                    currDamageAmount = damageAmounts[0];
                } else {
                    manager.Kill();
                }
            }
        }
    }
}
