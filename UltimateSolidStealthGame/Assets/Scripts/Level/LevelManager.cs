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

	public string LevelName {
		get { return LevelName; }
	}
    public AudioSource BGMSource {
        get { return bgmSource; }
    }

	void Awake () {
        enabled = false;
		manager = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerManager> ();
        bgmSource = GetComponent<AudioSource>();
        bgmSource.ignoreListenerPause = true;
        sfxGod = GameObject.FindGameObjectWithTag("SFXGod").GetComponent<AudioSource>();
        if (ambientNoise) InvokeRepeating("MakeAmbientNoise", 0.0f, 7.0f);
		foreach (GameObject e in equipmentForLevel) {
			manager.WeaponSystem.AddEquipment (e);
		}
		playerCurrLevel = PlayerPrefs.GetInt ("CurrentLevel", -1);
		if (playerCurrLevel != -1) {
			if (playerCurrLevel < level) {
				

				//Play level introduction ui animation
			}
		}
        bgmSource.Play();
	}

    void MakeAmbientNoise() {
        sfxGod.volume = 0.25f * PlayerPrefs.GetFloat("SFX", 1.0f);
        sfxGod.PlayOneShot(ambientNoise);
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
