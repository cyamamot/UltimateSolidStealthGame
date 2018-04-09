using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
	
	public List<int> patrolVertices = new List<int> ();

	int destPatrolIndex;
	Graph graph;
	UnityEngine.AI.NavMeshAgent nav;
	Queue<Vertex> path = new Queue<Vertex> ();
	Enums.directions direction;
	int currVertexIndex;

	// Use this for initialization
	void Start() {
		GameObject temp = GameObject.FindGameObjectWithTag ("Graph");
		if (temp) {
			graph = temp.GetComponent<Graph> ();
			if (graph) {
				nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
				transform.position.Set (transform.position.x, 0.0f, transform.position.z);
				currVertexIndex = graph.GetIndexFromPosition (transform.position);
				path = graph.FindShortestPath (currVertexIndex, patrolVertices [0]);
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
			Vertex v = path.Peek();
			float currX = transform.position.x;
			float currZ = transform.position.z;
			float destX = v.position.x;
			float destZ = v.position.z;
			if (currX == destX && currZ == destZ) {
				path.Dequeue ();
				if (path.Count > 0 && nav != null) {
					nav.SetDestination (path.Peek().position);
				}
			}
		}
	}
}
