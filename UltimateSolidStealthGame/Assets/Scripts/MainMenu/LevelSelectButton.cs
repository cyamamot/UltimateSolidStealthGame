using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour {

    [SerializeField]
    int level;
    [SerializeField]
    string levelName;

    Image fillCircleImage;
    Image buttonImage;
    float fillAmount;
    bool pressed;
    bool levelAvailable;
    int currLevel;

	void Awake () {
        fillCircleImage = transform.parent.GetComponent<Image>();
        fillCircleImage.fillAmount = 0.0f;
        buttonImage = GetComponent<Image>();
        currLevel = PlayerPrefs.GetInt("CurrentLevel", -1);
        if (level <= (currLevel + 1)) {
            levelAvailable = true;
        } else {
            buttonImage.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            levelAvailable = false;
        }
	}
	
	void Update () {
        if (levelAvailable) {
            fillCircleImage.fillAmount = fillAmount;
            if (pressed) {
                fillAmount += 0.025f;
            }
            if (fillAmount >= 1.0f) {
                SceneManager.LoadScene(levelName);
            }
        }
	}

    public void OnButtonPressed() {
        if (levelAvailable) {
            pressed = true;
            fillAmount = 0.0f;
        }
    }

    public void OnButtonReleased() {
        if (levelAvailable) {
            pressed = false;
            fillAmount = 0.0f;
        }
    }
}
