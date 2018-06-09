using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	Class to handle displaying and formatting WeaponSelection button wheel
*/
public class WeaponSelectWheel : MonoBehaviour {

	/*
		radius of button wheel
	*/
	float wheelRadius;

	/*
		reference to background circle image
	*/
	//Image bgCircle;
	/*
		list of references to Button gameobjects
	*/
	List<GameObject> buttonList;

    List<EquipmentButton> equipmentButtonList;
	/*
		number of buttons in list when last displayed
	*/
	int lastListSize;
	/*
		coordinates of middle of screen
	*/
	float midX;
	float midY;

	void Awake () {
		//bgCircle = GetComponent<Image> ();
		//bgCircle.gameObject.SetActive (false);
		if (buttonList == null) buttonList = new List<GameObject> ();
        if (equipmentButtonList == null) equipmentButtonList = new List<EquipmentButton>();
        //wheelRadius = Screen.height / 4.75f;
        wheelRadius = (transform.parent.parent.localScale.x * GetComponent<RectTransform>().rect.width) / 2.0f;
        enabled = false;
	}

	/*
		add button reference to list
		@param newButton - reference to Button gameobject
	*/
	public void AddButton(GameObject newButton) {
        if (buttonList == null) buttonList = new List<GameObject>();
        if (equipmentButtonList == null) equipmentButtonList = new List<EquipmentButton>();
        buttonList.Add (newButton);
        equipmentButtonList.Add(newButton.GetComponent<EquipmentButton>());
		newButton.transform.SetParent(transform, false);
		RectTransform rect = newButton.GetComponent<RectTransform> ();
		rect.localPosition = Vector3.one;
		rect.localScale = Vector3.one;
		newButton.SetActive (false);
	}

	/*
		displays wheel and sets button positions based on number of buttons in list
	*/
	public void DisplayWheel() {
		if (lastListSize != buttonList.Count) {
			lastListSize = buttonList.Count;
			float angle = (360.0f / buttonList.Count);
            float angleRad = angle * Mathf.Deg2Rad;
            for (int i = 0; i < lastListSize; i++) {
				buttonList [i].SetActive (true);
				float x = Mathf.Sin (angleRad * i) * wheelRadius;
				float y = Mathf.Cos (angleRad * i) * wheelRadius;
				Vector3 pos = new Vector3 (x, y, 0.0f) + transform.position;
				RectTransform rect = buttonList[i].GetComponent<RectTransform> ();
				rect.position = pos;
                //pos.Set(0.0f, 0.0f, -angle * i);
                //rect.Rotate(pos);
			}
		}
        for (int i = 0; i < buttonList.Count; i++) {
            equipmentButtonList[i].Pop();
		}
		//bgCircle.gameObject.SetActive (true);
	}

	/*
		hides wheel
	*/
	public void HideWheel() {
        for (int i = 0; i < buttonList.Count; i++) {
            equipmentButtonList[i].Close();
        }
        //bgCircle.gameObject.SetActive (false);
    }
}
