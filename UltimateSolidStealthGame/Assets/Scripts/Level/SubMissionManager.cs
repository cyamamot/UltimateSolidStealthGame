using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubMissionManager : MonoBehaviour {

    [SerializeField]
    protected bool missionCompleted;

    public bool MissionCompleted {
        get { return missionCompleted; }
    }

	protected void Awake () {
        enabled = false;
	}
	
	public abstract void ReceiveReport();
}
