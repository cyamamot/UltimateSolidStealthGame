using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Class of CigarettePack dropped by Cigarette 
	The actual distraction object
*/
public class CigarettePack : MonoBehaviour {

    /*
		vertex this pack is at
	*/
    Vertex vertex;

    public Vertex Vertex {
        get { return vertex; }
        set { vertex = value; }
    }

	void Start () {
		//transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
		transform.Rotate (new Vector3(0, Random.Range(0, 360), 0));
		//graph = GameObject.FindGameObjectWithTag ("Graph").GetComponent<Graph> ();
		//location = graph.GetIndexFromPosition (transform.position);
	}
}
