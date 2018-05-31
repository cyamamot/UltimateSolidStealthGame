using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour {

    Vertex vertex;
	LaserPointer pointer;

    public Vertex Vertex {
        get { return vertex; }
        set { vertex = value; }
    }
	public LaserPointer Pointer {
		set { pointer = value; }
	}
		
	void Start () {
	}

	void OnDestroy() {
		if (pointer) {
			pointer.LaserInvestigated ();
		}
	}
}
