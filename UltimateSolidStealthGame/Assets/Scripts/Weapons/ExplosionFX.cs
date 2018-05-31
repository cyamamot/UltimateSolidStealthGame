using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFX : MonoBehaviour {

    [SerializeField]
    float lifetime;
    [SerializeField]
    GameObject scorchDecalObject;

    ParticleSystem scorchDecal;
    ParticleSystem parentSystem;

	void Awake () {
        scorchDecal = scorchDecalObject.GetComponent<ParticleSystem>();
        parentSystem = GetComponent<ParticleSystem>();
	}

    public void ExplodeWithDecal(bool useDecal) {
        if (useDecal) {
            scorchDecalObject.SetActive(true);
        } else {
            scorchDecalObject.SetActive(false);
        }
        parentSystem.Play(true);
        StartCoroutine("WaitThenDestroy");
    }

    IEnumerator WaitThenDestroy() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
