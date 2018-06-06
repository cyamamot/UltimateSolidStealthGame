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
    float initialBGMVolume;

	void Awake() {
        gameObject.SetActive(false);
        Slider musicSlider = musicSliderObject.GetComponent<Slider>();
        Slider sfxSlider = sfxSliderObject.GetComponent<Slider>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        Toggle vibrationToggle = vibrationToggleObject.GetComponent<Toggle>();
        if (inGameOptions) {
            backToMainObject.SetActive(true);
        } else {
            backToMainObject.SetActive(false);
        }
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1.0f);
        initialBGMVolume = levelManager.BGMSource.volume;
        levelManager.BGMSource.volume = initialBGMVolume * musicSlider.value;
        vibrationToggle.isOn = (PlayerPrefs.GetInt("Vibration", 1) == 1) ? true : false;
        changeReady = true;
	}
	
	public void OnMusicSliderChange(float value) {
        if (changeReady) {
            PlayerPrefs.SetFloat("Music", value);
            levelManager.BGMSource.volume = (initialBGMVolume * value) / 2.0f;
        }
    }

    public void OnSFXSliderChange(float value) {
        if (changeReady) {
            PlayerPrefs.SetFloat("SFX", value);
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
        gameObject.SetActive(false);
    }

    public void OnBackToMainClick() {
        SceneManager.LoadScene("Scenes/MainMenu/MainMenu");
    }

    void OnEnable() {
        AudioListener.pause = true;
        if (levelManager) levelManager.BGMSource.volume = levelManager.BGMSource.volume / 2.0f;
    }

    void OnDisable() {
        AudioListener.pause = false;
        if (levelManager) levelManager.BGMSource.volume = initialBGMVolume * PlayerPrefs.GetFloat("Music", 1.0f);
    }
}
