using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectWheel : MonoBehaviour {

	[SerializeField]
	float wheelRadius = 1.0f;

	List<GameObject> buttonList;
	int lastListSize;
	float midX;
	float midY;

	// Use this for initialization
	void Awake () {
		buttonList = new List<GameObject> ();
		midX = Screen.width / 2.0f;
		midY = Screen.height / 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddButton(GameObject newButton) {
			buttonList.Add (newButton);
	}

	public void DisplayWheel() {
		if (lastListSize != buttonList.Count) {
			lastListSize = buttonList.Count;
			float angle = (360.0f / buttonList.Count) * Mathf.Deg2Rad;
			for (int i = 0; i < lastListSize; i++) {
				buttonList [i].SetActive (true);
				float x = Mathf.Sin (angle * i) * wheelRadius;
				float y = Mathf.Cos (angle * i) * wheelRadius;
				Vector3 pos = new Vector3 (x, y, 0.0f) + transform.position;
				RectTransform rect = buttonList [i].GetComponent<RectTransform> ();
				rect.position = pos;
			}
		}
		foreach (GameObject g in buttonList) {
			g.SetActive (true);
		}
	}

	public void HideWheel() {
		foreach (GameObject g in buttonList) {
			g.SetActive (false);
		}
	}
}
