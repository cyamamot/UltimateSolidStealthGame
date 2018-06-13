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

	void Start() {
        Slider musicSlider = musicSliderObject.GetComponent<Slider>();
        Slider sfxSlider = sfxSliderObject.GetComponent<Slider>();
        Toggle vibrationToggle = vibrationToggleObject.GetComponent<Toggle>();
        if (inGameOptions) {
            backToMainObject.SetActive(true);
            levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        } else {
            backToMainObject.SetActive(false);
        }

        musicSlider.value = PlayerPrefs.GetFloat("Music", 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1.0f);
        vibrationToggle.isOn = (PlayerPrefs.GetInt("Vibration", 1) == 1) ? true : false;
        changeReady = true;
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
        gameObject.SetActive(false);
    }

    public void OnBackToMainClick() {
        SceneManager.LoadScene("Scenes/MainMenu/MainMenu");
    }

    void OnEnable() {
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
        if (levelManager) levelManager.BGMSource.volume = levelManager.BGMSource.volume / 2.0f;
    }

    void OnDisable() {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        if (levelManager) levelManager.BGMSource.volume = levelManager.InitialBGMVolume * PlayerPrefs.GetFloat("Music", 1.0f);
    }
}
