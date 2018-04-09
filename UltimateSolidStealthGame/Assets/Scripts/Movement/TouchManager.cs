using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

	public float touchRange;
	public float stopTouchDistance;

	PlayerMovement playerMovement;
	Vector2 screenMid;
	Vector2 zeroAngleLine;

	// Use this for initialization
	void Start () {
		GameObject tempObj = GameObject.FindGameObjectWithTag ("Player");
		if (tempObj) {
			playerMovement = tempObj.GetComponent<PlayerMovement> ();
		}
		float widthMid = Screen.width / 2.0f;
		float heightMid = Screen.height / 2.0f;
		screenMid = new Vector2 (widthMid, heightMid);
		zeroAngleLine = new Vector2 (1.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			Vector2 touchPos = Input.GetTouch (0).position - screenMid;
			Vector2 touchPosNorm = touchPos.normalized;
			if (touchPos.magnitude <= stopTouchDistance) {
				playerMovement.StopMoving ();
			} else {
				float touchAngle = Quaternion.FromToRotation (zeroAngleLine, touchPosNorm).eulerAngles.z;
				if (touchAngle <= touchRange || touchAngle >= 360.0f - touchRange) {
					//Debug.Log ("Right");
					playerMovement.MoveUntilStop (Enums.directions.right);
				} else if (touchAngle <= 90 + touchRange && touchAngle >= 90 - touchRange) {
					//Debug.Log ("Up");
					playerMovement.MoveUntilStop (Enums.directions.up);
				} else if (touchAngle <= 180 + touchRange && touchAngle >= 180 - touchRange) {
					//Debug.Log ("Left");
					playerMovement.MoveUntilStop (Enums.directions.left);
				} else if (touchAngle <= 270 + touchRange && touchAngle >= 270 - touchRange) {
					//Debug.Log ("Down");
					playerMovement.MoveUntilStop (Enums.directions.down);
				}
			}
		}
	}
}
