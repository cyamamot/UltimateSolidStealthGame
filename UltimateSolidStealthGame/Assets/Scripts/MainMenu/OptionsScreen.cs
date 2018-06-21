using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScreen : MonoBehaviour {

    [SerializeField]
    GameObject musicSliderObject;
    [SerializeField]
    GameObject sfxSliderObject;
    [SerializeField]
    GameObject vibrationToggleObject;
    [SerializeField]
    GameObject backToMainObject;
    [SerializeField]
    bool inGameOptions;

    bool changeReady;
    LevelManager levelManager;
    RectTransform screenRect;
    int screenWidthHalf;

    void Start() {
        Slider musicSlider = musicSliderObject.GetComponent<Slider>();
        Slider sfxSlider = sfxSliderObject.GetComponent<Slider>();
        Toggle vibrationToggle = vibrationToggleObject.GetComponent<Toggle>();
        if (inGameOptions) {
            backToMainObject.SetActive(true);
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            screenWidthHalf = (int)(GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>().ScreenWidth / 2.0f);
        } else {
            backToMainObject.SetActive(false);
            screenWidthHalf = (int)(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width / 2.0f);
        }
        screenRect = GetComponent<RectTransform>();
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1.0f);
        vibrationToggle.isOn = (PlayerPrefs.GetInt("Vibration", 1) == 1) ? true : false;
        changeReady = true;
        gameObject.SetActive(false);
    }
	
	public void OnMusicSliderChange(float value) {
        if (changeReady) {
            PlayerPrefs.SetFloat("Music", value);
            if (levelManager) levelManager.BGMSource.volume = (levelManager.InitialBGMVolume * value) / 2.0f;
        }
    }

    public void OnSFXSliderChange(float value) {
        if (changeReady) {
            PlayerPrefs.SetFloat("SFX", value);
            if (levelManager) levelManager.SFXGod.volume = levelManager.InitialSFXVolume * value;
        }
    }

    public void OnVibrationToggle() {
        if (changeReady) {
            int vibrationValue = PlayerPrefs.GetInt("Vibration", 1);
            if (vibrationValue == 0) {
                PlayerPrefs.SetInt("Vibration", 1);
            } else {
                PlayerPrefs.SetInt("Vibration", 0);
            }
        }
    }

    public void OnXClick() {
        StartCoroutine("HideDisplay");
    }

    /*public void OnBackToMainClick() {
        //SceneManager.LoadScene("Scenes/MainMenu/MainMenu");
        levelManager.BackToMain();
    }*/

    void OnEnable() {
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        if (levelManager) levelManager.BGMSource.volume = levelManager.BGMSource.volume / 2.0f;
        if (changeReady) StartCoroutine("ShowDisplay");
    }

    void OnDisable() {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        if (levelManager) levelManager.BGMSource.volume = levelManager.InitialBGMVolume * PlayerPrefs.GetFloat("Music", 1.0f);
    }

    IEnumerator ShowDisplay() {
        float height = screenRect.rect.height;
        Vector2 newSize = new Vector2();
        for (int i = screenWidthHalf; i > -100; i -= 100) {
            if (i < 0) i = 0;
            newSize.Set(i, 0);
            screenRect.offsetMin = newSize;
            screenRect.offsetMax = -newSize;
            yield return null;
        }
    }

    IEnumerator HideDisplay() {
        float height = screenRect.rect.height;
        Vector2 newSize = new Vector2();
        for (int i = 0; i <= screenWidthHalf; i += 100) {
            newSize.Set(i, 0);
            screenRect.offsetMin = newSize;
            screenRect.offsetMax = -newSize;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
