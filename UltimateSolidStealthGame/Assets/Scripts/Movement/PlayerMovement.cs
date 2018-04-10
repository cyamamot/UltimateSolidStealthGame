using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	Graph graph;
	float moveAmount;
	Vector3 movement;
	UnityEngine.AI.NavMeshAgent nav;
	Enums.directions direction;
	int lastVertexIndex;
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
				lastVertexIndex = currVertexIndex;
				graph.vertices [currVertexIndex].occupied = true;
			}
		}
	}

	void Update() {  
		SetNewDestination ();
	}

	/*void SetNewDestination() {
		if (nav != null && graph != null) {
			Vector3 pos = new Vector3 (transform.position.x, 0.5f, transform.position.z);
			if (movement != Vector3.zero) {
				if (pos.x % moveAmount == 0.0f && pos.z % moveAmount == 0.0f) {
					if (!Physics.CheckSphere (transform.position + movement, 0.25f)) {
						nav.SetDestination (transform.position + movement);
					} else {
						StopMoving ();
					}
				}
			} else {
				if (pos.x % moveAmount == 0.0f && pos.z % moveAmount == 0.0f) {
					StopMoving ();
				}
			}
		}
	}*/

	void SetNewDestination() {
		if (nav != null && graph != null) {
			Vector3 pos = new Vector3 (transform.position.x, 0.5f, transform.position.z);
			if (nav.remainingDistance == 0) {
				if (lastVertexIndex != currVertexIndex) {
					graph.vertices [lastVertexIndex].occupied = false;
					graph.vertices [currVertexIndex].occupied = true;
				}
				if (movement != Vector3.zero) {
					switch (direction) {
					case Enums.directions.left:
						if (graph.vertices [currVertexIndex - 1] != null && graph.vertices [currVertexIndex - 1].occupied == false) {
							lastVertexIndex = currVertexIndex;
							currVertexIndex -= 1;
							graph.vertices [lastVertexIndex].occupied = true;
							graph.vertices [currVertexIndex].occupied = true;
							nav.SetDestination (graph.vertices [currVertexIndex].position);
						} else {
							StopMoving ();
						}
						break;
					case Enums.directions.right:
						if (graph.vertices [currVertexIndex + 1] != null && graph.vertices [currVertexIndex + 1].occupied == false) {
							lastVertexIndex = currVertexIndex;
							currVertexIndex += 1;
							graph.vertices [currVertexIndex].occupied = true;
							nav.SetDestination (graph.vertices [currVertexIndex].position);
						} else {
							StopMoving ();
						}
						break;
					case Enums.directions.up:
						if (graph.vertices [currVertexIndex + graph.gridWidth] != null && graph.vertices [currVertexIndex + graph.gridWidth].occupied == false) {
							lastVertexIndex = currVertexIndex;
							currVertexIndex += graph.gridWidth;
							graph.vertices [currVertexIndex].occupied = true;
							nav.SetDestination (graph.vertices [currVertexIndex].position);
						} else {
							StopMoving ();
						}
						break;
					case Enums.directions.down:
						if (graph.vertices [currVertexIndex - graph.gridWidth] != null && graph.vertices [currVertexIndex - graph.gridWidth].occupied == false) {
							lastVertexIndex = currVertexIndex;
							currVertexIndex -= graph.gridWidth;
							graph.vertices [currVertexIndex].occupied = true;
							nav.SetDestination (graph.vertices [currVertexIndex].position);
						} else {
							StopMoving ();
						}
						break;
					}
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
