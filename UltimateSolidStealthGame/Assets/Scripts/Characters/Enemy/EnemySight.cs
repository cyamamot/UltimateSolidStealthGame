using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySight : MonoBehaviour {

	[SerializeField]
	protected int sightDistance;
	[SerializeField]
	protected int FOV;
	[SerializeField]
	protected int alertedFOV;
	[SerializeField]
	protected int numFramesToResetPath;

	protected bool alerted;
	protected int frames;
	protected PlayerMovement playerMovement;
	protected EnemyManager manager;
	protected int ignoreEnemiesLayer;
	protected List<int> pathToPlayer;
	protected int currentFOV;

	public int IgnoreEnemiesLayer {
		get { return ignoreEnemiesLayer; }
	}
	public bool Alerted {
		get { return alerted; }
		set { alerted = value; }
	}
	public int CurrentFOV {
		get { return currentFOV; }
	}
	public int SightDistance {
		get { return sightDistance; }
	}
		
	protected virtual void Start () {
		ignoreEnemiesLayer = 1 << LayerMask.NameToLayer ("Enemy");
		ignoreEnemiesLayer = ~ignoreEnemiesLayer;
		GameObject temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp) {
			playerMovement = temp.GetComponent<PlayerMovement> ();
		}
		currentFOV = FOV;
		manager = gameObject.GetComponent<EnemyManager> ();
		pathToPlayer = new List<int> ();
	}

	protected virtual void Update() {
		CheckSightline ();
		if (alerted && pathToPlayer.Count == 0) {
			alerted = false;
			manager.Movement.PauseMovement ();
		}
	}

	protected abstract void CheckSightline ();

}
