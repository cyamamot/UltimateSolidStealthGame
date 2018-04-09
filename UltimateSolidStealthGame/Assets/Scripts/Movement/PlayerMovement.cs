using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Graph graph;
	float moveAmount;
	Vector3 movement;
	UnityEngine.AI.NavMeshAgent nav;
	Enums.directions direction;
	int currVertexIndex;

	// Use this for initialization
	void Start () {
		movement = Vector3.zero;
		GameObject temp = GameObject.FindGameObjectWithTag ("Graph");
		if (temp) {
			graph = temp.GetComponent<Graph> ();
			if (graph) {
				moveAmount = graph.vertexDistance;
				nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
				transform.position.Set(transform.position.x, 0.0f, transform.position.z);
				currVertexIndex = graph.GetIndexFromPosition(transform.position);
			}
		}
	}

	void Update() {  
		SetNewDestination ();
	}
		
	void SetNewDestination () {
		if (nav != null) {
			Vector3 pos = new Vector3 (transform.position.x, 0.5f, transform.position.z);
			if (movement != Vector3.zero) {
				if (pos.x % moveAmount == 0.0f && pos.z % moveAmount == 0.0f) {
					if (!Physics.CheckSphere (transform.position + movement, 0.25f)) {
						nav.SetDestination (transform.position + movement);
					}
				}
			} else {
				if (pos.x % moveAmount == 0.0f && pos.z % moveAmount == 0.0f) {
					StopMoving ();
				}
			}
		}
	}

	public void MoveUntilStop(Enums.directions dir) {
		direction = dir;
		switch (dir) {
		case Enums.directions.left:
			movement.Set (-moveAmount, 0.0f, 0.0f);
			break;
		case Enums.directions.right:
			movement.Set (moveAmount, 0.0f, 0.0f);
			break;
		case Enums.directions.up:
			movement.Set (0.0f, 0.0f, moveAmount);
			break;
		case Enums.directions.down:
			movement.Set (0.0f, 0.0f, -moveAmount);
			break;
		}
	}

	public void StopMoving() {
		movement = Vector3.zero;
	}
}
