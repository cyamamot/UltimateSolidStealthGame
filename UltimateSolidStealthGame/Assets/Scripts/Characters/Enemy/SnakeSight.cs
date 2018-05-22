using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSight : EnemySight {

    Vector3 playerLastSeenLoc;

    public Vector3 PlayerLastSeenLoc {
        get { return playerLastSeenLoc; }
    }

    protected override void Start() {
        playerLastSeenLoc = new Vector3();
        base.Start();
    }

    protected override void Update() {
        CheckSightline();
        if (alerted && pathToPlayer.Count == 0) {
            if (!FinalSightCheck()) {
                alerted = false;
                manager.Movement.PauseMovement(alertedPauseLength);
            } else {
                manager.Movement.Path.Add(manager.Movement.CurrVertexIndex);
            }
        }
    }

    protected override void CheckSightline() {
        frames = (frames + 1) % numFramesToResetPath;
        if (playerMovement != null && manager.Movement != null) {
            if (frames == (numFramesToResetPath - 1)) {
                Vector3 toPlayer = playerMovement.transform.position - gameObject.transform.position;
                Vector3 front = gameObject.transform.forward;
                float angle = Vector3.Angle(front.normalized, toPlayer.normalized);
                currentFOV = (alerted) ? alertedFOV : FOV;
                if (angle <= currentFOV && toPlayer.magnitude <= sightDistance) {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, toPlayer, out hit, Mathf.Infinity, ignoreEnemiesLayer)) {
                        if (hit.transform.CompareTag("Player")) {
                            pathToPlayer = manager.Graph.FindShortestPath(manager.Movement.CurrVertexIndex, playerMovement.ParentVertexIndex);
                            if (pathToPlayer.Count > 0) {
                                alerted = true;
                                playerLastSeenLoc = playerMovement.CurrLocation;
                                if (manager.Distraction) {
                                    manager.Distraction.Distracted = false;
                                }
                                manager.Movement.Path = pathToPlayer;
                            }
                        }
                    }
                }
            }
        }
    }

    bool FinalSightCheck() {
        float angle = 0;
        if (playerMovement != null && manager.Movement != null) {
            Vector3 toPlayer = playerMovement.transform.position - gameObject.transform.position;
            Vector3 front = gameObject.transform.forward;
            angle = Vector3.Angle(front.normalized, toPlayer.normalized);
            currentFOV = (alerted) ? alertedFOV : FOV;
            if (angle <= currentFOV && toPlayer.magnitude <= sightDistance) {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, toPlayer, out hit, Mathf.Infinity, ignoreEnemiesLayer)) {
                    if (hit.transform.CompareTag("Player")) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
