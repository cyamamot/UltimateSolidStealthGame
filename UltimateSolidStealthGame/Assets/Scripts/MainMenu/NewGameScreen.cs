using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameScreen : MonoBehaviour {

    MainMenu menu;

	void Awake () {
        menu = GetComponentInParent<MainMenu>();
	}
	
	public void DeleteClick() {
        PlayerPrefs.SetInt("CurrentLevel", 0);
        SceneManager.LoadScene(menu.FirstLevelName);
    }

    public void ReturnToMenuClick() {
        gameObject.SetActive(false);
    }
}
