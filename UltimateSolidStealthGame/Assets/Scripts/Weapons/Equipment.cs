using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour {

	[SerializeField]
	string equipmentType;
	[SerializeField]
	protected int count;
	[SerializeField]
	Sprite icon;

	protected MeshRenderer render;

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
		render = GetComponent<MeshRenderer> ();
	}

	public abstract void UseEquipment ();

	public void EquipmentRender(bool enable) {
		if (render) {
			render.enabled = enable;
		}
	}
}
