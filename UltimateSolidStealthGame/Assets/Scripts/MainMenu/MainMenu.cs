using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    GameObject newGameScreen;
    [SerializeField]
    GameObject optionsScreen;
    [SerializeField]
    string firstLevelName;

    int currLevel;

    public string FirstLevelName {
        get { return firstLevelName; }
    }

	void Awake () {
        currLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        newGameScreen.SetActive(false);
        optionsScreen.SetActive(false);
	}
	
	public void NewGameClick() {
        if (currLevel <= 0) {
            SceneManager.LoadScene(firstLevelName);
        }
        else {
            newGameScreen.SetActive(true);
        }
    }

    public void LevelSelectClick() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OptionsClick() {
        optionsScreen.SetActive(true);
    }

    public void StoreClick() {
        SceneManager.LoadScene("Store");
    }
}
