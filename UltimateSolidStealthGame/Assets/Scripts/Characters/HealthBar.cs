using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    GameObject target;
    float maxHealth;
    Vector3 loc;

    public GameObject Target {
        set { target = value; }
    }
    public float MaxHealth {
        set {
            maxHealth = value;
            Vector3 scale = transform.localScale;
            scale[0] = maxHealth;
            transform.localScale = scale;
        }
    }

    void Update() {
        loc = target.transform.position;
        loc[1] = transform.position[1];
        transform.position = loc;
    }

    public void MinusHealth(float currHealth) {
        Vector3 temp = transform.localScale;
        temp[0] = currHealth;
        transform.localScale = temp;
        if (currHealth <= 0.0f) {
            Destroy(this.gameObject);
        }
    }
}
