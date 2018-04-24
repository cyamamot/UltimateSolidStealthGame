using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CigarettePack : MonoBehaviour {

	BoxCollider collider;
	Graph graph;
	[SerializeField]
	int location;

	public int Location {
		get { return location; }
		set { location = value; }
	}

	void Start () {
		transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
		graph = GameObject.FindGameObjectWithTag ("Graph").GetComponent<Graph> ();
		location = graph.GetIndexFromPosition (transform.position);
		collider = GetComponent<BoxCollider> ();
	}

	void OnTriggerEnter (Collider other) {
		SmokeDistraction sd = other.gameObject.GetComponent<SmokeDistraction> ();
		if (sd && sd.Distracted) {
			Invoke ("DestroyCig", 0.25f);
		}
	}

	void DestroyCig() {
		Destroy (gameObject);
	}
}
