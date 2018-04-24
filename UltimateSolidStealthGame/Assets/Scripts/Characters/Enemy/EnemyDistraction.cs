using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDistraction : MonoBehaviour {

	[SerializeField]
	protected float checkRadius;
	[SerializeField]
	protected float timeBetweenChecks;
	[SerializeField]
	protected GameObject distraction;

	protected EnemyManager manager;
	protected List<int> pathToDistraction;
	[SerializeField]
	protected bool distracted;
	protected int nonDistractionLayers;
	[SerializeField]
	protected float distToDistraction;

	public EnemyManager Manager {
		get { return manager; }
	}
	public bool Distracted {
		get { return distracted; }
		set { distracted = value; }
	}

	public virtual void Start() {
		manager = GetComponent<EnemyManager> ();
		pathToDistraction = new List<int> ();
		distToDistraction = Mathf.Infinity;
		InvokeRepeating ("CheckForDistraction", 0.0f, timeBetweenChecks);
	}

	protected abstract void CheckForDistraction ();
		
	void LateUpdate () {
		if (distracted) {
			if (distraction && pathToDistraction.Count == 0) { 
				Vector3 pos = distraction.transform.position;
				if (transform.position.x == pos.x && transform.position.z == pos.z) {
				Debug.Log (gameObject.name + " here");
					enabled = false;
					manager.Movement.enabled = false;
					manager.Sight.enabled = false;
					manager.WeaponSystem.enabled = false;
					distracted = false;
					distToDistraction = Mathf.Infinity;
					//Smoking animation
					StartCoroutine ("AtDistraction");
				}
			} else if (!distraction) {
				distracted = false;
				distToDistraction = Mathf.Infinity;
				pathToDistraction.Clear ();
				manager.Movement.BackToPatrol ();
				return;
			}
		}
	}

	public void SetDistraction (int vertex, ref GameObject obj) {
		if (!manager.Movement.Alerted) {
			distraction = obj;
			distToDistraction = Vector3.Distance (transform.position, obj.transform.position);
			distracted = true;
			pathToDistraction = manager.Graph.FindShortestPath (manager.Movement.CurrVertexIndex, vertex);
			if (pathToDistraction.Count > 0) {
				manager.Movement.Path = pathToDistraction;
			}
		}
	}

	protected abstract IEnumerator AtDistraction () ;
}
