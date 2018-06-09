using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorPulse : MonoBehaviour {

    [SerializeField]
    Color startColor;
    [SerializeField]
    Color endColor;

    Image image;
    float ratio;

    void Awake() {
        image = GetComponent<Image>();
    }

    void OnEnable() {
        StartCoroutine("PulseColor");
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    IEnumerator PulseColor() {
        while (enabled) {
            for (int i = 0; i <= 40; i++) {
                ratio = i / 40.0f;
                yield return image.color = Color.Lerp(startColor, endColor, ratio);
            }
            for (int i = 40; i >= 0; i--) {
                ratio = i / 40.0f;
                yield return image.color = Color.Lerp(startColor, endColor, ratio);
            }
        }
    }
}
