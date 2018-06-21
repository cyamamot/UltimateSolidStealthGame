using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedSubMission : SubMissionManager {

	void Awake () {
        base.Awake();
        missionCompleted = true;
    }
	
	public override void ReceiveReport() {
        missionCompleted = false;
    }
}
