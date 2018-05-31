using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : EnemyManager {

    [SerializeField]
    int killsTillDeath;

    SnakeHead head;
    List<HealthManager> healthList;
    int currHealthIndex;

	protected override void Awake () {
        base.Awake();
        head = (transform.parent != null) ? transform.parent.GetComponentInChildren<SnakeHead>() : GetComponent<SnakeHead>();
        HealthManager[] healthArray = (transform.parent != null) ? transform.parent.GetComponentsInChildren<HealthManager>() : GetComponents<HealthManager>();
        healthList = new List<HealthManager>(healthArray);
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
        } else {
            SetRandomHealthIndex();
        }
    }

    public override void OnTakeDamage() {
        if (alive) {
            distraction.ResetDistraction();
            sight.SetSightOnPlayer();
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
