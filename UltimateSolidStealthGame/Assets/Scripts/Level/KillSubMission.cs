using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSubMission : SubMissionManager {

    [SerializeField]
    int numberOfTargets;

	void Awake () {
        base.Awake();
        if (numberOfTargets > 0) missionCompleted = false;
        else missionCompleted = true;
	}

    public override void ReceiveReport() {
        if (numberOfTargets > 0) {
            numberOfTargets--;
            if (numberOfTargets == 0) {
                missionCompleted = true;
            }
        } else {
            missionCompleted = false;
        }
    }
}
