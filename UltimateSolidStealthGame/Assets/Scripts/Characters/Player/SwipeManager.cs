using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
	Class to detect swipes and move player accordingly
*/
public class SwipeManager : MonoBehaviour {

	/*
		direction player will move in
	*/
	Vector2 moveDir;
	/*
		location of touch start on screen
	*/
	Vector2 touchStart;
	/*
		location of touch end on screen
	*/
	Vector2 touchEnd;
    PlayerManager manager;

    int swipeType;

	void Start () {
		moveDir = Vector2.zero;
        manager = GetComponent<PlayerManager>();
	}

	/*
		Called every frame
		detects touches and swipes and moves player based on swipe direction
	*/
	void Update () {
        if (Time.timeScale > 0.0f) {
            if (Input.touchCount > 0) {
                int id = Input.GetTouch(0).fingerId;
                Touch touch = Input.GetTouch(0);
                if (!EventSystem.current.IsPointerOverGameObject(id)) {
                    switch (touch.phase) {
                        case TouchPhase.Began:
                            manager.Movement.StopMoving();
                            touchStart = touch.position;
                            moveDir = Vector2.zero;
                            break;
                        case TouchPhase.Moved:
                            moveDir = touch.position - touchStart;
                            touchStart = touch.position;
                            MoveInDirection(moveDir);
                            break;
                        case TouchPhase.Ended:
                            if (moveDir.magnitude < 5.0f) {
                                manager.Interact.TryInteract();
                            }
                            break;
                    }
                }
            }
        }
	}

	/*
		moves player in direction of swipe based on whether x or y component  of dir is larger
		@param dir - direction of swipe
	*/
	void MoveInDirection(Vector2 dir) {
		if (moveDir.magnitude > 5.0f && manager.Movement) {
			float xDir = Mathf.Abs (dir.x);
			float yDir = Mathf.Abs (dir.y);
			if (xDir >= yDir) {
				if (moveDir.x > 0) {
                    manager.Movement.MoveUntilStop (Enums.directions.right);
				} else if (moveDir.x < 0) {
                    manager.Movement.MoveUntilStop (Enums.directions.left);
				}
			} else if (xDir < yDir) {
				if (moveDir.y > 0) {
                    manager.Movement.MoveUntilStop (Enums.directions.up);
				} else if (moveDir.y < 0) {
                    manager.Movement.MoveUntilStop (Enums.directions.down);
				}
			}
		}
	}
}
