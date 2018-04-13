using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSystem : MonoBehaviour {

	public Gun gun;

	EnemySight sight;
	GameObject player;
	EnemyMovement movement;

	// Use this for initialization
	void Start () {
		sight = GetComponent<EnemySight> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		GameObject temp = GameObject.FindGameObjectWithTag ("Enemy");
		if (temp != null) {
			movement = temp.GetComponent<EnemyMovement> ();
		}
	}

	void Update() {
		FireWeapon ();
	}

	public void FireWeapon() {
		if (sight != null && player != null && movement != null) {
			if (movement.Alerted == true) {
				RaycastHit hit;
				if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, sight.IgnoreEnemiesLayer)) {
					if (hit.transform.CompareTag("Player") == true) {
						gun.Fire ();
					}
				}
			}
		}
	}
}
