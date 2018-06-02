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

    public void ThrustZoneDamage() {
        Collider[] hits = Physics.OverlapBox(transform.position, overlapExtents);
        Debug.Log(hits.Length);
        foreach (Collider hit in hits) {
            HealthManager health = hit.GetComponent<HealthManager>();
            if (health) {
                health.Attack(7.0f, "Thrust");
            }
        }
    }
}
