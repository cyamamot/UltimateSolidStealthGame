using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Subclass of EnemySight component used by Soldier type enemies
*/
public class SoldierSight : EnemySight{

	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update ();
	}

	/*
		Soldier implementation of base class function
	*/
	protected override void CheckSightline () {
		frames = (frames + 1) % numFramesToResetPath;
		if (playerMovement != null && manager.Movement != null) {
			if (frames == (numFramesToResetPath - 1)) {
				Vector3 toPlayer = playerMovement.transform.position - gameObject.transform.position;
				Vector3 front = gameObject.transform.forward;
				float angle = Vector3.Angle (front.normalized, toPlayer.normalized);
				currentFOV = (alerted) ? alertedFOV : FOV;
				if (angle <= currentFOV && toPlayer.magnitude <= sightDistance) { 
					RaycastHit hit;
                    if (!alerted) {
                        if (Physics.Raycast(transform.position, toPlayer, out hit, Mathf.Infinity, sightLayer)) {
                            if (hit.transform.CompareTag("Player")) {
                                SeePlayer();
                            }
                        }
					} else {
                        if (Physics.Raycast(transform.position, toPlayer, out hit, Mathf.Infinity, specialSightLayer)) {
                            if (hit.transform.CompareTag("Player")) {
                                SeePlayer();
                            }
                        }
                    }
				}
			}
		}
	}

    void SeePlayer() {
        if (manager.IsBoss) {
            pathToPlayer = manager.Graph.FindShortestPath(manager.Movement.CurrVertexIndex, playerMovement.ParentVertexIndex);
        } else {
            pathToPlayer = manager.Graph.FindShortestPath(manager.Movement.CurrVertexIndex, playerMovement.CurrVertexIndex);
        }
        if (reporter) reporter.ReportToManager();
        if (pathToPlayer.Count > 0) {
            if (!Alerted) manager.ShowMark("Exclamation");
            Alerted = true;
            if (manager.Distraction) {
                manager.Distraction.ResetDistraction();
            }
            manager.Movement.Path = pathToPlayer;
        }
    }
}
