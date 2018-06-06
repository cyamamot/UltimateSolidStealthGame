using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketThrustZone : MonoBehaviour {

    BoxCollider box;
    Vector3 overlapExtents;

	void Start () {
        box = GetComponent<BoxCollider>();
        overlapExtents = box.size / 2.0f;
	}

    public void ThrustZoneDamage(float thrustTime) {
        StartCoroutine(Thrust(thrustTime));
    }

    IEnumerator Thrust(float time) {
        float startTime = Time.time;
        float currTime = Time.time;
        while (currTime - startTime < time) {
            Collider[] hits = Physics.OverlapBox(transform.position, overlapExtents);
            foreach (Collider hit in hits) {
                HealthManager health = hit.GetComponent<HealthManager>();
                if (health) {
                    health.Attack(1.0f, "Thrust");
                }
            }
            currTime = Time.time;
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }
}
