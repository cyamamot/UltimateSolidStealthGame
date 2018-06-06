using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour {

    ParticleSystem thrusters;
    float thrustTime;

    public float ThrustTime {
        set { thrustTime = value; }
    }

	void Start () {
        thrusters = GetComponentInChildren<ParticleSystem>();
	}

	public void FireMissile() {
        StartCoroutine("Thrusters");
    }

    IEnumerator Thrusters() {
        thrusters.Play();
        yield return new WaitForSeconds(thrustTime);
        thrusters.Stop();
    }
}
