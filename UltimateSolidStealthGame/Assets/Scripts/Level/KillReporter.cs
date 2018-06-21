using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillReporter : MonoBehaviour {

    [SerializeField]
    KillSubMission manager;

    void Start () {
        enabled = false;
    }

    public void ReportToManager() {
        manager.ReceiveReport();
    }
}
