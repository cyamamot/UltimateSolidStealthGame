using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggestBossHealthManager : HealthManager {

    [SerializeField]
    GameObject bossUIObject;

    [SerializeField]
    List<string> specialDamagers;

    float specialDamage;
    int normalDamageCount;
    float normalDamage;
    string currDamager;
    float currDamageAmount;
    BossHealthUI bossUI;

    protected override void Awake () {
        base.Awake();
        specialDamage = 1.0f;
        normalDamageCount = specialDamagers.Count + 1;
        normalDamage = health / normalDamageCount;
        specialDamagers.Shuffle();
        currDamager = "Normal";
        currDamageAmount = normalDamage;
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
                if (currDamager == "Normal") {
                    if (specialDamagers.Count > 0) {
                        currDamager = specialDamagers[0];
                        bossUI.SetSpecial(currDamager);
                        specialDamagers.RemoveAt(0);
                        currDamageAmount = specialDamage;
                    } else {
                        manager.Kill();
                    }
                } else {
                    currDamager = "Normal";
                    currDamageAmount = normalDamage;
                }
            }
        }
    }
}
