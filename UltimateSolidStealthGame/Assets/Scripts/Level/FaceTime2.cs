using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class FaceTime2 : MonoBehaviour {

    [System.Serializable]
    public class DialoguePair {
        [TextArea]
        public string dialogue;
        public bool isCaller;
    }

    [SerializeField]
    GameObject faceTimeMongoose;
    [SerializeField]
    GameObject textBox;
    [SerializeField]
    GameObject callEnded;
    [SerializeField]
    DialoguePair[] dialogue;

    PlayerUI ui;
    RawImage callerImage;
    VideoPlayer callerPlayer;
    RenderTexture callerTexture;
    RectTransform mongooseRect;
    RawImage mongooseImage;
    VideoPlayer mongoosePlayer;
    RenderTexture mongooseTexture;
    Text text;
    CanvasGroup groupAlpha;
    LevelManager manager;
    Vector2 mongooseRectNewSize;
    int currDialogueIndex;
    bool dialogueBegan;

	void Start () {
        enabled = false;
        ui = GetComponentInParent<PlayerUI>();
        mongooseRect = faceTimeMongoose.GetComponent<RectTransform>();
        callerPlayer = GetComponent<VideoPlayer>();
        mongoosePlayer = faceTimeMongoose.GetComponent<VideoPlayer>();
        text = textBox.GetComponentInChildren<Text>();
        groupAlpha = GetComponent<CanvasGroup>();
        manager = GetComponentInParent<LevelManager>();
        callerImage = GetComponent<RawImage>();
        mongooseImage = faceTimeMongoose.GetComponent<RawImage>();
        mongooseRectNewSize = new Vector2(ui.ScreenWidth, ui.ScreenHeight);
        mongooseRect.sizeDelta = mongooseRectNewSize;
        callerTexture = new RenderTexture((int)callerPlayer.clip.width, (int)callerPlayer.clip.height, 0);
        callerPlayer.targetTexture = callerTexture;
        callerImage.texture = callerTexture;
        mongooseTexture = new RenderTexture((int)mongoosePlayer.clip.width, (int)mongoosePlayer.clip.height, 0);
        mongoosePlayer.targetTexture = mongooseTexture;
        mongooseImage.texture = mongooseTexture;
        mongoosePlayer.Pause();
        callerPlayer.Pause();
        textBox.SetActive(false);
        callEnded.SetActive(false);
	}

    public void StartCall() {
        StartCoroutine(SettingUpCall());
    }

	IEnumerator SettingUpCall() {
        yield return new WaitForSecondsRealtime(1.0f);
        int newMongooseWidth = (int)(mongooseRect.rect.width / 3.0f);
        int newMongooseHeight = (int)(mongooseRect.rect.height / 3.0f);
        for (int i = 3; i > 0; i--) {
            mongooseRectNewSize.Set(newMongooseWidth * i, newMongooseHeight * i);
            yield return mongooseRect.sizeDelta = mongooseRectNewSize;
        }
        textBox.SetActive(true);
        SetDialogue();
        dialogueBegan = true;
    }

    void SetDialogue() {
        if (dialogue[currDialogueIndex].isCaller) {
            callerPlayer.Play();
            mongoosePlayer.Pause();
            mongoosePlayer.frame = 0;
        } else {
            mongoosePlayer.Play();
            callerPlayer.Pause();
            callerPlayer.frame = 0;
        }
        text.text = dialogue[currDialogueIndex].dialogue;
    }

    void NextDialogue() {
        if (dialogueBegan) {
            currDialogueIndex++;
            if (currDialogueIndex < dialogue.Length) {
                SetDialogue();
            }
            else {
                StartCoroutine(EndCall());
            }
        }
    }

    IEnumerator EndCall() {
        mongooseImage.color = Color.black;
        callerImage.color = Color.black;
        mongooseImage.texture = null;
        faceTimeMongoose.SetActive(false);
        callerImage.texture = null;
        textBox.SetActive(false);
        callEnded.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        float ratio;
        for (int i = 50; i >= 0; i--) {
            ratio = i / 50.0f;
            yield return groupAlpha.alpha = ratio;
        }
        gameObject.SetActive(false);
    }

    public void OnButtonClick() {
        NextDialogue();
    }
}
