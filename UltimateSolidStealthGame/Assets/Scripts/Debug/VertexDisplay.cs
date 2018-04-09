using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexDisplay : MonoBehaviour {

	public Vertex vert;

	MeshRenderer mesh;
	Color startColor = Color.green;
	Color overColor = Color.red;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshRenderer> ();
		if (mesh) {
			mesh.material.color = startColor;
		}
	}

	void OnMouseOver() {
		if (mesh) {
			mesh.material.color = overColor;
			if (Input.GetMouseButtonDown(1)) {
				Debug.Log (vert.index + " : ");
				Debug.Log ("    position : " + vert.position);
				foreach (int i in vert.adjacentVertices) {
					Debug.Log ("    adjacent : " + i);
				}
			} else if (Input.GetMouseButtonDown(0)) {
				Debug.Log (vert.index + " : ");
			}
		}
	}

	void OnMouseExit() {
		if (mesh) {
			mesh.material.color = startColor;
		}
	}
}
