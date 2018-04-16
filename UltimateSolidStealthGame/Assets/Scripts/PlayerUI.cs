using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	Text handgunBullets;
	[SerializeField]
	Text uziBullets;
	[SerializeField]
	Text rifleBullets;
	[SerializeField]
	Button handgunButton;
	[SerializeField]
	Button uziButton;
	[SerializeField]
	Button rifleButton;
	[SerializeField]
	Color available;
	[SerializeField]
	Color unavailable;
	[SerializeField]
	Color currentlyPressed;
	[SerializeField]
	Image bulletImage;
	[SerializeField]
	Image knifeImage;

	PlayerWeaponSystem playerWeaponSystem;
	HealthManager playerHealth;
	string currGunType;
	Gun handgun;
	Gun uzi;
	Gun rifle;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp) {
			playerWeaponSystem = temp.GetComponent<PlayerWeaponSystem> ();
			playerHealth = temp.GetComponent<HealthManager> ();
			if (playerWeaponSystem) {
				currGunType = playerWeaponSystem.CurrGunType;
			}
		}
		handgunButton.image.color = available;
		uziButton.image.color = available;
		rifleButton.image.color = available;

		bulletImage.gameObject.SetActive (false);
		knifeImage.gameObject.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (handgun && uzi && rifle) {
			handgunBullets.text = handgun.BulletsLeft.ToString();
			if (handgun.BulletsLeft > 0) {
				handgunButton.image.color = (handgunButton.image.color == currentlyPressed) ? currentlyPressed : available;
			} else {
				handgunButton.image.color = unavailable;
			}
			uziBullets.text = uzi.BulletsLeft.ToString ();
			if (uzi.BulletsLeft > 0) {
				uziButton.image.color = (uziButton.image.color == currentlyPressed) ? currentlyPressed : available;
			} else {
				uziButton.image.color = unavailable;
			}
			rifleBullets.text = rifle.BulletsLeft.ToString ();
			if (rifle.BulletsLeft > 0) {
				rifleButton.image.color = (rifleButton.image.color == currentlyPressed) ? currentlyPressed : available;
			} else {
				rifleButton.image.color = unavailable;
			}
		}
		if (playerWeaponSystem.CurrGunType == "") {
			knifeImage.gameObject.SetActive (true);
			bulletImage.gameObject.SetActive (false);
		} else {
			knifeImage.gameObject.SetActive (false);
			bulletImage.gameObject.SetActive (true);
		}
	}

	public void SetGunsForUI(ref Gun h, ref Gun u, ref Gun r) {
		handgun = h;
		uzi = u;
		rifle = r;
	}

	public void ClickGunButton(string gunType) {
		if (gunType != currGunType) {
			currGunType = gunType;
			switch (gunType) {
			case "Handgun":
				if (handgun.BulletsLeft > 0) {
					handgunButton.image.color = currentlyPressed;
					uziButton.image.color = available;
					rifleButton.image.color = available;
				}
				break;
			case "Uzi":
				if (uzi.BulletsLeft > 0) {
					uziButton.image.color = currentlyPressed;
					handgunButton.image.color = available;
					rifleButton.image.color = available;
				}
				break;
			case "AssaultRifle":
				if (rifle.BulletsLeft > 0) {
					rifleButton.image.color = currentlyPressed;
					handgunButton.image.color = available;
					uziButton.image.color = available;
				}
				break;
			}
			playerWeaponSystem.SwapWeapon (gunType);
		} else {
			currGunType = "";
			handgunButton.image.color = available;
			uziButton.image.color = available;
			rifleButton.image.color = available;
			playerWeaponSystem.SwapToKnife ();
		}
	}
}
