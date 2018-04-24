﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSight : EnemySight {

	int iceLayer;

	protected override void Start() {
		base.Start();
		iceLayer = LayerMask.NameToLayer("IcePlayer");
	}

	protected override void Update() {
		base.Update ();
	}

	protected override void CheckSightline () {
		frames = (frames + 1) % numFramesToResetPath;
		if (playerMovement != null && manager.Movement != null) {
			if (frames == (numFramesToResetPath - 1)) {
				Vector3 toPlayer = playerMovement.transform.position - gameObject.transform.position;
				Vector3 front = gameObject.transform.forward;
				float angle = Vector3.Angle (front.normalized, toPlayer.normalized);
				float fov = (manager.Movement.Alerted) ? alertedFOV : FOV;
				if (angle <= fov && toPlayer.magnitude <= sightDistance) {
					RaycastHit hit;
					if (Physics.Raycast(transform.position, toPlayer, out hit, Mathf.Infinity, ignoreEnemiesLayer)) {
						if (hit.transform.CompareTag ("Player") == true && (hit.transform.gameObject.layer != iceLayer)) {
							List<int> pathToPlayer = manager.Graph.FindShortestPath (manager.Movement.CurrVertexIndex, playerMovement.CurrVertexIndex);
							if (pathToPlayer.Count > 0) {
								manager.Movement.Alerted = true;
								manager.Movement.Path = pathToPlayer;
							}
						}
					}
				}
			}
		}
	}
}
