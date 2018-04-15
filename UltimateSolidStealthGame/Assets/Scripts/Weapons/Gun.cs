using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public GameObject bullet;
	public string gunType;
	public int roundsPerFire = 1;
	public int bulletSpeed = 20;
	public float barrelDist= 0.25f;
	public float damage = 1.0f;
	public float timeBetweenRounds = 0.125f;

	bool firing;

	public void Fire () {
		if (bullet != null) {
			if (firing == false) {
				firing = true;
				StartCoroutine ("Shoot");
			}
		}
	}

	IEnumerator Shoot() {
		for (int i = 0; i < roundsPerFire; i++) {
			Transform parent = transform.parent.gameObject.transform;
			Vector3 barrelPos = parent.position + parent.forward * barrelDist;
			GameObject firedBullet = (GameObject)Instantiate (bullet, barrelPos, Quaternion.AngleAxis (90, transform.right));
			Rigidbody rb = firedBullet.GetComponent<Rigidbody> ();
			Bullet b = firedBullet.GetComponent<Bullet> ();
			if (rb != null && b != null) {
				b.Owner = gameObject.tag;
				b.Damage = damage;
				rb.velocity = parent.forward * bulletSpeed;
				yield return new WaitForSeconds (timeBetweenRounds);
			}
		}
		firing = false;
	}
}
