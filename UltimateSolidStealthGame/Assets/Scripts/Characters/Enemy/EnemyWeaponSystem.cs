using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class that allows enemy to fire at player based on equipped weapon
*/
public class EnemyWeaponSystem : MonoBehaviour {

	/*
		prefab of weapon object
	*/
	[SerializeField]
	protected GameObject gunPrefab;
	/*
		time between calls of fire to wait before calling fire again
	*/
	[SerializeField]
	protected float fireWaitTime = 2.0f;

    [SerializeField]
    protected float range;


	/*
		whether weapon is currently being fired
	*/
	protected bool firing;

	/*
		reference to EnemyManager component
	*/
	protected EnemyManager manager;
	/*
		instance of gunPrefab
	*/
	protected GameObject gunInstance;
	/*
		reference to Gun component of gunInstance
	*/
	protected Gun gun;

	protected virtual void Start () {
		manager = GetComponent<EnemyManager> ();
		if (gunPrefab) {
			gunInstance = GameObject.Instantiate (gunPrefab, transform);
			if (gunInstance != null) {
				gunInstance.layer = LayerMask.NameToLayer ("EnemyWeapon");
				gunInstance.transform.localPosition = Vector3.zero;
				gunInstance.transform.localRotation = Quaternion.identity;
				gunInstance.transform.localScale = Vector3.one;
				gun = gunInstance.GetComponent<Gun> ();
				if (gun) {
					gun.BulletsLeft = -1;
				}
			}
		}
	}

	protected virtual void Update() {
		FireWeapon ();
	}

	/*
		fires weapon if specified time had passed, weapon is not currently firing, and the enemy is visible in the camera
		if player is directly in front of enemy and enemy is looking in one of the cardinal directions, fire weapon
	*/
	public virtual void FireWeapon() {
		if (!firing && manager.Renderer.isVisible) {
			if (manager.Sight && manager.Player && manager.Movement && gun) {
				if (manager.Sight.Alerted) {
					if (Vector3.Angle (transform.forward.normalized, Vector3.right) % 90.0f == 0.0f) {
						RaycastHit hit;
						if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, manager.Sight.SpecialSightLayer)) {
                            if (range == 0 || range <= hit.distance) {
                                if (hit.transform.CompareTag("Player") == true) {
                                    firing = true;
                                    manager.Movement.StopMovement(0.75f);
                                    gun.UseEquipment();
                                    StartCoroutine("FirePause");
                                }
                            }
						}
					}
				}
			}
		}
	}

	/*
		coroutine to prevent enemy from firing for specified time
	*/
	protected IEnumerator FirePause() {
		yield return new WaitForSeconds (fireWaitTime);
		firing = false;
		yield return null;
	}
}
