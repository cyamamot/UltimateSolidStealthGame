using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggestBossWeaponSystem : EnemyWeaponSystem {

    [SerializeField]
    float weaponAngle;

	protected void Start () {
        base.Start();
	}

    protected override void Update() {
        FireWeapon();
    }

    public override void FireWeapon() {
        if (!firing && manager.Renderer.isVisible) {
            if (manager.Sight && manager.Player && manager.Movement && gun) {
                if (manager.Sight.Alerted) {
                    if (Vector3.Angle(transform.forward.normalized, Vector3.right) % 90.0f == 0.0f) {
                        Vector3 toPlayer = manager.Player.transform.position - transform.position;
                        if (Mathf.Abs(Vector3.SignedAngle(transform.forward, toPlayer, Vector3.up)) <= weaponAngle) {
                            RaycastHit hit;
                            if (Physics.Raycast(transform.position, toPlayer, out hit, Mathf.Infinity, manager.Sight.SightLayer)) {
                                if (range == 0 || hit.distance <= range) {
                                    if (hit.transform.CompareTag("Player") == true) {
                                        firing = true;
                                        gun.UseEquipment();
                                        StartCoroutine("FirePause");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
