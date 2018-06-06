using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonatorInteractable : Interactable {

    [SerializeField]
    GameObject blastZoneObject;
    [SerializeField]
    float timeToWait;

    BlastZone zone;
    bool detonating;

	void Start () {
        zone = blastZoneObject.GetComponent<BlastZone>();
	}

    public override void Interact() {
        if (!detonating) {
            detonating = true;
            StartCoroutine("Detonate");
        }
    }

    IEnumerator Detonate() {
        zone.Blast();
        yield return new WaitForSeconds(timeToWait);
        detonating = false;
    }
}
