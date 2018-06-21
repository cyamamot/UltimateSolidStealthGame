using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	string levelName;
	[SerializeField]
	int level;
	[SerializeField]
	List<GameObject> equipmentForLevel;
	[SerializeField]
	string nextLevel;
    [SerializeField]
    AudioClip ambientNoise;
    [SerializeField]
    MissionSuccessScreen successScreen;
    [SerializeField]
    MissionFailedScreen failedScreen;
    [SerializeField]
    FaceTime faceTime;
    [SerializeField]
    Image blackoutScreen;

	PlayerManager manager;
    int playerCurrLevel;
    AudioSource bgmSource;
    AudioSource sfxGod;
    float initialBGMVolume;
    float initialSFXVolume;

    public string LevelName {
		get { return levelName; }
	}
    public AudioSource BGMSource {
        get { return bgmSource; }
    }
    public AudioSource SFXGod {
        get { return sfxGod; }
    }
    public float InitialBGMVolume {
        get { return initialBGMVolume; }
    }
    public float InitialSFXVolume {
        get { return initialSFXVolume; }
    }

	void Awake () {
        enabled = false;
		manager = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerManager> ();
        manager.Swipe.enabled = false;
        bgmSource = GetComponent<AudioSource>();
        bgmSource.ignoreListenerPause = true;
        sfxGod = GameObject.FindGameObjectWithTag("SFXGod").GetComponent<AudioSource>();
        initialBGMVolume = bgmSource.volume;
        initialSFXVolume = sfxGod.volume;
        foreach (GameObject e in equipmentForLevel) {
			manager.WeaponSystem.AddEquipment (e);
		}
        StartCoroutine("Whiteout");
    }

    public void StartLevelMusic() {
        bgmSource.volume = initialBGMVolume * PlayerPrefs.GetFloat("Music", 1.0f);
        sfxGod.volume = initialSFXVolume * PlayerPrefs.GetFloat("SFX", 1.0f);
        bgmSource.Play();
        if (ambientNoise) InvokeRepeating("MakeAmbientNoise", 0.0f, ambientNoise.length * 1.2f);
        manager.Swipe.enabled = true;
    }

    void MakeAmbientNoise() {
        sfxGod.PlayOneShot(ambientNoise, 0.125f * PlayerPrefs.GetFloat("Music", 1.0f));
    }

    public void LevelFailed() {
        failedScreen.DisplayFailedScreen();
    }

	public void FinishLevel() {
        bgmSource.Stop();
        successScreen.DisplaySuccessScreen();
        if (nextLevel != "") {
            if (playerCurrLevel < level) {
				PlayerPrefs.SetInt ("CurrentLevel", level);
            }
        }
	}

    public void GoToNextLevel() {
        StartCoroutine("Blackout");
    }

    public void RetryLevel() {
        SceneManager.LoadScene(LevelName);
    }

    public void BackToMain() {
        SceneManager.LoadScene("Scenes/MainMenu/MainMenu");
    }

    IEnumerator Blackout() {
        blackoutScreen.gameObject.SetActive(true);
        Color blackout = Color.black;
        for (int i = 0; i <= 25; i++) {
            blackout.a = i / 25.0f;
            yield return blackoutScreen.color = blackout;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        if (nextLevel != "") {
            SceneManager.LoadScene(nextLevel);
        }
    }

    IEnumerator Whiteout() {
        yield return new WaitForSecondsRealtime(0.5f);
        Color whiteout = Color.black;
        for (int i = 25; i >= 0; i--) {
            whiteout.a = i / 25.0f;
            yield return blackoutScreen.color = whiteout;
        }
        yield return new WaitForSecondsRealtime(0.75f);
        blackoutScreen.gameObject.SetActive(false);
        faceTime.StartCall();
    }
}
