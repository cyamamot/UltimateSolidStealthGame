using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionFailedScreen : MonoBehaviour {

    [SerializeField]
    RectTransform titleRect;
    [SerializeField]
    RectTransform tipRect;
    [SerializeField]
    Text tipText;
    [SerializeField]
    string[] tips;

    float ratio;
    Vector3 scale;
    RectTransform screenRect;
    float hintWidth;
    float titleHeight;
    int screenWidthHalf;
    //LevelManager levelManager;

    void Awake() {
        scale = new Vector3();
        screenRect = GetComponent<RectTransform>();
        hintWidth = tipRect.rect.width;
        titleHeight = titleRect.rect.height;
        Vector2 newSize = new Vector2();
        newSize.Set(0, titleHeight);
        titleRect.sizeDelta = newSize;
        newSize.Set(hintWidth, 0);
        tipRect.sizeDelta = newSize;
        screenWidthHalf = (int)(GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>().ScreenWidth / 2.0f);
        scale = new Vector3();
    }

    void Start() {
        gameObject.SetActive(false);
        enabled = false;
    }

    public void DisplayFailedScreen() {
        Time.timeScale = 0.0f;
        gameObject.SetActive(true);
        int randHint = Random.Range(0, tips.Length - 1);
        tipText.text = tips[randHint];
        StartCoroutine("ShowDisplay");
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
        yield return new WaitForSecondsRealtime(0.05f);
        StartCoroutine("ShowFailTitle");
    }

    IEnumerator ShowFailTitle() {
        Vector2 newSize = new Vector2();
        for (int i = 0; i <= 600; i += 50) {
            if (i == 600) i = 575;
            newSize.Set(i, titleHeight);
            yield return titleRect.sizeDelta = newSize;
        }
        yield return new WaitForSecondsRealtime(0.375f);
        StartCoroutine("ShowHint");
    }

    IEnumerator ShowHint() {
        Vector2 newSize = new Vector2();
        for (int i = 0; i <= 150; i += 15) {
            newSize.Set(hintWidth, i);
            yield return tipRect.sizeDelta = newSize;
        }
    }

    /*public void ReplayLevel() {
        levelManager.RetryLevel();
    }

    public void BackToMenu() {
        levelManager.BackToMain();
    }*/

    //private void OnEnable() {
        //DisplayFailedScreen();
    //}
}
