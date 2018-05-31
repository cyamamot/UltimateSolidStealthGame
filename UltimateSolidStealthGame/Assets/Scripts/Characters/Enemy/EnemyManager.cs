using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class used to maintain references to all enemy components
	Used by components to access one another
*/
public class EnemyManager : CharacterManager {

    [SerializeField]
    bool isBoss;

    /*
		references to major Enemy components
	*/
    protected EnemyMovement movement;
    protected EnemySight sight;
    protected EnemyWeaponSystem weaponSystem;
    protected EnemyDistraction distraction;
    protected EnemySightPlane plane;
    /*
		reference to player GameObject
	*/
    protected GameObject player;
    protected PlayerMovement playerMovement;

    public bool IsBoss {
        get { return isBoss; }
    }
	public EnemyMovement Movement {
		get { return movement; }
	}
	public EnemySight Sight {
		get { return sight; }
	}
	public EnemyWeaponSystem WeaponSystem {
		get { return weaponSystem; }
	}
	public GameObject Player {
		get { return player; }
	}
	public EnemyDistraction Distraction {
		get { return distraction; }
	}

	protected override void Awake () {
		base.Awake ();
        GameObject parent = (transform.parent != null) ? transform.parent.gameObject : null;
		sight = (parent != null) ? parent.GetComponentInChildren<EnemySight> () : GetComponent<EnemySight>();
		movement = (parent != null) ? parent.GetComponentInChildren<EnemyMovement>() : GetComponent<EnemyMovement>(); ;
		weaponSystem = (parent != null) ? parent.GetComponentInChildren<EnemyWeaponSystem>() : GetComponent<EnemyWeaponSystem>();
        distraction = (parent != null) ? parent.GetComponentInChildren<EnemyDistraction>() : GetComponent<EnemyDistraction>();
        plane = (parent != null) ? parent.GetComponentInChildren<EnemySightPlane>() : GetComponent<EnemySightPlane>();
        player = GameObject.FindGameObjectWithTag ("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
	}

    protected virtual void DisableComponents() {
        movement.StopAllCoroutines();
        if (distraction) distraction.StopAllCoroutines();
        if (movement) movement.enabled = false;
        if (sight) sight.enabled = false;
        if (weaponSystem) weaponSystem.enabled = false;
        if (health) health.enabled = false;
        if (distraction) distraction.enabled = false;
        movement.Nav.isStopped = true;
        movement.Nav.ResetPath();
        if (plane) plane.enabled = false;
    }

	/*
		Called to turn off all components when enemy is killed
	*/
	public override void Kill() {
		Debug.Log ("Enemy Dead");
		graph.vertices[movement.CurrVertexIndex].occupied = false;
		graph.vertices[movement.CurrVertexIndex].occupiedBy = "";
        if (graph.vertices[movement.LastVertexIndex].occupiedBy == name) {
            graph.vertices[movement.LastVertexIndex].occupied = false;
            graph.vertices[movement.LastVertexIndex].occupiedBy = "";
        }
        DisableComponents();
		GetComponent<Collider> ().enabled = false;
	}

    public override void OnTakeDamage() {
        distraction.ResetDistraction();
        sight.SetSightOnPlayer();
    }
}
