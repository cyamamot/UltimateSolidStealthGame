using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherInteractable : Interactable {

    [SerializeField]
    GameObject rocketPivotObject;
    [SerializeField]
    GameObject thrustZoneObject;
    [SerializeField]
    float thrustOnTime;

    Animator rocketAnimator;
    RocketLauncher launcher;
    RocketThrustZone zone;
    bool launcherOn;

    private void Start() {
        rocketAnimator = rocketPivotObject.GetComponent<Animator>();
        launcher = rocketPivotObject.GetComponent<RocketLauncher>();
        zone = thrustZoneObject.GetComponent<RocketThrustZone>();
        launcher.ThrustTime = thrustOnTime - 2.0f;
    }

    public override void Interact() {
        if (!launcherOn) {
            launcherOn = true;
            if (rocketAnimator) {
                StartCoroutine("MoveLauncher");
            } else {
                launcherOn = false;
            }
        }
    }

    IEnumerator MoveLauncher() {
        rocketAnimator.SetTrigger("MoveToFire");
        yield return new WaitForSeconds(2.0f);
        launcher.FireMissile();
        yield return new WaitForSeconds(0.5f);
        zone.ThrustZoneDamage(thrustOnTime - 2.0f);
        yield return new WaitForSeconds(thrustOnTime - 2.0f);
        rocketAnimator.SetTrigger("MoveToBase");
        yield return new WaitForSeconds(2.0f);
        launcherOn = false;
    }
}
