using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour {

    Vertex vertex;
	LaserPointer pointer;

    public Vertex Vertex {
        set { vertex = value; }
    }
	public int Location {
		get {
            if (vertex.parentVertex != null) {
                return vertex.parentVertex.index;
            } else {
                return vertex.index;
            }
        }
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
