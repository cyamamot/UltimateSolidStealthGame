using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentButton : MonoBehaviour {

    float scale;
    Vector3 newScale;

    // Use this for initialization
    void Start () {
        enabled = false;
        newScale = new Vector3();
	}
	
	public void Pop() {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine("PopOpen");
    }

    public void Close() {
        StopAllCoroutines();
        StartCoroutine("PopClose");
    }

    IEnumerator PopOpen() {
        for (int i = 0; i <= 3; i += 1) {
            scale = i / 2.0f;
            newScale.Set(scale, scale, scale);
            yield return transform.localScale = newScale;
        }
        for (int i = 3; i >= 2; i -= 1) {
            scale = i / 2.0f;
            newScale.Set(scale, scale, scale);
            yield return transform.localScale = newScale;
        }
    }

    IEnumerator PopClose() {
        for (int i = 2; i <= 3; i += 1) {
            scale = i / 2.0f;
            newScale.Set(scale, scale, scale);
            yield return transform.localScale = newScale;
        }
        for (int i = 3; i >= 0; i -= 1) {
            scale = i / 2.0f;
            newScale.Set(scale, scale, scale);
            yield return transform.localScale = newScale;
        }
        gameObject.SetActive(false);
    }
}
