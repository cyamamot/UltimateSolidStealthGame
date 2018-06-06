using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour {

    [SerializeField]
    List<NameImagePair> specialPics;
    [SerializeField]
    List<Image> specialImages;

    Slider health;
    float maxHealth;
    float currHealth;

    [System.Serializable]
    public class NameImagePair {
        public string name;
        public Sprite image;
    };

	void Start () {
        health = GetComponentInChildren<Slider>();
        health.value = 0.0f;
        StartCoroutine("HealthAnimation");
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

    public void SetSpecial(string name) {
        if (specialImages.Count > 0) {
            foreach (NameImagePair pair in specialPics) {
                if (pair.name == name) {
                    specialImages[0].sprite = pair.image;
                    specialImages[0].gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    IEnumerator HealthAnimation() {
        for (int i = 0; i <= 40; i++) {
            yield return health.value = i / 40.0f;
        }
        yield break;
    }
}
