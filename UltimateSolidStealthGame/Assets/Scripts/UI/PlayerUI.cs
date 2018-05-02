﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	GameObject buttonPrefab;
	[SerializeField]
	Text counter;
	[SerializeField]
	Slider slider;
	[SerializeField]
	GameObject weaponWheelPrefab;

	WeaponSelectWheel weaponWheel;
	List<GameObject> equipmentInstanceList;
	float pressTime;
	PlayerManager manager;
	string currEquipmentType;
	Equipment currEquipment;
	bool wheelDisplayed;
	bool primaryPressed;

	void Awake () {
		equipmentInstanceList = new List<GameObject> ();
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player) {
			manager = player.GetComponent<PlayerManager> ();
			if (manager) {
				GameObject temp = GameObject.Instantiate (weaponWheelPrefab);
				temp.transform.SetParent(gameObject.transform, false);
				RectTransform rect = temp.GetComponent<RectTransform> ();
				rect.position = new Vector3 (Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
				rect.localScale = Vector3.one;
				weaponWheel = temp.GetComponent<WeaponSelectWheel> ();
			}
		}
	}

	void Update () {
		if (currEquipment) {
			if (counter.IsActive()) {
				counter.text = "x" + currEquipment.Count.ToString ();
			} else if (slider.IsActive()) {
				slider.value = currEquipment.Count / 100.0f;
			}
			if (primaryPressed) {
				float time = Time.time - pressTime;
				if (time > 0.5f) {
					primaryPressed = false;
					weaponWheel.DisplayWheel();
					wheelDisplayed = true;
				}
			}
		}
	}

	public void AddEquipment(ref GameObject e) {
		equipmentInstanceList.Add (e);
		Equipment equipment = e.GetComponent<Equipment> ();
		GameObject newEquipment = Instantiate (buttonPrefab);
		Button button = newEquipment.GetComponent<Button> ();
		Image image = newEquipment.GetComponent<Image> ();
		RectTransform rect = newEquipment.GetComponent<RectTransform> ();
		button.onClick.AddListener (delegate{manager.WeaponSystem.SwapEquipment(equipment.EquipmentType);});
		image.sprite = equipment.Icon;
		weaponWheel.AddButton (newEquipment);
		newEquipment.transform.SetParent(weaponWheel.transform, false);
		rect.localPosition = Vector3.one;
		rect.localScale = Vector3.one;
		newEquipment.SetActive (false);
	}

	public void OnPrimaryDown() {
		if (wheelDisplayed) {
			weaponWheel.HideWheel();
			wheelDisplayed = false;
		} else {
			pressTime = Time.time;
			primaryPressed = true;
		}
	}

	public void OnPrimaryUp() {
		if (primaryPressed) {
			manager.WeaponSystem.UseEquipped ();
			primaryPressed = false;
		}
	}

	public void UpdateUIOnGunSwap(Equipment e, string s) {
		currEquipmentType = s;
		currEquipment = e;
		weaponWheel.HideWheel ();
		wheelDisplayed = false;
		if (e.GetComponent<Gun>() || e.GetComponent<Cigarette>()) {
			counter.gameObject.SetActive(true);
			slider.gameObject.SetActive(false);
		} else if (e.GetComponent<IceMachine>()) {
			counter.gameObject.SetActive(false);
			slider.gameObject.SetActive(true);
		} else {
			counter.gameObject.SetActive(false);
			slider.gameObject.SetActive(false);
		}
	}
}
