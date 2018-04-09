using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour{

	public List<Vertex> vertices;
	public int width;
	public int height;
	public bool ready = false;
	public float vertexDistance = 1.0f;

	// Use this for initialization
	void Awake () {
		vertices = new List<Vertex> ();
		if (vertices.Count == 0) { 
			int count = 0;
			Vector3 pos = new Vector3 ();
			for (float i = vertexDistance; i < (float)height; i += vertexDistance) {                         
				for (float j = vertexDistance; j < (float)width; j += vertexDistance) {
					pos.Set (j, 0.5f, i);
					if (!Physics.CheckSphere (pos, 0.125f)) {
						Vertex vert = new Vertex ();
						vert.position.Set (pos.x, 0.0f, pos.z);
						vert.index = count;
						vertices.Add (vert);
					} else {
						vertices.Add (null);
					}
					count++;
				}
			}
		}
		SetAdjacent ();
		ready = true;
	}

	void SetAdjacent() {
		int numVertices = vertices.Count;
		int gridWidth = width * (int) (1.0f / vertexDistance) - 1;
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

	public Queue<Vertex> FindShortestPath(int begin, int end) {
		Vertex source = vertices[begin];
		Vertex dest = vertices[end];
		Queue<List<Vertex>> queue = new Queue<List<Vertex>> ();
		foreach (Vertex v in vertices) {
			if (v != null) {
				v.visited = false;
			}
		}
		source.visited = true;
		List<Vertex> path = new List<Vertex> ();
		path.Add (source);
		queue.Enqueue (path);
		while (queue.Count != 0) {
			List<Vertex> currPath = queue.Dequeue ();
			Vertex currVertex = currPath[currPath.Count - 1];
			if (currVertex.position == dest.position) {
				return new Queue<Vertex> (currPath);
			} else {
				foreach (int index in currVertex.adjacentVertices) {
					if (vertices[index].visited == false) {
						List<Vertex> newPath = new List<Vertex> (currPath);
						vertices[index].visited = true;
						newPath.Add (vertices[index]);
						queue.Enqueue (newPath);
					}
				}
			}
		}
		return null;
	}

	public int GetIndexFromPosition(Vector3 pos) {
		foreach (Vertex v in vertices) {
			if (v != null) {
				if (v.position.x == pos.x && v.position.z == pos.z) {
					return v.index;
				}
			}
		}
		return -1;
	}
}
