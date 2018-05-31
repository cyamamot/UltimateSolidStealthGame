using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour {

    [SerializeField]
    GameObject firstSnakeSegment;

    EnemyManager manager;
    SnakeSight sight;
    Vector3 posYOffset;

    void Start() {
        manager = firstSnakeSegment.GetComponentInParent<EnemyManager>();
        sight = (SnakeSight)manager.Sight;
        posYOffset = transform.position - manager.transform.position;
    }

	
	void LateUpdate () {
        transform.position = manager.transform.position + posYOffset;
        if (manager.Alive) {
            if (manager.Sight.Alerted) {
                StopAllCoroutines();
                Vector3 toPlayer = sight.PlayerLastSeenLoc - manager.transform.position;
                toPlayer[1] = transform.forward[1];
                transform.forward = Vector3.RotateTowards(transform.forward, toPlayer, 0.05f, 0.0f);
            }
            else if (manager.Distraction.Distracted) {
                if (manager.Distraction.Distraction) {
                    StopAllCoroutines();
                    Vector3 toDistraction = manager.Distraction.Distraction.transform.position - manager.transform.position;
                    toDistraction[1] = transform.forward[1];
                    transform.forward = Vector3.RotateTowards(transform.forward, toDistraction, 0.05f, 0.0f);
                }
            }
        }
	}

    public void Turn(Vector3 dir) {
        if (!manager.Sight.Alerted && !manager.Distraction.Distracted) {
            StopAllCoroutines();
            StartCoroutine(TurnToward(dir));
        }
    }

    IEnumerator TurnToward(Vector3 dir) {
        float signedAngle = Vector3.SignedAngle(transform.forward.normalized, dir.normalized, Vector3.up);
        while (Mathf.Abs(signedAngle) >= 1.0f) {
            yield return transform.forward = Vector3.RotateTowards(transform.forward, manager.transform.forward, 0.125f, 0.0f);
            signedAngle = Vector3.SignedAngle(transform.forward, manager.transform.forward, Vector3.up);
        }
    }
}
