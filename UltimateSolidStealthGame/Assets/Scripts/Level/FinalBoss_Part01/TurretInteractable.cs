using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteractable : Interactable {

    MeshRenderer render;
    TurretControl control;
    bool isOn;

    public bool IsOn {
        get { return isOn; }
        set {
            isOn = value;
            if (isOn) {
                render.material.color = Color.white;
            } else {
                render.material.color = Color.black;
            }
        }
    }

	void Start () {
        render = GetComponent<MeshRenderer>();
        control = GetComponentInParent<TurretControl>();
        render.material.color = Color.black;
    }
	
	public override void Interact() {
        if (isOn) {
            Turret turret = GetComponentInParent<Turret>();
            turret.IsOn = false;
            IsOn = false;
            control.ControlPressed();
        }
    }
}
