using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour {

    [SerializeField]
    List<Image> specialImages;

    Slider health;
    float maxHealth;
    float currHealth;

	// Use this for initialization
	void Start () {
        health = GetComponentInChildren<Slider>();
        health.value = 1.0f;
    }
	
    public void SetMaxHealth(float max) {
        maxHealth = max;
        currHealth = maxHealth;
    }

	public void SetHealth(float h) {
        currHealth = h;
        health.value = currHealth / maxHealth;
    }

    public void SpecialDamage() {
        if (specialImages.Count > 0) {
            specialImages[0].gameObject.SetActive(false);
            specialImages.RemoveAt(0);
        }
    }
}
