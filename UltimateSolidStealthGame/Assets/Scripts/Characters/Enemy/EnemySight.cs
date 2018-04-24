using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySight : MonoBehaviour {

	[SerializeField]
	protected int sightDistance = 6;
	[SerializeField]
	protected int FOV = 45;
	[SerializeField]
	protected int alertedFOV = 90;
	[SerializeField]
	protected int numFramesToResetPath = 10;

	protected int frames;
	protected PlayerMovement playerMovement;
	protected EnemyManager manager;
	protected int ignoreEnemiesLayer;

	public int IgnoreEnemiesLayer {
		get { return ignoreEnemiesLayer; }
	}
		
	protected virtual void Start () {
		frames = 0;
		ignoreEnemiesLayer = 1 << LayerMask.NameToLayer ("Enemy");
		ignoreEnemiesLayer = ~ignoreEnemiesLayer;
		GameObject temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp) {
			playerMovement = temp.GetComponent<PlayerMovement> ();
		}
		manager = gameObject.GetComponent<EnemyManager> ();
	}

	protected virtual void Update() {
		CheckSightline ();
	}

	protected abstract void CheckSightline ();
}
