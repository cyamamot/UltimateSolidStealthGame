using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    [SerializeField]
    GameObject otherTarget;

	GameObject player;
    MeshRenderer otherRenderer;

	void Start () {
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
            } else {
                //transform.position = player.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.2f);
            }
		}
	}
}
