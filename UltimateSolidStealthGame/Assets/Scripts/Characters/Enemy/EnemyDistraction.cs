using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistraction : MonoBehaviour {

	float smokeTime = 500;
	EnemyManager manager;
	List<int> pathToDistraction;
	bool distracted;
	bool runningDistraction;

	public EnemyManager Manager {
		get { return manager; }
	}
	public bool Distracted {
		get { return distracted; }
		set { distracted = value; }
	}
	public float SmokeTime {
		set { smokeTime = value; }
	}

	void Start() {
		manager = GetComponent<EnemyManager> ();
		pathToDistraction = new List<int> ();
	}

	void Update() {
		if (pathToDistraction.Count == 0 && distracted && !runningDistraction) {
			runningDistraction = true;
			manager.Movement.enabled = false;
			manager.Sight.enabled = false;
			manager.WeaponSystem.enabled = false;
			//Smoking animation
			StartCoroutine("Smoking");
		}
	}

	public void SetDistraction(int vertex) {
		if (!distracted && !manager.Movement.Alerted) {
			distracted = true;
			pathToDistraction = manager.Graph.FindShortestPath (manager.Movement.CurrVertexIndex, vertex);
			if (pathToDistraction != null) {
				manager.Movement.Path = pathToDistraction;
			}
		}
	}

	public void ClearDistraction() {
		manager.Movement.enabled = true;
		manager.Sight.enabled = true;
		manager.WeaponSystem.enabled = true;
		runningDistraction = false;
		distracted = false;
		pathToDistraction.Clear();
		manager.Movement.Path.Clear();
		//end smoking animation
		StopCoroutine ("Smoking");
	}

	IEnumerator Smoking() {
		//smoking animation
		yield return new WaitForSeconds (smokeTime);
	}
}
