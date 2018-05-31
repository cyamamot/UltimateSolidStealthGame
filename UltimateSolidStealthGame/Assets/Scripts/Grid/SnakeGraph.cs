using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGraph : Graph {

    [SerializeField]
    GameObject childGraphObject;
    [SerializeField]
    int bottomLeftMapping = -1;

    Graph childGraph;

    public override void Awake() {
        base.Awake();
        childGraph = childGraphObject.GetComponent<Graph>();
        SetChildrenAndParent();
    }

    void SetChildrenAndParent() {
        int ratio = (int)(vertexDistance / childGraph.VertexDistance);
        int currParentIndex = 0;
        int bottomLeftTemp = GetBottomLeftMapping();
        bottomLeftMapping = (bottomLeftTemp != -1) ? bottomLeftTemp : bottomLeftMapping;
        if (bottomLeftMapping != -1) {
            for (int i = 0; i < gridHeight; i++) {
                for (int j = 0; j < gridWidth; j++) {
                    // for each parentGraph vertex, link parentVertex to all childVertices it overlaps and vice versa
                    for (int k = 0; k < ratio; k++) {
                        for (int l = 0; l < ratio; l++) {
                            if (vertices[currParentIndex] != null) {
                                int currChildIndex = bottomLeftMapping + (j * ratio) + (i * ratio * childGraph.GridWidth) + l + (k * childGraph.GridWidth);
                                Vertex temp = childGraph.vertices[currChildIndex];
                                vertices[currParentIndex].childVertices.Add(temp);
                                if (temp != null) {
                                    temp.parentVertex = vertices[currParentIndex];
                                } else {
                                    vertices[currParentIndex] = null;
                                }
                            }
                        }
                    }
                    currParentIndex++;
                }
            }
        }
    }

    int GetBottomLeftMapping() {
        Vector3 bottomLeft = box.center;
        bottomLeft[0] -= width / 2.0f;
        bottomLeft[0] += childGraph.VertexDistance / 2.0f;
        bottomLeft[2] -= height / 2.0f;
        bottomLeft[2] += childGraph.VertexDistance / 2.0f;
        bottomLeft = transform.TransformPoint(bottomLeft);
        return childGraph.GetIndexFromPosition(bottomLeft);
    }

    void Update() {

    }
}
