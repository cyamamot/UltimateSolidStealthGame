using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
	
	public List<int> patrolVertices = new List<int> ();

	int destPatrolIndex;
	Graph graph;
	UnityEngine.AI.NavMeshAgent nav;
	List<int> path;
	Enums.directions direction;
	int lastVertexIndex;
	int currVertexIndex;
	string name;

	// Use this for initialization
	void Start() {
		GameObject temp = GameObject.FindGameObjectWithTag ("Graph");
		if (temp) {
			graph = temp.GetComponent<Graph> ();
			if (graph) {
				nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
				transform.position.Set (transform.position.x, 0.0f, transform.position.z);
				currVertexIndex = graph.GetIndexFromPosition (transform.position);
				lastVertexIndex = currVertexIndex;
				name = gameObject.name;
				graph.vertices [currVertexIndex].occupiedBy = name;
				graph.vertices [currVertexIndex].occupied = true;
				if (patrolVertices.Count > 0) {
					path = graph.FindShortestPath (currVertexIndex, patrolVertices [0]);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (graph.ready == true) {
			OnPatrol ();
			TravelBetweenPatrolPoints ();
		}
	}

	void OnPatrol() {
		if (patrolVertices.Count > 1) {
			int patrolIndexInGraph = patrolVertices [destPatrolIndex];
			Vertex v = graph.vertices[patrolIndexInGraph];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (currX == destX && currZ == destZ) {
				destPatrolIndex = (destPatrolIndex + 1) % patrolVertices.Count;
				path = graph.FindShortestPath (patrolIndexInGraph, patrolVertices[destPatrolIndex]);
			}
		}
	}

	void TravelBetweenPatrolPoints() {      
		if (path != null && path.Count > 0) {
			Vertex v = graph.vertices[path[0]];
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (nav.remainingDistance <= 0.1f) {
				if (lastVertexIndex != currVertexIndex) {
					if (graph.vertices[lastVertexIndex].occupiedBy == name) {
						graph.vertices [lastVertexIndex].occupied = false;
						graph.vertices [lastVertexIndex].occupiedBy = "";
					}
					graph.vertices [currVertexIndex].occupied = true;
					graph.vertices [currVertexIndex].occupiedBy = name;
				}
				if (path.Count > 1) {
                    if (graph.vertices[path[1]].occupied == true) { 
						return;
					}
				}
				path.RemoveAt (0);
				if (path.Count > 0 && nav != null) {
                    lastVertexIndex = currVertexIndex;
                    Vector3 moveDir = graph.vertices[path[0]].position - graph.vertices[currVertexIndex].position;
					transform.rotation = Quaternion.LookRotation(moveDir);
                    if (moveDir.x > 0) {
                        currVertexIndex += 1;
                    } else if (moveDir.x < 0) {
                        currVertexIndex -= 1;
                    } else if (moveDir.z > 0) {
                        currVertexIndex += graph.gridWidth;
                    } else if (moveDir.z < 0) {
                        currVertexIndex -= graph.gridWidth;
                    }
					graph.vertices [lastVertexIndex].occupied = true;
					graph.vertices [lastVertexIndex].occupiedBy = name;
					graph.vertices [currVertexIndex].occupied = true;
					graph.vertices [currVertexIndex].occupiedBy = name;
					nav.SetDestination (graph.vertices[path[0]].position);
				}
			}
		}
	}
}
