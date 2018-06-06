using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	base class of all Equipment types
*/
public abstract class Equipment : MonoBehaviour {

	/*
		name of type of equipment
	*/
	[SerializeField]
	protected string equipmentType;
	/*
		initial number of Equipment
	*/
	[SerializeField]
	protected int count;
	/*
		icon used to display in weapon wheel
	*/
	[SerializeField]
	protected Sprite icon;

    [SerializeField]
    protected AudioClip sfx;

	/*
		reference to MeshRenderer component
	*/
	protected MeshRenderer render;

    protected AudioSource sfxGod;

	public string EquipmentType {
		get { return equipmentType; }
	}
	public int Count {
		get { return count; }
		set { count = value; }
	}
	public Sprite Icon {
		get { return icon; }
	}

	public virtual void Awake() {
        enabled = false;
		render = GetComponent<MeshRenderer> ();
        sfxGod = GameObject.FindGameObjectWithTag("SFXGod").GetComponent<AudioSource>();
	}

	public abstract void UseEquipment ();

	/*
		enables or disables rendering of Equipment object
		@param enable - whether to enable or disable rendering
	*/
	public void EquipmentRender(bool enable) {
		if (render) {
			render.enabled = enable;
		}
	}
}
