using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightPlane : MonoBehaviour {

	[SerializeField]
	int numRays;
	[SerializeField]
	int alertedNumRays;
	[SerializeField]
	GameObject planePrefab;

	GameObject sightPlane;
	Mesh sightPlaneMesh;
	int defaultLayer;
	EnemyManager manager;
	List<Vector3> newVerts;
	List<int> newIndices;

	void Start () {
		manager = GetComponent<EnemyManager> ();
		newVerts = new List<Vector3> ();
		newIndices = new List<int> ();
		defaultLayer = 1 << LayerMask.NameToLayer ("Default");
		sightPlane = Instantiate (planePrefab);
		sightPlane.transform.position = Vector3.zero;
		Destroy(sightPlane.GetComponent<MeshCollider> ());
		sightPlaneMesh = sightPlane.GetComponent<MeshFilter> ().mesh;
		sightPlaneMesh.Clear ();
	}

	void Update () {
		CreatePlane ();
	}

	void CreatePlane() {
		sightPlaneMesh.Clear ();
		newIndices.Clear ();
		newVerts.Clear ();
		newVerts.Add (transform.position);
		int fov = manager.Sight.CurrentFOV;
		int rays = (manager.Sight.Alerted) ? alertedNumRays : numRays;
		float currAngle = -fov;
		float deltaAngle = (fov * 2.0f) / (rays - 1);
		for (int i = 0; i < rays; i++) {
			Vector3 dir = Quaternion.AngleAxis (currAngle, Vector3.up) * transform.forward;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, dir, out hit, manager.Sight.SightDistance, defaultLayer)) {
				newVerts.Add (hit.point);
			} else {
				newVerts.Add (transform.position + (manager.Sight.SightDistance * dir));
			}
			if (i == 0) {
				newIndices.Add (i);
			} else if (i != rays - 1) {
				newIndices.Add (i);
				newIndices.Add (0);
				newIndices.Add (i);
			} else {
				newIndices.Add (i);
				newIndices.Add (0);
			}
			currAngle += deltaAngle;
		}
		sightPlaneMesh.vertices = newVerts.ToArray ();
		sightPlaneMesh.triangles = newIndices.ToArray ();
	}
}
