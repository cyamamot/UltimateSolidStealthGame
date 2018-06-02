using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockInvolvedInteractables : Interactable {

    [SerializeField]
    GameObject shockZoneObject;
    [SerializeField]
    float activeTime;

    ShockZone zone;
    ParticleSystem particles;
    bool active;

	// Use this for initialization
	void Start () {
        zone = shockZoneObject.GetComponent<ShockZone>();
        particles = GetComponent<ParticleSystem>();
	}

    public override void Interact() {
        if (!active) {
            active = true;
            StartCoroutine("Activate");
        }
    }

    IEnumerator Activate() {
        particles.Play();
        zone.AddCondition();
        yield return new WaitForSeconds(activeTime);
        particles.Stop();
        zone.RemoveCondition();
        active = false;
    }
}
