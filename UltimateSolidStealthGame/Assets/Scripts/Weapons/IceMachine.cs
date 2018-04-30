using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMachine : Equipment {

	[SerializeField]
	float iceTime;
	[SerializeField]
	float refillPauseTime;
	[SerializeField]
	float refillTime;

	GameObject player;
	bool playerIced;

	// Use this for initialization
	void Awake () {
		base.Awake ();
		player = transform.parent.gameObject;
		Count = 100;
	}
	
	public override void UseEquipment ()
	{
		if (!playerIced) {
			StopCoroutine ("Refill");
			StartCoroutine ("Ice");
		} else {
			StopCoroutine ("Ice");
			StartCoroutine ("Refill");
		}
	}

	IEnumerator Ice() {
		if (Count > 0) {
			playerIced = true;
			player.layer = LayerMask.NameToLayer ("IcePlayer");
			while (Count > 0) {
				yield return new WaitForSeconds (iceTime);
				Count--;
				if (render && !render.enabled) {
					break;
				}
			}
			StopCoroutine ("Refill");
			StartCoroutine ("Refill");
		}
	}

	IEnumerator Refill() {
		playerIced = false;
		player.layer = LayerMask.NameToLayer ("Player");
		if (Count < 100) {
			yield return new WaitForSeconds (refillPauseTime);
			while (Count < 100) {
				Count++;
				yield return new WaitForSeconds (refillTime);
			}
		}
	}
}
