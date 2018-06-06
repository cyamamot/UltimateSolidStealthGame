using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastZone : MonoBehaviour {

    BoxCollider box;
    Vector3 overlapExtents;
    ParticleSystem blast;

    void Start () {
        box = GetComponent<BoxCollider>();
        overlapExtents = box.size / 2.0f;
        blast = GetComponentInChildren<ParticleSystem>();
    }
	
	public void Blast() {
        blast.Play();
        Collider[] hits = Physics.OverlapBox(transform.position, overlapExtents);
        Debug.Log(hits.Length);
        foreach (Collider hit in hits) {
            HealthManager health = hit.GetComponent<HealthManager>();
            if (health) {
                health.Attack(10.0f, "Blast");
            }
        }
    }
}
