using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeBody : MonoBehaviour {

    [SerializeField]
    GameObject graphObject;
    [SerializeField]
    GameObject nextSegment;
    [SerializeField]
    Material brokenMaterial;
    [SerializeField]
    Mesh brokenMesh;

    SnakeBody nextSegmentBody;
    Graph graph;
    UnityEngine.AI.NavMeshAgent nav;
    int currIndex;
    int lastIndex;
    string enemyName;

    void Start () {
        if (nextSegment) {
            nextSegmentBody = nextSegment.GetComponent<SnakeBody>();
        }
        graph = graphObject.GetComponent<Graph>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currIndex = graph.GetIndexFromPosition(transform.position);
        lastIndex = currIndex;
        enemyName = gameObject.name;
        graph.vertices[currIndex].occupiedBy = enemyName;
        graph.vertices[currIndex].occupied = true;
        graph.vertices[currIndex].NotifyParentOrChild();
    }
	
	void Update () {
	    if (!nextSegmentBody) {
            if (nav.remainingDistance <= 1.0f) {
                graph.vertices[lastIndex].occupied = false;
                graph.vertices[lastIndex].occupiedBy = "";
                graph.vertices[lastIndex].NotifyParentOrChild();
            }
        }
	}

    public void SetDestination(int destIndex) {
        lastIndex = currIndex;
        currIndex = destIndex;
        nav.SetDestination(graph.vertices[destIndex].position);
        graph.vertices[currIndex].occupied = true;
        graph.vertices[currIndex].occupiedBy = enemyName;
        graph.vertices[currIndex].NotifyParentOrChild();
        if (nextSegmentBody) {
            nextSegmentBody.SetDestination(lastIndex);
        } else {
            if (graph.vertices[lastIndex].occupiedBy == enemyName) {
                StartCoroutine(ClearVertex(lastIndex));
            }
        }
    }

    IEnumerator ClearVertex(int index) {
        yield return new WaitForSeconds(1.0f);
        graph.vertices[index].occupied = false;
        graph.vertices[index].occupiedBy = "";
        graph.vertices[index].NotifyParentOrChild();
    }

    public void KillSegment() {
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter) {
            filter.mesh = brokenMesh;
            Renderer rend = GetComponent<Renderer>();
            if (rend) {
                rend.material = brokenMaterial;
            }
        }
    }
}
