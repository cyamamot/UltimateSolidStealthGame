using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField]
    GameObject yawObject;
    [SerializeField]
    GameObject pitchObject;
    [SerializeField]
    GameObject target;
    [SerializeField]
    float sightDistance;
    [SerializeField]
    float fireWaitTime;


    Gun gun;
    LineRenderer laser;
    GameObject player;
    Transform pitchTransform;
    Transform yawTransform;
    Transform playerTransform;
    Transform targetTransform;
    Vector3 newTargetDist;
    bool isOn;
    float randRotateDir;
    float distToPlayer;
    bool firing;

    public bool IsOn {
        get { return isOn; }
        set {
            isOn = value;
            if (isOn == true) {
                laser.enabled = true;
            } else {
                laser.enabled = false;
                firing = false;
                StopAllCoroutines();
                StartCoroutine("DeactivateGun");
            }
        }
    }

    void Awake () {
        gun = GetComponentInChildren<Gun>();
        laser = GetComponentInChildren<LineRenderer>();
        laser.enabled = (isOn) ? true : false;
        player = GameObject.FindGameObjectWithTag("Player");
        pitchTransform = pitchObject.transform;
        yawTransform = yawObject.transform;
        playerTransform = player.transform;
        targetTransform = target.transform;
        pitchTransform.LookAt(target.transform);
        target.transform.position = new Vector3(targetTransform.position[0], playerTransform.position[1], targetTransform.position[2]);
        distToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        SetRandomRotateDirection();
    }

    void LateUpdate() {
        if (isOn) {
            distToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            if (distToPlayer <= sightDistance) {
                if (!firing) {
                    firing = true;
                    StartCoroutine("FireGun");
                }
                if (targetTransform.position != playerTransform.position) {
                    Vector3 pitchToPlayer = player.transform.position - pitchTransform.position;
                    Vector3 yawToPlayer = player.transform.position - yawTransform.position;
                    yawToPlayer[1] = 0.0f;
                    yawTransform.forward = Vector3.RotateTowards(yawTransform.forward, yawToPlayer, 0.025f, 0.0f);
                    newTargetDist.Set(0.0f, 0.0f, distToPlayer);
                    target.transform.localPosition = Vector3.MoveTowards(targetTransform.localPosition, newTargetDist, 0.0625f);
                    pitchTransform.LookAt(target.transform);
                    laser.SetPosition(1, targetTransform.localPosition);
                }
            } else {
                firing = false;
                StopAllCoroutines();
                newTargetDist.Set(0.0f, 0.0f, 3.0f);
                target.transform.localPosition = Vector3.MoveTowards(targetTransform.localPosition, newTargetDist, 0.0625f);
                pitchTransform.LookAt(target.transform);
                laser.SetPosition(1, targetTransform.localPosition);
                yawTransform.Rotate(randRotateDir * Vector3.up * Time.deltaTime);
            }
        }
    }

    IEnumerator FireGun() {
        while (isOn) {
            gun.UseEquipment();
            yield return new WaitForSeconds(fireWaitTime); 
        }
    }

    IEnumerator DeactivateGun() {
        newTargetDist.Set(0.0f, 0.0f, 1.5f);
        for (int i = 0; i < 100; i++) {
            yield return target.transform.localPosition = Vector3.MoveTowards(targetTransform.localPosition, newTargetDist, 0.1f);
            pitchTransform.LookAt(target.transform);
            if (target.transform.localPosition == newTargetDist) {
                yield break;
            }
        }
    }

    void SetRandomRotateDirection() {
        float temp = Random.Range(-1.0f, 1.0f);
        randRotateDir = (temp >= 0.0f) ? 15.0f : -15.0f;
    }
}
