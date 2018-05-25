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

    bool toggleReady;

	void Awake() {
        Slider musicSlider = musicSliderObject.GetComponent<Slider>();
        Slider sfxSlider = sfxSliderObject.GetComponent<Slider>();
        Toggle vibrationToggle = vibrationToggleObject.GetComponent<Toggle>();
        if (inGameOptions) {
            backToMainObject.SetActive(true);
        } else {
            backToMainObject.SetActive(false);
        }
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1.0f);
        vibrationToggle.isOn = (PlayerPrefs.GetInt("Vibration", 1) == 1) ? true : false;
        toggleReady = true;
	}
	
	public void OnMusicSliderChange(float value) {
        PlayerPrefs.SetFloat("Music", value);
    }

    public void OnSFXSliderChange(float value) {
        PlayerPrefs.SetFloat("SFX", value);
    }

    public void OnVibrationToggle() {
        if (toggleReady) {
            Debug.Log("Toggled");
            int vibrationValue = PlayerPrefs.GetInt("Vibration", 1);
            if (vibrationValue == 0) {
                PlayerPrefs.SetInt("Vibration", 1);
            }
            else {
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
}
