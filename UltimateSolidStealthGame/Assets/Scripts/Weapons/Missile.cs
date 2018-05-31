using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet {

    [SerializeField]
    GameObject explosionFXObject;
    [SerializeField]
    float explosionRadius;

    int playerLayer;
    int nonMissileLayers;

	// Use this for initialization
	void Start () {
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        nonMissileLayers = 1 << gameObject.layer;
        nonMissileLayers = ~nonMissileLayers;
	}

    void OnCollisionEnter(Collision collision) {
        Vector3 loc = collision.contacts[0].point;
        string hitTag = collision.gameObject.tag;
        if (hitTag == "Untagged") {
            Destroy(gameObject);
            GameObject obj = Instantiate(explosionFXObject, loc, Quaternion.identity);
            ExplosionFX fx = obj.GetComponent<ExplosionFX>();
            fx.ExplodeWithDecal(false);
            Explode(loc);
        } else if (hitTag == "Floor") {
            Destroy(gameObject);
            GameObject obj = Instantiate(explosionFXObject, loc, Quaternion.identity);
            ExplosionFX fx = obj.GetComponent<ExplosionFX>();
            fx.ExplodeWithDecal(true);
            Explode(loc);
        }
    }

    void Explode(Vector3 pos) {
        Collider[] hits = Physics.OverlapSphere(pos, explosionRadius, playerLayer);
        foreach (Collider hit in hits) {
            PlayerManager pm = hit.gameObject.GetComponent<PlayerManager>();
            if (pm) {
                RaycastHit rayHit;
                if (Physics.Raycast(pos, pm.transform.position - pos, out rayHit, explosionRadius, nonMissileLayers)) {
                    if (rayHit.transform.tag == "Player") {
                        float rawDamageRatio =  1.0f - rayHit.distance / explosionRadius;
                        if (rawDamageRatio <= 0.9f) {
                            float roundedDamageRatio = ((int)(rawDamageRatio / 0.25f)) * 0.25f;
                            pm.Health.Attack(damage - (roundedDamageRatio * damage));
                        } else {
                            pm.Health.Attack(damage);
                        }
                    }
                }
            }
        }
    }
}
