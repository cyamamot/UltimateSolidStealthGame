using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
	Class that allows enemies to move between points on graph
*/
public class EnemyMovement : MonoBehaviour {

    [System.Serializable]
    public class PairStruct {
        public int index;
        public bool patrolHere;
        public Vector3 lookDir;
    }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PairStruct))]
    public class PairStructDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            var indexRect = new Rect(position.x, position.y, 75, position.height);
            var patrolHereRect = new Rect(position.x + 80, position.y, 20, position.height);
            var lookDirRect = new Rect(position.x + 105, position.y, 125, position.height);
            EditorGUI.PropertyField(indexRect, property.FindPropertyRelative("index"), GUIContent.none);
            EditorGUI.PropertyField(patrolHereRect, property.FindPropertyRelative("patrolHere"), GUIContent.none);
            EditorGUI.PropertyField(lookDirRect, property.FindPropertyRelative("lookDir"), GUIContent.none);
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
    #endif

    /*
		time enemy should pause when they reach patrol point
	*/
    [SerializeField]
	protected int pauseLength;
	/*
		current vertex in graph that enemy is at
	*/
	[SerializeField]
    protected int currVertexIndex;
	/*
		list of indices of vertices that the enemy should patrol between
	*/
	[SerializeField]
    protected List<PairStruct> patrolVertices = new List<PairStruct> ();
	/*
		used for debugging, shows path enemy is currently traveling along
	*/
	[SerializeField]
    protected bool showDebug;

    /*
		index of vertex that the enemy was last at
	*/
    protected int lastVertexIndex;
    /*
		index of patrol vertex that the enemy should move to next
	*/
    protected int destPatrolIndex;
    /*
		primary path that defines path along which the enemy moves, either between patrols, to distractions, or to player
	*/
    protected List<int> path;
    /*
		current direction enemy is facing
	*/
    protected Enums.directions direction;
    /*
		enemy name
	*/
    protected string enemyName;
    /*
		last direction that the enemy moved in
		used to determine if enemy changed directions
	*/
    protected Vector3 lastMoveDir;

    /*
		reference to EnemyManager component
	*/
    protected EnemyManager manager;
    /*
		reference to NavMeshAgent component
	*/
    protected UnityEngine.AI.NavMeshAgent nav;

    protected AudioSource audioSource;
    /*
        number of times attempts to go to next patrol point should be made before changing patrol points
    */
    int maxPatrolTries = 50;
    int numPatrolTries;
    bool moving;


    public int CurrVertexIndex {
		get { return currVertexIndex; }
	}
	public int LastVertexIndex {
		get { return lastVertexIndex; }
	}
	public List<int> Path {
		get { return path; }
		set { path = value; }
	}
	public UnityEngine.AI.NavMeshAgent Nav {
		get { return nav; }
	}
    public bool Moving {
        get { return moving; }
        set { moving = value; }
    }

    void Awake() {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    protected void Start() {
		manager = GetComponent<EnemyManager> ();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
		path = new List<int> ();
		currVertexIndex = manager.Graph.GetIndexFromPosition (transform.position);
		lastVertexIndex = currVertexIndex;
		lastMoveDir = transform.forward;
		enemyName = gameObject.name;
		manager.Graph.vertices [currVertexIndex].occupiedBy = enemyName;
		manager.Graph.vertices [currVertexIndex].occupied = true;
        manager.Graph.vertices[currVertexIndex].NotifyParentOrChild();
        if (patrolVertices.Count > 0) {
			path = manager.Graph.FindShortestPath (currVertexIndex, patrolVertices[0].index);
		}
        moving = true;
	}

	/*
		Called every frame
		returns enemy to patrol if they are not alerted or distracted
		if showDebug == true, show path enemy is on
	*/
	protected void Update() {
		if (manager && manager.Graph.Ready) {
			/*if (manager.Sight && !manager.Sight.Alerted) {
				if (manager.Distraction && !manager.Distraction.Distracted) {
					BackToPatrol ();
				} else if (!manager.Distraction) {
					BackToPatrol ();
				}
			}*/
			TravelBetweenPathPoints ();
			OnPatrol ();
        }
        #if UNITY_EDITOR
        if (showDebug) {
            foreach (int i in path) {
                Vector3 pos = manager.Graph.vertices[i].position;
                Debug.DrawLine(pos, pos + (Vector3.up * 5.0f), Color.red, 0.01f);
            }
        }
        #endif
    }

    /*
		Sets new destination patrol vertex if enemy has reached current patrol
	*/
    protected void OnPatrol() {
        if (moving) {
            if (patrolVertices.Count >= 1) {
                if ((manager.Sight && !manager.Sight.Alerted) || !manager.Sight) {
                    if ((manager.Distraction && !manager.Distraction.Distracted) || !manager.Distraction) {
                        int patrolIndexInGraph = patrolVertices[destPatrolIndex].index;
                        Vertex v = manager.Graph.vertices[patrolIndexInGraph];
                        float currX = transform.position.x;
                        float currZ = transform.position.z;
                        float destX = v.position.x;
                        float destZ = v.position.z;
                        if ((Mathf.Abs(destX - currX) <= nav.stoppingDistance) && (Mathf.Abs(destZ - currZ) <= nav.stoppingDistance)) {
                            PairStruct currPatrolStruct = patrolVertices[destPatrolIndex];
                            if (currPatrolStruct.patrolHere) PauseMovement(pauseLength);
                            if (currPatrolStruct.lookDir != Vector3.zero) Turn(currPatrolStruct.lookDir);
                            destPatrolIndex = (destPatrolIndex + 1) % patrolVertices.Count;
                            BackToPatrol();
                        }
                    }
                }
            }
        }
	}

	/*
		Allows enemy to travel between vertices in graph along a certain path
		recalculates new destination as enemy approaches current destination
		sets appropriate occupant of current destination vertex and clears occupation of last occupied vertex
		if path.Count >= 2, turn in direction of next vertex in path if it is in a different direction
		update currVertex according to direction moved in
	*/
	protected virtual void TravelBetweenPathPoints() {
        if (moving) {
            if (path.Count > 0) {
                if (nav.remainingDistance <= nav.stoppingDistance) {
                    if (!audioSource.isPlaying) audioSource.Play();
                    if (lastVertexIndex != currVertexIndex) {
                        if (manager.Graph.vertices[lastVertexIndex].occupiedBy == enemyName) {
                            manager.Graph.vertices[lastVertexIndex].occupied = false;
                            manager.Graph.vertices[lastVertexIndex].occupiedBy = "";
                            manager.Graph.vertices[lastVertexIndex].NotifyParentOrChild();
                        }
                        manager.Graph.vertices[currVertexIndex].occupied = true;
                        manager.Graph.vertices[currVertexIndex].occupiedBy = enemyName;
                        manager.Graph.vertices[currVertexIndex].NotifyParentOrChild();
                    }
                    Vector3 moveDir;
                    if (path.Count >= 2) {
                        if (manager.Graph.vertices[path[1]].occupied) {
                            moveDir = manager.Graph.vertices[path[1]].position - manager.Graph.vertices[path[0]].position;
                            audioSource.Stop();
                            if (moveDir != lastMoveDir) {
                                Turn(moveDir);
                                lastMoveDir = moveDir;
                            }
                            return;
                        }
                    }
                    path.RemoveAt(0);
                    if (path.Count > 0 && nav != null) {
                        lastVertexIndex = currVertexIndex;
                        moveDir = manager.Graph.vertices[path[0]].position - manager.Graph.vertices[currVertexIndex].position;
                        if (moveDir != lastMoveDir) {
                            Turn(moveDir);
                        }
                        lastMoveDir = moveDir;
                        if (moveDir.x > 0) {
                            currVertexIndex += 1;
                        }
                        else if (moveDir.x < 0) {
                            currVertexIndex -= 1;
                        }
                        else if (moveDir.z > 0) {
                            currVertexIndex += manager.Graph.GridWidth;
                        }
                        else if (moveDir.z < 0) {
                            currVertexIndex -= manager.Graph.GridWidth;
                        }
                        manager.Graph.vertices[lastVertexIndex].occupied = true;
                        manager.Graph.vertices[lastVertexIndex].occupiedBy = enemyName;
                        manager.Graph.vertices[lastVertexIndex].NotifyParentOrChild();
                        manager.Graph.vertices[currVertexIndex].occupied = true;
                        manager.Graph.vertices[currVertexIndex].occupiedBy = enemyName;
                        manager.Graph.vertices[currVertexIndex].NotifyParentOrChild();
                        nav.SetDestination(manager.Graph.vertices[path[0]].position);
                    }
                }
            }
        }
	}

	/*
		sets path to next patrol point
        continue trying if path cannot be created until successful or number of tries reaches maxPatrolTries
        else go to next patrol point
	*/
	public void BackToPatrol() {
		if (patrolVertices.Count > 0) {
			List<int> newPath = manager.Graph.FindShortestPath (currVertexIndex, patrolVertices[destPatrolIndex].index);
			if (newPath.Count > 0) {
                numPatrolTries = 0;
                path = newPath;
			} else {
                numPatrolTries++;
                if (numPatrolTries == maxPatrolTries) {
                    numPatrolTries = 0;
                    destPatrolIndex = (destPatrolIndex + 1) % patrolVertices.Count;
                }
            }
		}
	}

	/*
		Pauses enemy movement
		if they are alerted or distracted during this time, coroutine ends
	*/
	public void PauseMovement(float length) {
        audioSource.Stop();
		StartCoroutine (Pause(length));
	}

    public void StopMovement(float time) {
        audioSource.Stop();
        StartCoroutine(Stop(time));
    }

    IEnumerator Stop(float time) {
        moving = false;
        yield return new WaitForSeconds(time);
        moving = true;
    }


    IEnumerator Pause(float length) {
		enabled = false;
		for (int i = 0; i < length; i++) {
			if (!manager.Sight.Alerted) {
				yield return new WaitForSeconds (0.1f);
			} else if (manager.Sight.Alerted || (manager.Distraction && manager.Distraction.Distracted) || path.Count != 0){
				enabled = true;
				yield break;
			}
		}
		if (path.Count == 0) {
			BackToPatrol ();
		}
		enabled = true;
	}

	/*
		turns enemy in direction
		@param dir - direction enemy should turn in
	*/
	public void Turn(Vector3 dir) {
		StopCoroutine ("TurnTowards");
		StartCoroutine ("TurnTowards", dir);
	}

	IEnumerator TurnTowards(Vector3 towards) {
		if (transform.forward.normalized != towards.normalized) {
			int count = 0;
			float angle = Vector3.SignedAngle (transform.forward.normalized, towards.normalized, Vector3.up);
			while (Mathf.Abs (Vector3.Angle (transform.forward.normalized, towards.normalized)) >= 15.0f && count <= 12) {
				if (angle < 0.0f) {
					transform.rotation *= Quaternion.Euler (0.0f, -15.0f, 0.0f);
				} else {
					transform.rotation *= Quaternion.Euler (0.0f, 15.0f, 0.0f);
				}
				count++;
				yield return null;
			}
			transform.rotation = Quaternion.LookRotation(towards);
		}
	}
}
