using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class FaceTime : MonoBehaviour {

    [SerializeField]
    string[] callerText;
    [SerializeField]
    GameObject textBoxObject;

    VideoPlayer player;
    RawImage image;
    RenderTexture text;
    Text callerTextBox;
    RawImage background;
    bool runningText;                   //Whether text is currently running in TextBox
    bool canStartText;
    int currTextIndex;

    void Start() {
        Time.timeScale = 0.0f;
        textBoxObject.transform.localScale = Vector3.zero;
        player = GetComponent<VideoPlayer>();
        image = GetComponent<RawImage>();
        background = transform.parent.GetComponent<RawImage>();
        callerTextBox = transform.parent.GetComponentInChildren<Text>();
        image.transform.localScale = Vector3.zero;
        text = new RenderTexture((int)player.clip.width, (int)player.clip.height, 0);
        player.targetTexture = text;
        image.texture = text;
        StartCoroutine("FaceTimePopUp");
    }

    void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            if (canStartText) {
                if (runningText) {
                    StopAllCoroutines();
                    textBoxObject.transform.localScale = Vector3.one;
                    ResetCallerFace();
                    callerTextBox.text = callerText[currTextIndex];
                    if (currTextIndex < callerText.Length) currTextIndex++;
                    else FinishCall();
                    runningText = false;
                } else {
                    if (currTextIndex < callerText.Length) {
                        StopAllCoroutines();
                        StartCoroutine(DisplayText(callerText[currTextIndex]));
                    } else {
                        FinishCall();
                    }
                }
            }
        }
	}

    void FinishCall() {
        textBoxObject.SetActive(false);
        StopAllCoroutines();
        StartCoroutine("FaceTimeClose");
        canStartText = false;
    }

    IEnumerator FaceTimePopUp() {
        Vector3 newScale = new Vector3();
        float scale;
        for (int i = 0; i <= 16; i += 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return image.rectTransform.localScale = newScale;
        }
        for (int i = 16; i >= 10; i -= 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return image.rectTransform.localScale = newScale;
        }
        yield return new WaitForSeconds(0.25f);
        canStartText = true;
        StartCoroutine(DisplayText(callerText[0]));
    }

    IEnumerator FaceTimeClose() {
        Vector3 newScale = new Vector3();
        float scale;
        Color temp = background.color;
        for (int i = 10; i <= 16; i += 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return image.rectTransform.localScale = newScale;
        }
        for (int i = 16; i >= 0; i -= 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return image.rectTransform.localScale = newScale;
        }
        yield return new WaitForSeconds(1.0f);
        for (int i = 20; i >= 0; i--) {
            temp.a = i / 20.0f;
            yield return background.color = temp;
        }
        Debug.Log(Time.timeScale);
        background.gameObject.SetActive(false);
    }

    IEnumerator DisplayText(string text) {
        player.Play();
        callerTextBox.text = "";
        //runningText = false;
        runningText = true;
        textBoxObject.transform.localScale = Vector3.zero;
        Vector3 newScale = new Vector3();
        float scale;
        for (int i = 0; i <= 14; i += 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return textBoxObject.transform.localScale = newScale;
        }
        for (int i = 14; i >= 10; i -= 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return textBoxObject.transform.localScale = newScale;
        }
        yield return new WaitForSeconds(0.05f);
        //runningText = true;
        foreach (char character in text) {
            yield return callerTextBox.text += character;
        }
        runningText = false;
        currTextIndex++;
        ResetCallerFace();
    }

    void ResetCallerFace() {
        Debug.Log(player.frame);
        player.Stop();
        player.frame = 1;
        Debug.Log(player.frame);
    }
}
