using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CigarettePack : MonoBehaviour {

	SphereCollider collider;
	Graph graph;
	int location;
	HashSet<EnemyDistraction> affectedEnemies;

	public int Location {
		set { location = value; }
	}

	void Start () {
		affectedEnemies = new HashSet<EnemyDistraction> ();
		collider = GetComponent<SphereCollider> ();
		transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
		graph = GameObject.FindGameObjectWithTag ("Graph").GetComponent<Graph> ();
	}

	void OnTriggerStay (Collider other) {
		EnemyDistraction edm = other.gameObject.GetComponent<EnemyDistraction> ();
		if (edm) {
			affectedEnemies.Add (edm);
			edm.SetDistraction (location);
		}
	}

	void OnDestroy() {
		foreach (EnemyDistraction ed in affectedEnemies) {
			ed.ClearDistraction ();
		}
	}
}
