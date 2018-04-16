using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSystem : MonoBehaviour {

	[SerializeField]
	GameObject handgunPrefab;
	[SerializeField]
	GameObject uziPrefab;
	[SerializeField]
	GameObject riflePrefab;

	GameObject handgun;
	GameObject uzi;
	GameObject rifle;
	PlayerUI ui;
	string currGunType;
	GameObject currGun;
	Gun gun;

	public string CurrGunType {
		get { return currGunType; }
	}
	public GameObject CurrGun {
		get { return currGun; }
	}

	// Use this for initialization
	void Start () {
		handgun = GameObject.Instantiate (handgunPrefab, transform);
		handgun.SetActive (false);
		uzi = GameObject.Instantiate (uziPrefab, transform);
		uzi.SetActive (false);
		rifle = GameObject.Instantiate (riflePrefab, transform);
		rifle.SetActive (false);
		GameObject temp = GameObject.FindGameObjectWithTag ("UI");
		if (temp != null) {
			ui = temp.GetComponent<PlayerUI> ();
			if (ui != null) {
				Gun h = handgun.GetComponent<Gun> ();
				Gun u = uzi.GetComponent<Gun> ();
				Gun r = rifle.GetComponent<Gun> ();
				ui.SetGunsForUI (ref h, ref u, ref r);
			}
		}
		SwapToKnife ();
	}

	void Update() {
		if (gun && gun.BulletsLeft == 0) {
			SwapToKnife ();
		}
	}

	void OnCollisionEnter(Collision collision) {
		//use for gun pickup
	}
	
	public void UseWeapon() {
		if (gun != null) {
			gun.Fire ();
		} else {
			//USE KNIFE (MESH-ONLY GAMEOBJECT)
		}
	}

	public void SwapWeapon(string gunType) {
		if (currGun != null) {
			currGun.gameObject.SetActive (false);
		}
		switch (gunType) {
		case "Handgun":
			handgun.SetActive (true);
			currGun = handgun;
			break;
		case "Uzi":
			uzi.SetActive (true);
			currGun = uzi;
			break;
		case "AssaultRifle":
			rifle.SetActive (true);
			currGun = rifle;
			break;
		}
		gun = currGun.GetComponent<Gun> ();
		currGunType = gun.GunType;
	}

	public void SwapToKnife() {
		if (currGun != null) {
			currGun.gameObject.SetActive (false);
		}
		gun = null;
		currGunType = "";
		currGun = null;
	}

	void KnifeAttack() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity)) {

		}
	}
}
