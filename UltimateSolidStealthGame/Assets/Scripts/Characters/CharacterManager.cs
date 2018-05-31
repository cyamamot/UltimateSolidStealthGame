﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Base class of Manager classes
*/
public class CharacterManager : MonoBehaviour {

    /*
		reference to components necessary in all manager subclasses
	*/
    [SerializeField]
    GameObject graphObject;

	protected HealthManager health;
	protected Graph graph;
    protected bool alive;

	public HealthManager Health {
		get { return health; }
	}
	public Graph Graph {
		get { return graph; }
	}
    public bool Alive {
        get { return alive; }
    }
		
	protected virtual void Awake () {
		health = GetComponent<HealthManager> ();
		if (graphObject) {
			graph = graphObject.GetComponent<Graph> ();
		}
	}
	
	public virtual void Kill() {}

    public virtual void OnTakeDamage() {}
}
