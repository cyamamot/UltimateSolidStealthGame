using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSystem : MonoBehaviour {

	public GameObject gunPrefab;
	public float fireWaitTime = 2.0f;

	EnemySight sight;
	GameObject player;
	EnemyMovement movement;
	GameObject gunInstance;
	Gun gun;
	bool firing;

	// Use this for initialization
	void Start () {
		sight = GetComponent<EnemySight> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		movement = GetComponent<EnemyMovement> ();
		if (gunPrefab != null) {
			gunInstance = GameObject.Instantiate (gunPrefab, transform);
			if (gunInstance != null) {
				gunInstance.transform.localPosition = Vector3.zero;
				gunInstance.transform.localRotation = Quaternion.identity;
				gunInstance.transform.localScale = Vector3.one;
			}
			gun = gunInstance.GetComponent<Gun> ();
		}
	}

	void Update() {
		FireWeapon ();
	}

	public void FireWeapon() {
		if (firing == false) {
			if (sight != null && player != null && movement != null && gun != null) {
				Vector3 zeroAngleVec = new Vector3 (1.0f, 0.0f, 0.0f);
				if (movement.Alerted == true) {
					if (Vector3.Angle (transform.forward.normalized, zeroAngleVec) % 90.0f == 0.0f) {
						RaycastHit hit;
						if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, sight.IgnoreEnemiesLayer)) {
							if (hit.transform.CompareTag ("Player") == true) {
								firing = true;
								gun.Fire ();
								StartCoroutine ("FirePause");
							}
						}
					}
				}
			}
		}
	}

	IEnumerator FirePause() {
		yield return new WaitForSeconds (fireWaitTime);
		firing = false;
		yield return null;
	}
}
