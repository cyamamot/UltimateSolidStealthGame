using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassImpact : MonoBehaviour {

    [SerializeField]
    float noiseRadius;

    Transform bladesTransform;
    Vector3 halfYScale;
    int ignoreRaycastLayer;
    int hidingPlaceLayer;
    int enemyLayer;
    int defaultLayer;

    void Start () {
        enabled = false;
        bladesTransform = transform.GetChild(0);
        halfYScale = Vector3.one;
        halfYScale[1] = 0.5f;
        ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
        hidingPlaceLayer = LayerMask.NameToLayer("HidingPlace");
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        defaultLayer = 1 << LayerMask.NameToLayer("Default");
    }

    void OnTriggerEnter(Collider other) {
        bladesTransform.localScale = halfYScale;
        gameObject.layer = ignoreRaycastLayer;
        PlayerMovement playerMove = other.GetComponent<PlayerMovement>();
        if (playerMove && !playerMove.IsCrounching) {
            Collider[] hits = Physics.OverlapSphere(transform.position, noiseRadius, enemyLayer, QueryTriggerInteraction.Ignore);
            EnemyMovement enemyMove;
            foreach (Collider hit in hits) {
                enemyMove = hit.GetComponent<EnemyMovement>();
                if (enemyMove) {
                    enemyMove.SetPathToSound(transform.position);
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        bladesTransform.localScale = Vector3.one;
        gameObject.layer = hidingPlaceLayer;
    }
}
