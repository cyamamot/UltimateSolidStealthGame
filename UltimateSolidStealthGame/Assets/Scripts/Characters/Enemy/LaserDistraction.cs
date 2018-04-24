using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDistraction : EnemyDistraction {

	[SerializeField]
	float laserTime;

	public override void Start () {
		base.Start ();
	}

	protected override void CheckForDistraction () {
		
	}

	protected override IEnumerator AtDistraction () {
		//smoking animation
		yield return new WaitForSeconds (laserTime);
		manager.Movement.enabled = true;
		manager.Sight.enabled = true;
		manager.WeaponSystem.enabled = true;
		pathToDistraction.Clear ();
	}
}
