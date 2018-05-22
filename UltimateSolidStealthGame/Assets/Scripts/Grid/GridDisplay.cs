#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDisplay : MonoBehaviour {

    [SerializeField]
    Color lineColor;

    Graph graph;

	void Start () {
        graph = GetComponent<Graph>();
	}
	
	void Update () {
		for (float i = 0.0f; i <= graph.Width; i += graph.VertexDistance) {
            Vector3 bottom = graph.FloorBottomLeft + new Vector3(i, 0, 0);
            Vector3 top = graph.FloorBottomLeft + new Vector3(i, 0, graph.Height);
            Debug.DrawLine(bottom, top, lineColor, 0.0f);
        }
        for (float j = 0.0f; j <= graph.Height; j += graph.VertexDistance) {
            Vector3 left = graph.FloorBottomLeft + new Vector3(0, 0.1f, j);
            Vector3 right = graph.FloorBottomLeft + new Vector3(graph.Width, 0.1f, j);
            Debug.DrawLine(left, right, lineColor, 0.0f);
        }
    }
}
#endif
