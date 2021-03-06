﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Subclass of EnemyDistraction class
	Used to detect whether a laser pointer distraction is in range
	If one is found, set path to it
*/
public class LaserDistraction : EnemyDistraction {

	public override void Start () {
		base.Start ();
		distractionLayers = 1 << LayerMask.NameToLayer ("Laser");
	}

	protected override void LateUpdate() {
		base.LateUpdate ();
	}

	/*
	 	Uses Physics.Checksphere to detect any laser pointer in range
	 	if one is found, check whether there is direct line of sight to distraction
	 	if it is in the line of sight, set path to laser
	*/
	protected override void CheckForDistraction () {
		Collider[] hits = Physics.OverlapSphere (transform.position, checkRadius, distractionLayers, QueryTriggerInteraction.Collide);
		foreach (Collider hit in hits) {
			if (!Physics.Linecast (transform.position, hit.transform.position, manager.Sight.SightLayer, QueryTriggerInteraction.Ignore)) {
				LaserTarget target = hit.GetComponent<LaserTarget> ();
				if (target) {
					GameObject obj = hit.gameObject;
                    if (manager.IsBoss) {
                        SetDistraction(target.Vertex.parentVertex.index, ref obj);
                    } else {
                        SetDistraction(target.Vertex.index, ref obj);
                    }
				}
			}
		}
	}

	public override void SetDistraction (int vertex, ref GameObject obj) {
		base.SetDistraction (vertex, ref obj);
	}

	protected override IEnumerator AtDistraction () {
		manager.Movement.Turn(distraction.transform.forward);
        isAtDistraction = true;
		//laser animation
		Destroy(distraction);
		yield return new WaitForSeconds (distractionTime);
        ResetDistraction();
	}

    public override void ResetDistraction() {
        isAtDistraction = false;
        StopCoroutine(AtDistraction());
        if (manager.Movement) manager.Movement.enabled = true;
        if (manager.Sight) manager.Sight.enabled = true;
        if (manager.WeaponSystem) manager.WeaponSystem.enabled = true;
        if (pathToDistraction.Count == 0) manager.Movement.BackToPatrol();
        enabled = true;
        distracted = false;
        pathToDistraction.Clear();
    }
}
