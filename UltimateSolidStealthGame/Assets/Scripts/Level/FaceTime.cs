using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class FaceTime : MonoBehaviour {

    [SerializeField]
    [TextArea]
    string[] callerText;
    [SerializeField]
    GameObject textBoxObject;
    [SerializeField]
    RectTransform callerRect;
    [SerializeField]
    CanvasGroup incomingCallPanel;
    [SerializeField]
    GameObject faceTimePanel;

    VideoPlayer player;
    RawImage image;
    RenderTexture texture;
    Text callerTextBox;
    RawImage background;
    LevelManager levelManager;
    bool runningText;                   //Whether text is currently running in TextBox
    bool canStartText;
    int currTextIndex;
    RectTransform screenRect;
    int screenWidthHalf;

    void Start() {
        AudioListener.pause = true;
        textBoxObject.transform.localScale = Vector3.zero;
        player = callerRect.GetComponent<VideoPlayer>();
        image = callerRect.GetComponent<RawImage>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        callerTextBox = textBoxObject.GetComponentInChildren<Text>();
        texture = new RenderTexture((int)player.clip.width, (int)player.clip.height, 0);
        player.targetTexture = texture;
        image.texture = texture;
        screenRect = GetComponent<RectTransform>();
        screenWidthHalf = (int)(GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>().ScreenWidth / 2.0f);
        faceTimePanel.SetActive(false);
        gameObject.SetActive(false);
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

    public void StartCall() {
        gameObject.SetActive(true);
        StartCoroutine("FlashIncomingCall");
    }

    void FinishCall() {
        textBoxObject.SetActive(false);
        StopAllCoroutines();
        StartCoroutine("FaceTimeClose");
        canStartText = false;
    }

    IEnumerator FlashIncomingCall() {
        for (int j = 0; j < 2; j++) {
            for (int i = 0; i <= 20; i++) {
                yield return incomingCallPanel.alpha = i / 20.0f;
            }
            for (int i = 20; i >= 0; i--) {
                yield return incomingCallPanel.alpha = i / 20.0f;
            }
        }
        incomingCallPanel.gameObject.SetActive(false);
        StartCoroutine("FaceTimePopUp");
    }

    IEnumerator FaceTimePopUp() {
        faceTimePanel.SetActive(true);
        float height = screenRect.rect.height;
        Vector2 newSize = new Vector2();
        for (int i = screenWidthHalf; i > -100; i -= 100) {
            if (i < 0) i = 0;
            newSize.Set(i, 0);
            screenRect.offsetMin = newSize;
            screenRect.offsetMax = -newSize;
            yield return null;
        }
        for (int i = 0; i <= 500; i += 100) {
            newSize.Set(i, 250);
            yield return callerRect.sizeDelta = newSize;
        }
        canStartText = true;
        yield return new WaitForSeconds(0.125f);
        StartCoroutine(DisplayText(callerText[0]));
    }

    IEnumerator FaceTimeClose() {
        float height = screenRect.rect.height;
        Vector2 newSize = new Vector2();
        for (int i = 500; i >= 0; i -= 100) {
            newSize.Set(i, 250);
            yield return callerRect.sizeDelta = newSize;
        }
        for (int i = 0; i <= screenWidthHalf + 100; i += 100) {
            int j = i;
            if (i > screenWidthHalf) j = screenWidthHalf;
            newSize.Set(j, 0);
            screenRect.offsetMin = newSize;
            screenRect.offsetMax = -newSize;
            yield return null;
        }
        image.enabled = false;
        AudioListener.pause = false;
        levelManager.StartLevelMusic();
        gameObject.SetActive(false);
    }

    IEnumerator DisplayText(string text) {
        player.Play();
        callerTextBox.text = "";
        runningText = true;
        textBoxObject.transform.localScale = Vector3.zero;
        float scale;
        Vector3 newScale = new Vector3();
        for (int i = 0; i <= 10; i += 2) {
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
