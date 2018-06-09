using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	Gun subclass of Equipment that maintains gun control
	Fires bullets 
*/
public class Gun : Equipment {

	/*
		prefab of Bullet gameobject
	*/
	[SerializeField]
	protected GameObject bullet;
	/*
		number of bullets to fire per call
	*/
	[SerializeField]
    protected int roundsPerFire = 1;
	/*
		speed of bullet
	*/
	[SerializeField]
    protected int bulletSpeed = 20;
	/*
		distance in front of component owner to instantiate bullet
	*/
	[SerializeField]
    protected float barrelDist = 0.25f;
	/*
		damage done by bullet
	*/
	[SerializeField]
    protected float damage = 1.0f;
	/*
		time to wait between each fired bullet
	*/
	[SerializeField]
    protected float timeBetweenRounds = 0.125f;
	/*
		number of bullets left
	*/
	[SerializeField]
    protected int bulletsLeft;

	public float Damage {
		get { return damage; }
	}
	public int BulletsLeft {
		get { return bulletsLeft; }
		set { bulletsLeft = value; }
	}

    /*
		whether the gun is currently firing
	*/
    protected bool firing = false;

	protected virtual void Awake() {
		base.Awake ();
		count = bulletsLeft;
	}

	/*
	 	base class override
		when called, fire bullet if gun is not currently firing
	*/
	public override void UseEquipment () {
		if (bullet) {
			if (!firing) {
				firing = true;
				StartCoroutine ("Shoot");
			}
		}
	}

	/*
		coroutine to create and fire bullet
		fires in bursts based on roundsPerFire
		once done, gun can be fired again
	*/
	protected virtual IEnumerator Shoot() {
		for (int i = 0; i < roundsPerFire; i++) {
			if (bulletsLeft > 0 || bulletsLeft == -1) {
				Transform parent = transform.parent.gameObject.transform;
				Vector3 barrelPos = parent.position + parent.forward * barrelDist;
				GameObject firedBullet = (GameObject)Instantiate (bullet, barrelPos, Quaternion.AngleAxis (90, transform.right));
				Rigidbody rb = firedBullet.GetComponent<Rigidbody> ();
				Bullet b = firedBullet.GetComponent<Bullet> ();
				if (rb && b) {
					b.Damage = damage;
					firedBullet.layer = gameObject.layer;
					rb.velocity = parent.forward * bulletSpeed;
                    sfxGod.PlayOneShot(sfx, 0.02f * sfxMultiplier);
					if (bulletsLeft > 0) {
						bulletsLeft--;
						count = bulletsLeft;
					}
					yield return new WaitForSeconds (timeBetweenRounds);
				}
			}
		}
		firing = false;
	}
}
