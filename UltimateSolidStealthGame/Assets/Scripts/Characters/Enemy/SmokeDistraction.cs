using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDistraction : EnemyDistraction {

	[SerializeField]
	float smokeTime;

	public override void Start () {
		base.Start ();
		nonDistractionLayers = 1 << LayerMask.NameToLayer ("Smoke");
	}

	protected override void CheckForDistraction () {
		Collider[] hits = Physics.OverlapSphere (transform.position, checkRadius, nonDistractionLayers, QueryTriggerInteraction.Collide);
		if (hits.Length == 0) {
			distToDistraction = Mathf.Infinity;
			return;
		}
		foreach (Collider hit in hits) {
			CigarettePack cig = hit.GetComponent<CigarettePack> ();
			if (cig) {
				if (Vector3.Distance (transform.position, hit.transform.position) < distToDistraction) {
					GameObject obj = hit.gameObject;
					SetDistraction (cig.Location, ref obj);
				}
			}
		}
	}

	protected override IEnumerator AtDistraction () {
		//smoking animation
		yield return new WaitForSeconds (smokeTime);
		CheckForDistraction ();
		enabled = true;
		manager.Movement.enabled = true;
		manager.Sight.enabled = true;
		manager.WeaponSystem.enabled = true;
	}
}
