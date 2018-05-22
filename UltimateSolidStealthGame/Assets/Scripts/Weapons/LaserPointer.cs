using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : Equipment {

	[SerializeField]
	GameObject targetPrefab;
	[SerializeField]
	Color laserUninvestigated;
	[SerializeField]
	Color laserInvestigated;

	LaserTarget target;
	int ignoreLayers;
	PlayerManager manager;
	LineRenderer laser;
	bool turnedOn;
    Camera cam;

	void Awake () {
		base.Awake ();
		count = -1;
		ignoreLayers = 1 << LayerMask.NameToLayer ("Default");
		laser = GetComponent<LineRenderer> ();
		laser.enabled = false;
		manager = GetComponentInParent<PlayerManager> ();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

	public override void UseEquipment() {
		if (manager.Movement) {
			if (!turnedOn) {
				RaycastHit hit;
				if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity, ignoreLayers, QueryTriggerInteraction.Collide)) {
                    Vector3 pointInCameraSpace = cam.WorldToViewportPoint(hit.point);
                    if (pointInCameraSpace[0] >= 0.0f && pointInCameraSpace[0] <= 1.0f
                            && pointInCameraSpace[1] >= 0.0f && pointInCameraSpace[1] <= 1.0f
                                && pointInCameraSpace[2] > 0.0f) {
                        turnedOn = true;
                        manager.Movement.StopMoving();
                        Vector3 offset = new Vector3();
                        float offsetAmount = manager.Graph.VertexDistance / 2.0f;
                        switch (manager.Movement.Direction) {
                            case Enums.directions.up:
                                offset.Set(0.0f, 0.0f, -offsetAmount);
                                break;
                            case Enums.directions.down:
                                offset.Set(0.0f, 0.0f, offsetAmount);
                                break;
                            case Enums.directions.left:
                                offset.Set(offsetAmount, 0.0f, 0.0f);
                                break;
                            case Enums.directions.right:
                                offset.Set(-offsetAmount, 0.0f, 0.0f);
                                break;
                        }
                        GameObject temp = Instantiate(targetPrefab, hit.point + offset, Quaternion.identity);
                        temp.transform.forward = manager.transform.forward;
                        target = temp.GetComponent<LaserTarget>();
                        target.Vertex = manager.Graph.vertices[manager.Graph.GetIndexFromPosition(hit.point + offset)];
                        target.Pointer = this;
                        LaserUninvestigated();
                        StartCoroutine("ShootRay", hit.point);
                    }
				}
			} else {
				turnedOn = false;
				laser.enabled = false;
				if (target) {
					Destroy (target.gameObject);
				}
				StopCoroutine ("ShootRay");
			}
		}
	}

	IEnumerator ShootRay(Vector3 pos) {
		laser.enabled = true;
		bool go = true;
		while (go) {
			if (manager.Movement.Movement == Vector3.zero) {
				laser.SetPositions (new Vector3[] { transform.position, pos });
			} else if (manager.Movement.Movement != Vector3.zero) {
				turnedOn = false;
				laser.enabled = false;
				if (target) {
					Destroy (target.gameObject);
				}
				StopCoroutine ("ShootRay");
			}
			yield return null;
		}
	}

	public void LaserInvestigated() {
		laser.startColor = laserInvestigated;
		laser.endColor = laserInvestigated;
	}

	public void LaserUninvestigated() {
		laser.startColor = laserUninvestigated;
		laser.endColor = laserUninvestigated;
	}
}
