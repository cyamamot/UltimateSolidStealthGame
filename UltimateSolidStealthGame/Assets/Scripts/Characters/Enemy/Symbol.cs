using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol : MonoBehaviour {

    [SerializeField]
    Sprite questionSprite;
    [SerializeField]
    Sprite exclamationSprite;

    GameObject target;
    Vector3 loc;
    SpriteRenderer spriteRenderer;
    float ratio;
    Vector3 scale;

    public GameObject Target {
        set { target = value; }
    }

    void Start() {
        scale = new Vector3();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    void Update () {
        loc = target.transform.position;
        loc[1] = transform.position[1];
        transform.position = loc;
    }

    public void PopMark(string type) {
        gameObject.SetActive(true);
        StopAllCoroutines();
        transform.localScale = Vector3.zero;
        if (type == "Exclamation") {
            spriteRenderer.sprite = exclamationSprite;
        } else if (type == "Question") {
            spriteRenderer.sprite = questionSprite;
        }
        StartCoroutine(PopMark());
    }

    IEnumerator PopMark() {
        for (int i = 0; i <= 3; i++) {
            ratio = i / 9.0f;
            scale.Set(ratio, ratio, ratio);
            yield return transform.localScale = scale;
        }
        yield return new WaitForSeconds(0.75f);
        for (int i = 3; i >= 0; i--) {
            ratio = i / 9.0f;
            scale.Set(ratio, ratio, ratio);
            yield return transform.localScale = scale;
        }
        gameObject.SetActive(false);
    }
}
