using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : EnemyManager {

    [SerializeField]
    int killsTillDeath;
    [SerializeField]
    GameObject healthUIObject;

    SnakeHead head;
    List<HealthManager> healthList;
    LevelManager levelManager;
    int currHealthIndex;
    float maxTotalHealth;
    float currTotalHealth;
    BossHealthUI healthUI;

	protected override void Awake () {
        base.Awake();
        head = (transform.parent != null) ? transform.parent.GetComponentInChildren<SnakeHead>() : GetComponent<SnakeHead>();
        HealthManager[] healthArray = (transform.parent != null) ? transform.parent.GetComponentsInChildren<HealthManager>() : GetComponents<HealthManager>();
        healthList = new List<HealthManager>(healthArray);
        GameObject temp = GameObject.FindGameObjectWithTag("LevelManager");
        if (temp) {
            levelManager = temp.GetComponent<LevelManager>();
        }
        healthUI = healthUIObject.GetComponent<BossHealthUI>();
        maxTotalHealth = killsTillDeath * healthList[0].Health;
        currTotalHealth = maxTotalHealth;
        healthUI.SetMaxHealth(maxTotalHealth);
        alive = true;
	}

    private void Start() {
        if (healthList.Count > 0) {
            foreach (HealthManager curr in healthList) {
                curr.enabled = false;
            }
            SetRandomHealthIndex();
        }
    }

    void SetRandomHealthIndex() {
        currHealthIndex = Random.Range(0, healthList.Count);
        healthList[currHealthIndex].enabled = true;
    }

    public override void Kill () {
        killsTillDeath--;
        SnakeBody segment = healthList[currHealthIndex].gameObject.GetComponent<SnakeBody>();
        segment.KillSegment();
        healthList.RemoveAt(currHealthIndex);
        if (killsTillDeath <= 0) {
            Debug.Log("Enemy Dead");
            alive = false;
            DisableComponents();
            levelManager.FinishLevel();
        } else {
            SetRandomHealthIndex();
        }
    }

    public override void OnTakeDamage(float damage) {
        if (alive) {
            distraction.ResetDistraction();
            sight.SetSightOnPlayer();
            currTotalHealth -= damage;
            healthUI.SetHealth(currTotalHealth);
        }
    }

    protected override void DisableComponents() {
        movement.StopAllCoroutines();
        if (distraction) distraction.StopAllCoroutines();
        if (movement) movement.enabled = false;
        if (sight) sight.enabled = false;
        if (weaponSystem) weaponSystem.enabled = false;
        if (health) health.enabled = false;
        if (distraction) distraction.enabled = false;
    }

    public void SetCurrHealthAttackable(bool canAttack) {
        healthList[currHealthIndex].enabled = canAttack;
    }
}
