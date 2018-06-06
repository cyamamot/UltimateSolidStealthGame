using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    [SerializeField]
    GameObject otherTarget;
    [SerializeField]
    float otherZoomDistance;

    float initialDistance;
	GameObject player;
    MeshRenderer otherRenderer;
    bool zoomedOut;

	void Start () {
        initialDistance = Camera.main.orthographicSize;
		player = GameObject.FindGameObjectWithTag ("Player");
        if (otherTarget) {
            otherRenderer = otherTarget.GetComponentInChildren<MeshRenderer>();
        }
        if (player != null) {
            if (otherRenderer && otherRenderer.isVisible) {
                Vector3 toOther = (otherRenderer.transform.position - player.transform.position);
                Vector3 offsetPos = player.transform.position + (0.25f * toOther);
                transform.position = player.transform.position + offsetPos;
            }
            else {
                transform.position = player.transform.position;
            }
        }
    }
		
	void LateUpdate () {
		if (player != null) {
            if (otherRenderer && otherRenderer.isVisible) {
                Vector3 toOther = (otherRenderer.transform.position - player.transform.position);
                Vector3 offsetPos = player.transform.position + (0.25f * toOther);
                transform.position = Vector3.MoveTowards(transform.position, offsetPos, 0.1f);
                if (!zoomedOut) {
                    zoomedOut = true;
                    StopAllCoroutines();
                    StartCoroutine(Zoom(otherZoomDistance));
                }
            } else {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.2f);
                if (zoomedOut) {
                    zoomedOut = false;
                    StopAllCoroutines();
                    StartCoroutine(Zoom(initialDistance));
                }
            }
		}
	}

    IEnumerator Zoom(float distance) {
        float currCamSize = Camera.main.orthographicSize;
        float delta = 0.0f;
        if (distance < currCamSize) {
            delta = -0.1f;
        } else {
            delta = 0.1f;
        }
        for (float i = currCamSize; i < distance; i += delta) {
            yield return Camera.main.orthographicSize = i;
        }
    }
}
