using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour{

    [SerializeField]
    protected float vertexDistance;
    [SerializeField]
    protected int graphCount;

    protected BoxCollider box;
	protected int width;
    protected int height;
    protected int gridWidth;
    protected int gridHeight;
    protected int floorTop;
    protected Vector3 floorBottomLeft;
    protected bool ready = false;

	public List<Vertex> vertices;

    public int Width {
        get { return width; }
    }
    public int Height {
        get { return height; }
    }
	public int GridWidth {
		get { return gridWidth; }
	}
    public int GridHeight {
        get { return gridHeight; }
    }
    public bool Ready {
		get { return ready; }
	}
	public float VertexDistance {
		get { return vertexDistance; }
	}
    public int FloorTop {
        get { return floorTop; }
    }
    public Vector3 FloorBottomLeft {
        get { return floorBottomLeft; }
    }
    public int GraphCount {
        get { return graphCount; }
    }
		
	public virtual void Awake () {
        box = GetComponent<BoxCollider>();
		if (box) {
            box.enabled = true;
			Vector3 size = box.bounds.size;
			width = (int)size [0];
			height = (int)size [2];
            floorTop = (int)(box.bounds.center[1] - (size[1] / 2.0f));
            floorBottomLeft = new Vector3(box.bounds.center[0] - (width / 2.0f), box.bounds.center[1], box.bounds.center[2] - (height / 2.0f));
			gridWidth = (int)(width * (1.0f / vertexDistance));
            gridHeight = (int)(height * (1.0f / vertexDistance));
            box.enabled = false;
			vertices = new List<Vertex> ();
            int checkLayer = 1 << LayerMask.NameToLayer("Default");
            int ignoreRaycastLayer = 1 << LayerMask.NameToLayer("Ignore Raycast");
            checkLayer += ignoreRaycastLayer;
			if (vertices.Count == 0) { 
				int count = 0;
				Vector3 pos = new Vector3 ();
				for (float i = (vertexDistance / 2.0f); i < (float)height; i += vertexDistance) {                         
					for (float j = (vertexDistance / 2.0f); j < (float)width; j += vertexDistance) {
						pos.Set (j, 0, i);
						pos += floorBottomLeft;
						if (!Physics.CheckSphere (pos, 0.125f, checkLayer, QueryTriggerInteraction.Ignore)) {
							Vertex vert = new Vertex ();
							vert.position.Set (pos.x, floorTop, pos.z);
							vert.visited = false;
							vert.occupied = false;
							vert.index = count;
							vertices.Add (vert);
							count++;
						} else {
							vertices.Add (null);
							count++;
						}
					}
				}
			}
			SetAdjacent ();
			ready = true;
			graphCount = vertices.Count;
		}
	}

	void SetAdjacent() {
		int numVertices = vertices.Count;
		for (int i = 0; i < numVertices; i++) {
			if (vertices[i] != null && vertices[i].adjacentVertices.Count == 0) {
				if ((i - 1) >= 0 && ((i - 1) % gridWidth) < (i % gridWidth) && vertices[i - 1] != null) {
					vertices[i].adjacentVertices.Add (i - 1);
				}
				if ((i + 1) < numVertices && ((i + 1) % gridWidth) > (i % gridWidth) && vertices[i + 1] != null) {
					vertices[i].adjacentVertices.Add (i + 1);
				}
				if ((i - gridWidth) >= 0 && vertices[i - gridWidth] != null) {
					vertices[i].adjacentVertices.Add (i - gridWidth);
				}
				if ((i + gridWidth) < numVertices && vertices[i + gridWidth] != null) {
					vertices[i].adjacentVertices.Add (i + gridWidth);
				}
			}
		}
	}

	public List<int> FindShortestPath(int begin, int end) {
        if (begin >= 0 && begin < vertices.Count && end >= 0 && end < vertices.Count) {
            Vertex source = vertices[begin];
            Vertex dest = vertices[end];
            Queue<List<int>> queue = new Queue<List<int>>();
            foreach (Vertex v in vertices) {
                if (v != null) {
                    v.visited = false;
                }
            }
            source.visited = true;
            List<int> path = new List<int>();
            path.Add(source.index);
            queue.Enqueue(path);
            while (queue.Count != 0) {
                List<int> currPath = queue.Dequeue();
                Vertex currVertex = vertices[currPath[currPath.Count - 1]];
                if (currVertex.position == dest.position) {
                    return currPath;
                } else {
                    foreach (int index in currVertex.adjacentVertices) {
                        if (vertices[index] != null) {
                            if (vertices[index].visited == false && (vertices[index].occupiedBy == "Player" || vertices[index].occupied == false)) {
                                List<int> newPath = new List<int>(currPath);
                                vertices[index].visited = true;
                                newPath.Add(index);
                                queue.Enqueue(newPath);
                            }
                        }
                    }
                }
            }
            return new List<int>();
        } else {
            return new List<int>();
        }
	}

	public int GetIndexFromPosition(Vector3 pos) {
		foreach (Vertex v in vertices) {
			if (v != null) {
				if (Mathf.Approximately(v.position.x, pos.x) && Mathf.Approximately(v.position.z, pos.z)) {
					return v.index;
				}
			}
		}
		return -1;
	}
}
