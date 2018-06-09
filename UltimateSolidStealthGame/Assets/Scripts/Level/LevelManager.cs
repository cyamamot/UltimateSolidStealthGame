using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	PlayerManager manager;
    int playerCurrLevel;
    AudioSource bgmSource;
    AudioSource sfxGod;
    float initialBGMVolume;
    float initialSFXVolume;

    public string LevelName {
		get { return LevelName; }
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
        bgmSource = GetComponent<AudioSource>();
        bgmSource.ignoreListenerPause = true;
        sfxGod = GameObject.FindGameObjectWithTag("SFXGod").GetComponent<AudioSource>();
        initialBGMVolume = bgmSource.volume;
        initialSFXVolume = sfxGod.volume;
        foreach (GameObject e in equipmentForLevel) {
			manager.WeaponSystem.AddEquipment (e);
		}
	}

    public void StartLevelMusic() {
        bgmSource.volume = initialBGMVolume * PlayerPrefs.GetFloat("Music", 1.0f);
        sfxGod.volume = initialSFXVolume * PlayerPrefs.GetFloat("SFX", 1.0f);
        bgmSource.Play();
        if (ambientNoise) InvokeRepeating("MakeAmbientNoise", 0.0f, ambientNoise.length * 1.2f);
    }

    void MakeAmbientNoise() {
        sfxGod.PlayOneShot(ambientNoise, 0.125f * PlayerPrefs.GetFloat("Music", 1.0f));
    }

	public void FinishLevel() {
        //play finish level ui animation
        //Set new playerpref currentlevel
        bgmSource.Stop();
        if (nextLevel != "") {
            if (playerCurrLevel < level) {
				PlayerPrefs.SetInt ("CurrentLevel", level);
            }
			SceneManager.LoadScene(nextLevel);
        }
	}
}
