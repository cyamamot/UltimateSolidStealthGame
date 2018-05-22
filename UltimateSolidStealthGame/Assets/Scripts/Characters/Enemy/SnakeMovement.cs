using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : EnemyMovement {

    [SerializeField]
    GameObject nextSegment;
    [SerializeField]
    GameObject headObject;

    SnakeBody nextSegmentBody;
    SnakeHead head;

	// Use this for initialization
	void Start () {
        base.Start();
        if (nextSegment) {
            nextSegmentBody = nextSegment.GetComponent<SnakeBody>();
        }
        if (headObject) {
            head = headObject.GetComponent<SnakeHead>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    protected override void TravelBetweenPathPoints() {
        if (path.Count > 0) {
            if (nav.remainingDistance <= nav.stoppingDistance) {
                if (lastVertexIndex != currVertexIndex) {
                    if (!nextSegmentBody) {
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
                    head.Turn(moveDir);
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

                    nav.SetDestination(manager.Graph.vertices[path[0]].position);
                    manager.Graph.vertices[path[0]].occupied = true;
                    manager.Graph.vertices[path[0]].occupiedBy = enemyName;
                    manager.Graph.vertices[path[0]].NotifyParentOrChild();
                    if (nextSegmentBody) {
                        nextSegmentBody.SetDestination(lastVertexIndex);
                    }
                }
            }
        }
    }
}
