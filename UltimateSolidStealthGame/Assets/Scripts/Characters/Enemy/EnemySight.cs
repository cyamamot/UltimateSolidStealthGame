using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class used by enemies to see the player 
	If player is in enemy's line of sight, a path to them is set
*/
public abstract class EnemySight : MonoBehaviour {

	/*
	 	Distance the the enemy can see the player
	*/
	[SerializeField]
	protected int sightDistance;
	/*
	 	normal fov of player
	*/
	[SerializeField]
	protected int FOV;
	/*
	 	fov of player when they are alerted
	*/
	[SerializeField]
	protected int alertedFOV;
	/*
	 	number of frames between calls to find path to destination
	*/
	[SerializeField]
	protected int numFramesToResetPath;

    [SerializeField]
    protected float alertedPauseLength;

    [SerializeField]
    float runSpeed;

    float walkSpeed;

    /*
	 	whether the enemy is currently alerted
	*/
    protected bool alerted;
	/*
	 	current frame between 0 and numFramesToResetPath
	*/
	protected int frames;
	/*
	 	reference to player's PlayerMovement component
	*/
	protected PlayerMovement playerMovement;
	/*
	 	reference to enemy's EnemyManager component
	*/
	protected EnemyManager manager;
	/*
	 	layer that defines all non layers in sight
	*/
	protected int sightLayer;
	/*
	 	list used to store indices of vertices that make up path to player
	*/
	protected List<int> pathToPlayer;
	/*
	 	the enemy's FOV based on whether they see the player or not
	*/
	protected int currentFOV;

    protected int specialSightLayer;

    protected AlertedReporter reporter;

    public int SightLayer {
		get { return sightLayer; }
	}
    public int SpecialSightLayer {
        get { return specialSightLayer; }
    }
	public bool Alerted {
		get { return alerted; }
		set {
            alerted = value;
            if (alerted) {
                manager.Movement.Nav.speed = runSpeed;
            } else {
                manager.Movement.Nav.speed = walkSpeed;
            }
        }
	}
	public int CurrentFOV {
		get { return currentFOV; }
	}
	public int SightDistance {
		get { return sightDistance; }
	}
    public float AlertedPauseLength {
        get { return alertedPauseLength; }
    }

    protected virtual void Start () {
        sightLayer = 1 << LayerMask.NameToLayer ("Enemy");
        sightLayer += (1 << LayerMask.NameToLayer("Ignore Raycast"));
		sightLayer = ~sightLayer;
        specialSightLayer = (~sightLayer) + (1 << LayerMask.NameToLayer("HidingPlace"));
        specialSightLayer = ~specialSightLayer;
        GameObject temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp) {
			playerMovement = temp.GetComponent<PlayerMovement> ();
		}
		currentFOV = FOV;
        GameObject parent = (transform.parent != null) ? transform.parent.gameObject : null;
        manager = (parent != null) ? parent.GetComponentInChildren<EnemyManager>() : GetComponent<EnemyManager>();
        walkSpeed = manager.Movement.Nav.speed;
        runSpeed = (runSpeed >= 0.0f) ? runSpeed : walkSpeed;
        pathToPlayer = new List<int> ();
        reporter = GetComponent<AlertedReporter>();
	}

	protected virtual void Update() {
		CheckSightline ();
		if (alerted && pathToPlayer.Count == 0) {
			Alerted = false;
            manager.ShowMark("Question");
            manager.Movement.PauseMovement (alertedPauseLength);
		}
	}

	/*
		Checks whether the player is currently in enemy's direct line of sight based on their FOV
		If they are, set the enemy's path to the player
	*/
	protected abstract void CheckSightline ();

    public virtual void SetSightOnPlayer() {
        if (manager.IsBoss) {
            pathToPlayer = manager.Graph.FindShortestPath(manager.Movement.CurrVertexIndex, playerMovement.ParentVertexIndex);
        } else {
            pathToPlayer = manager.Graph.FindShortestPath(manager.Movement.CurrVertexIndex, playerMovement.CurrVertexIndex);
            if (!Alerted) manager.ShowMark("Exclamation");
        }
        Alerted = true;
        manager.Movement.HeardSound = false;
        if (reporter) reporter.ReportToManager();
        if (pathToPlayer.Count > 0) {
            manager.Movement.Path = pathToPlayer;
        }
    }
}
