using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class used to maintain references to all enemy components
	Used by components to access one another
*/
public class EnemyManager : CharacterManager {

	/*
		references to major Enemy components
	*/
	EnemyMovement movement;
	EnemySight sight;
	EnemyWeaponSystem weaponSystem;
	EnemyDistraction distraction;
	EnemySightPlane plane;
	/*
		reference to player GameObject
	*/
	GameObject player;

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
        GameObject parent = transform.parent.gameObject;
		sight = (parent != null) ? parent.GetComponentInChildren<EnemySight> () : GetComponent<EnemySight>();
		movement = (parent != null) ? parent.GetComponentInChildren<EnemyMovement>() : GetComponent<EnemyMovement>(); ;
		weaponSystem = (parent != null) ? parent.GetComponentInChildren<EnemyWeaponSystem>() : GetComponent<EnemyWeaponSystem>();
        distraction = (parent != null) ? parent.GetComponentInChildren<EnemyDistraction>() : GetComponent<EnemyDistraction>();
        plane = (parent != null) ? parent.GetComponentInChildren<EnemySightPlane>() : GetComponent<EnemySightPlane>();
        player = GameObject.FindGameObjectWithTag ("Player");
	}

	/*
		Called to turn off all components when enemy is killed
	*/
	public override void Kill() {
		Debug.Log ("Enemy Dead");
		graph.vertices[movement.CurrVertexIndex].occupied = false;
		graph.vertices[movement.CurrVertexIndex].occupiedBy = "";
		graph.vertices[movement.LastVertexIndex].occupied = false;
		graph.vertices[movement.LastVertexIndex].occupiedBy = "";
		movement.enabled = false;
		sight.enabled = false;
		weaponSystem.enabled = false;
		health.enabled = false;
		distraction.enabled = false;
		movement.Nav.enabled = false;
        plane.enabled = false;
		GetComponent<Collider> ().enabled = false;
	}
}
