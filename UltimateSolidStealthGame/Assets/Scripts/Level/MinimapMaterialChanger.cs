using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapMaterialChanger : MonoBehaviour {

    [SerializeField]
    Material materialToChange;
    [SerializeField]
    Color newColor;

    Color defaultColor;

	void Awake () {
        defaultColor = materialToChange.color;
	}

    void OnPreRender() {
        materialToChange.color = newColor;
    }

    void OnPostRender() {
        materialToChange.color = defaultColor;
    }
}
