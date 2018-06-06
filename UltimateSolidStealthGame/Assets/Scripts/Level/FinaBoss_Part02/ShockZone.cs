using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockZone : MonoBehaviour {

    int numConditionsMet;
    BoxCollider box;
    Vector3 overlapExtents;
    ParticleSystem particles;

    // Use this for initialization
    void Start () {
        box = GetComponent<BoxCollider>();
        overlapExtents = box.size / 2.0f;
        particles = GetComponent<ParticleSystem>();
    }
	
	public void AddCondition() {
        numConditionsMet++;
        if (numConditionsMet == 2) {
            StartCoroutine("Shock");
        }
    }

    public void RemoveCondition() {
        numConditionsMet--;
        StopAllCoroutines();
        particles.Stop();
        if (numConditionsMet < 0) numConditionsMet = 0;
    }

    IEnumerator Shock() {
        yield return new WaitForSeconds(1.0f);
        particles.Play();
        while (numConditionsMet == 2) {
            Collider[] hits = Physics.OverlapBox(transform.position, overlapExtents);
            foreach (Collider hit in hits) {
                HealthManager health = hit.GetComponent<HealthManager>();
                if (health) {
                    health.Attack(0.5f, "Shock");
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        particles.Stop();
        yield break;
    }
}
