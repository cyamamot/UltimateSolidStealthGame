using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour {

    [SerializeField]
    float reactivateTime;

    int numActiveControls;
    int currActiveControls;
    Turret[] turrets;
    TurretInteractable[] turretPanels;

	void Start () {
        numActiveControls = 2;
        currActiveControls = numActiveControls;
        turrets = GetComponentsInChildren<Turret>();
        turretPanels = GetComponentsInChildren<TurretInteractable>();
        StartCoroutine(WaitToReactivate(10.0f));
	}
	
	public void ControlPressed() {
        currActiveControls--;
        if (currActiveControls <= 0) {
            foreach (Turret turret in turrets) {
                turret.IsOn = false;
            }
            StartCoroutine(WaitToReactivate(reactivateTime));
        }
    }

    IEnumerator WaitToReactivate(float time) {
        yield return new WaitForSeconds(time);
        currActiveControls = numActiveControls;
        foreach (Turret turret in turrets) {
            turret.IsOn = true;
        }
        int index1 = Random.Range(0, 4);
        int index2 = Enums.RandomIntExcept(0, 4, index1);
        turretPanels[index1].IsOn = true;
        turretPanels[index2].IsOn = true;
    }

}
