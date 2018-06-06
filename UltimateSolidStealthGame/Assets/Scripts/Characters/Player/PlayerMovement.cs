﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class that defines player movement ability
	continues to move player in specified direction until told to stop
*/
public class PlayerMovement : MonoBehaviour {

	/*
		index of vertex player is currently at
	*/
	[SerializeField]
	int currVertexIndex;

	/*
		index of vertex player was last at
	*/
	int lastVertexIndex;
	/*
		amount by which player moves per move call
	*/
	float moveAmount;
	/*
		normalized direction of movement
	*/
	Vector3 movement;
	/*
		direction of movement
	*/
	Enums.directions direction;
	/*
		name of player
	*/
	string playerName;

	/*
		reference to PlayerManager component
	*/
	PlayerManager manager;
	/*
		reference to NavMeshAgent component
	*/
	UnityEngine.AI.NavMeshAgent nav;
    AudioSource audioSource;

	public int CurrVertexIndex {
		get { return currVertexIndex; }
	}
    public Vector3 CurrLocation {
        get { return manager.Graph.vertices[currVertexIndex].position; }
    }
	public Vector3 Movement {
		get { return movement; }
	}
	public UnityEngine.AI.NavMeshAgent Nav {
		get { return nav; }
	}
	public Enums.directions Direction {
		get { return direction; }
	}
    public int ParentVertexIndex {
        get {
            Vertex parentVertex = manager.Graph.vertices[currVertexIndex].parentVertex;
            if (parentVertex != null) {
                return parentVertex.index;
            } else {
                return -1;
            }
        }
    }
		
	void Start () {
		movement = Vector3.zero;
		manager = GetComponent<PlayerManager> ();
		moveAmount = manager.Graph.VertexDistance;
		nav = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
		currVertexIndex = manager.Graph.GetIndexFromPosition(transform.position);
		lastVertexIndex = currVertexIndex;
		playerName = gameObject.name;
		manager.Graph.vertices [currVertexIndex].occupiedBy = playerName;
		manager.Graph.vertices [currVertexIndex].occupied = true;
        manager.Graph.vertices[currVertexIndex].NotifyParentOrChild();
        Vector3 forwardDir = transform.forward.normalized;
		if (forwardDir == Vector3.forward) {
			direction = Enums.directions.up;
		} else if (forwardDir == Vector3.back) {
			direction = Enums.directions.down;
		} else if (forwardDir == Vector3.left) {
			direction = Enums.directions.left;
		} else if (forwardDir == Vector3.right) {
			direction = Enums.directions.right;
		}
	}

	void Update() {  
		if (manager.Graph.Ready == true) {
			SetNewDestination ();
		}
	}

	/*
		sets new destination for player based on direction of movement specified
		updates vertex info in graph accordingly
		player moves in increments of moveAmount per call
	*/
	void SetNewDestination() {
		if (nav && manager && manager.Graph) {
			if (nav.remainingDistance <= 0.1f) {
                int nextVertex;
				if (lastVertexIndex != currVertexIndex) {
					if (manager.Graph.vertices[lastVertexIndex].occupiedBy == playerName) {
						manager.Graph.vertices [lastVertexIndex].occupied = false;
						manager.Graph.vertices [lastVertexIndex].occupiedBy = "";
                        manager.Graph.vertices[lastVertexIndex].NotifyParentOrChild();
                    }
					manager.Graph.vertices [currVertexIndex].occupied = true;
					manager.Graph.vertices [currVertexIndex].occupiedBy = playerName;
                    manager.Graph.vertices[currVertexIndex].NotifyParentOrChild();
                }
				if (movement != Vector3.zero) {
					transform.rotation = Quaternion.LookRotation(movement);
					switch (direction) {
					case Enums.directions.left:
                        nextVertex = currVertexIndex - 1;
                        if (nextVertex >= 0 && nextVertex < manager.Graph.GraphCount) {
                            if (manager.Graph.vertices[currVertexIndex - 1] != null) {
                                if (!audioSource.isPlaying) audioSource.Play();
                                if (manager.Graph.vertices[currVertexIndex - 1].occupied == true) {
                                    return;
                                }
                                lastVertexIndex = currVertexIndex;
                                currVertexIndex -= 1;
                            }
                            else {
                                StopMoving();
                            }
                        } else {
                            return;
                        }
						break;
					case Enums.directions.right:
                        nextVertex = currVertexIndex + 1;
                        if (nextVertex >= 0 && nextVertex < manager.Graph.GraphCount) {
                            if (manager.Graph.vertices[currVertexIndex + 1] != null) {
                                if (!audioSource.isPlaying) audioSource.Play();
                                if (manager.Graph.vertices[currVertexIndex + 1].occupied == true) {
                                    return;
                                }
                                lastVertexIndex = currVertexIndex;
                                currVertexIndex += 1;
                            } else {
                                StopMoving();
                            }
                        } else {
                            return;
                        }
						break;
					case Enums.directions.up:
                        nextVertex = currVertexIndex + manager.Graph.GridWidth;
                        if (nextVertex >= 0 && nextVertex < manager.Graph.GraphCount) {
                            if (!audioSource.isPlaying) audioSource.Play();
                            if (manager.Graph.vertices[nextVertex] != null) {
                                if (manager.Graph.vertices[nextVertex].occupied == true) {
                                    return;
                                }
                                lastVertexIndex = currVertexIndex;
                                currVertexIndex += manager.Graph.GridWidth;
                            }
                            else {
                                StopMoving();
                            }
                        } else {
                            return;
                        }
						break;
					case Enums.directions.down:
                        nextVertex = currVertexIndex - manager.Graph.GridWidth;
                        if (nextVertex >= 0 && nextVertex < manager.Graph.GraphCount) {
                            if (manager.Graph.vertices[nextVertex] != null) {
                                if (!audioSource.isPlaying) audioSource.Play();
                                if (manager.Graph.vertices[nextVertex].occupied == true) {
                                    return;
                                }
                                lastVertexIndex = currVertexIndex;
                                currVertexIndex -= manager.Graph.GridWidth;
                            }
                            else {
                                StopMoving();
                            }
                        } else {
                            return;
                        }
                        break;
                    }
					manager.Graph.vertices [lastVertexIndex].occupied = true;
					manager.Graph.vertices [lastVertexIndex].occupiedBy = playerName;
                    manager.Graph.vertices[lastVertexIndex].NotifyParentOrChild();
                    manager.Graph.vertices [currVertexIndex].occupied = true;
					manager.Graph.vertices [currVertexIndex].occupiedBy = playerName;
                    manager.Graph.vertices[currVertexIndex].NotifyParentOrChild();
                    nav.SetDestination (manager.Graph.vertices [currVertexIndex].position);
				} 
			}
		}
	}

	/*
		Sets direction to move in
		@param dir - direction to move in
	*/
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
            movement.Set(0.0f, 0.0f, -moveAmount);
			break;
		}
	}

	/*
		stops player from continuing movement
	*/
	public void StopMoving() {
		movement = Vector3.zero;
        if (audioSource.isPlaying) audioSource.Stop() ;
    }
}
