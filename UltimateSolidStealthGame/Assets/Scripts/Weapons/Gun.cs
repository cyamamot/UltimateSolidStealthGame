using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	[SerializeField]
	GameObject bullet;
	[SerializeField]
	string gunType;
	[SerializeField]
	int roundsPerFire = 1;
	[SerializeField]
	int bulletSpeed = 20;
	[SerializeField]
	float barrelDist= 0.25f;
	[SerializeField]
	float damage = 1.0f;
	[SerializeField]
	float timeBetweenRounds = 0.125f;
	[SerializeField]
	int bulletsLeft;

	public string GunType {
		get { return gunType; }
	}
	public float Damage {
		get { return damage; }
	}
	public int BulletsLeft {
		get { return bulletsLeft; }
		set { bulletsLeft = value; }
	}

	bool firing = false;

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
			if (bulletsLeft > 0 || bulletsLeft == -1) {
				Transform parent = transform.parent.gameObject.transform;
				Vector3 barrelPos = parent.position + parent.forward * barrelDist;
				GameObject firedBullet = (GameObject)Instantiate (bullet, barrelPos, Quaternion.AngleAxis (90, transform.right));
				Rigidbody rb = firedBullet.GetComponent<Rigidbody> ();
				Bullet b = firedBullet.GetComponent<Bullet> ();
				if (rb != null && b != null) {
					b.Owner = gameObject.tag;
					b.Damage = damage;
					rb.velocity = parent.forward * bulletSpeed;
					if (bulletsLeft > 0) {
						bulletsLeft--;
					}
					yield return new WaitForSeconds (timeBetweenRounds);
				}
			}
		}
		firing = false;
	}
}
