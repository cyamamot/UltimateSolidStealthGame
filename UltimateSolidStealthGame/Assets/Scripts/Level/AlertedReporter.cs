using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedReporter : MonoBehaviour {

    [SerializeField]
    AlertedSubMission manager;

    void Start () {
        enabled = false;
	}

    public void ReportToManager() {
        manager.ReceiveReport();
    }
}
