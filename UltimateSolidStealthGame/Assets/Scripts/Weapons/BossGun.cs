using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGun : Gun {

    [SerializeField]
    int shotsPerRound;

    [SerializeField]
    float spread;

	protected virtual void Awake () {
        base.Awake();
	}

    protected override IEnumerator Shoot() {
        for (int i = 0; i < roundsPerFire; i++) {
            if (bulletsLeft > 0 || bulletsLeft == -1) {
                Transform parent = transform.parent.gameObject.transform;
                Vector3 barrelPos = parent.position + parent.forward * barrelDist;
                for (int j = 0; j < shotsPerRound; j++) {
                    GameObject firedBullet = (GameObject)Instantiate(bullet, barrelPos, Quaternion.AngleAxis(90, transform.right));
                    Rigidbody rb = firedBullet.GetComponent<Rigidbody>();
                    Bullet b = firedBullet.GetComponent<Bullet>();
                    if (rb && b) {
                        b.Damage = damage;
                        firedBullet.layer = gameObject.layer;
                        float randomX = Random.Range(-spread, spread);
                        float randomY = Random.Range(-spread - 0.5f, spread + 0.5f);
                        rb.velocity = parent.forward * bulletSpeed + (randomY * transform.up) + (randomX * transform.right);
                        if (bulletsLeft > 0) {
                            bulletsLeft--;
                            count = bulletsLeft;
                        }
                    }
                    Destroy(firedBullet, 1.0f);
                }
                yield return new WaitForSeconds(timeBetweenRounds);
            }
        }
        firing = false;
    }
}
