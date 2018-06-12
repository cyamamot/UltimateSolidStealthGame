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
    [SerializeField]
    Text incomingCallText;

    VideoPlayer player;
    RawImage image;
    RenderTexture text;
    Text callerTextBox;
    RawImage phoneIcon;
    RawImage background;
    LevelManager levelManager;
    bool runningText;                   //Whether text is currently running in TextBox
    bool canStartText;
    int currTextIndex;
    Vector3 newScale;
    float scale;

    void Start() {
        AudioListener.pause = true;
        newScale = new Vector3();
        textBoxObject.transform.localScale = Vector3.zero;
        player = GetComponent<VideoPlayer>();
        phoneIcon = incomingCallText.GetComponentInChildren<RawImage>();
        phoneIcon.rectTransform.localScale = Vector3.zero;
        incomingCallText.enabled = false;
        image = GetComponent<RawImage>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        background = transform.parent.GetComponent<RawImage>();
        callerTextBox = textBoxObject.GetComponentInChildren<Text>();
        image.transform.localScale = Vector3.zero;
        text = new RenderTexture((int)player.clip.width, (int)player.clip.height, 0);
        player.targetTexture = text;
        image.texture = text;
        StartCoroutine("PlayPhoneIcon");
    }

    void Update () {
        if (canStartText) {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
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

    IEnumerator PlayPhoneIcon() {
        Vector3 startPos = phoneIcon.transform.localPosition;
        Vector3 leftPos = startPos;
        Vector3 rightPos = leftPos;
        leftPos.x = -7;
        rightPos.x = 7;
        for (int i = 0; i <= 16; i += 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return phoneIcon.rectTransform.localScale = newScale;
        }

        for (int i = 16; i >= 10; i -= 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return phoneIcon.rectTransform.localScale = newScale;
        }
        incomingCallText.enabled = true;
        for (int i = 0; i < 50; i++) {
            if (i == 24) {
                yield return new WaitForSeconds(1.0f);
            } else {
                if (i % 2 == 0) {
                    yield return phoneIcon.rectTransform.localPosition = leftPos;
                }
                else {
                    yield return phoneIcon.rectTransform.localPosition = rightPos;
                }
            }
        }
        yield return phoneIcon.rectTransform.localPosition = startPos;
        yield return new WaitForSeconds(0.5f);
        incomingCallText.enabled = false;
        for (int i = 10; i <= 16; i += 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return phoneIcon.rectTransform.localScale = newScale;
        }
        for (int i = 16; i >= 0; i -= 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return phoneIcon.rectTransform.localScale = newScale;
        }
        incomingCallText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("FaceTimePopUp");
    }

    void FinishCall() {
        textBoxObject.SetActive(false);
        StopAllCoroutines();
        StartCoroutine("FaceTimeClose");
        canStartText = false;
    }

    IEnumerator FaceTimePopUp() {
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
        AudioListener.pause = false;
        levelManager.StartLevelMusic();
        background.gameObject.SetActive(false);
    }

    IEnumerator DisplayText(string text) {
        player.Play();
        callerTextBox.text = "";
        runningText = true;
        textBoxObject.transform.localScale = Vector3.zero;
        for (int i = 0; i <= 12; i += 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return textBoxObject.transform.localScale = newScale;
        }
        for (int i = 12; i >= 10; i -= 2) {
            scale = i / 10.0f;
            newScale.Set(scale, scale, scale);
            yield return textBoxObject.transform.localScale = newScale;
        }
        yield return new WaitForSeconds(0.05f);
        foreach (char character in text) {
            yield return callerTextBox.text += character;
        }
        runningText = false;
        currTextIndex++;
        ResetCallerFace();
    }

    void ResetCallerFace() {
        player.Pause();
        player.frame = 0;
    }
}
