using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeWeaponSystem : EnemyWeaponSystem {

    [SerializeField]
    GameObject projectileSpawnLoc;
    [SerializeField]
    GameObject projectileObject;
    [SerializeField]
    float baseDamage;
    [SerializeField]
    float projectileSpeed;

    protected override void Start() {
        manager = (transform.parent != null) ? transform.parent.GetComponentInChildren<EnemyManager>() : GetComponent<EnemyManager>();
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    protected override void Update() {
        base.Update();
    }

    public override void FireWeapon() {
        if (!firing && renderer.isVisible && manager.Sight.Alerted) {
            Vector3 spawnLoc = projectileSpawnLoc.transform.position;
            Vector3 toPlayer = (manager.Player.transform.position - spawnLoc).normalized;
            Vector3 ignoreYToPlayer = new Vector3(toPlayer[0], 0.0f, toPlayer[2]);
            if (Vector3.Angle(ignoreYToPlayer.normalized, transform.forward) <= 45.0f) {
                float angle = Vector3.SignedAngle(transform.up, toPlayer, transform.right);
                Vector3 axis = Vector3.Cross(transform.up, toPlayer);
                GameObject firedBullet = (GameObject)Instantiate(projectileObject, spawnLoc, Quaternion.AngleAxis(angle, axis));
                Rigidbody rb = firedBullet.GetComponent<Rigidbody>();
                Missile m = firedBullet.GetComponent<Missile>();
                if (rb && m) {
                    firing = true;
                    m.Damage = baseDamage;
                    firedBullet.layer = gameObject.layer;
                    rb.velocity = toPlayer * projectileSpeed;
                }
                StartCoroutine("FirePause");
            }
        }
    }
}
