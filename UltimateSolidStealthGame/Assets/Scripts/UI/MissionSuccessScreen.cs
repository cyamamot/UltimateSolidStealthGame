using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSuccessScreen : MonoBehaviour {

    [SerializeField]
    SubMissionManager[] subMissions;
    [SerializeField]
    RawImage[] starImages;
    [SerializeField]
    Text[] missionStatusList;
    [SerializeField]
    RectTransform[] subMissionPanels;
    [SerializeField]
    RectTransform titlePanel;


    int numSuccesses;
    float ratio;
    Vector3 scale;
    RectTransform screenRect;
    float subMissionHeight;
    float titleHeight;
    int screenWidthHalf;
    //LevelManager levelManager;

    void Awake() {
        numSuccesses = 1;
        subMissions = GetComponents<SubMissionManager>();
        subMissionHeight = subMissionPanels[0].rect.height;
        titleHeight = titlePanel.rect.height;
        Vector2 newSize = new Vector2();
        foreach (RawImage star in starImages) {
            star.rectTransform.localScale = Vector3.zero;
        }
        foreach (RectTransform panel in subMissionPanels) {
            newSize.Set(0, subMissionHeight);
            panel.sizeDelta = newSize;
        }
        newSize.Set(0, titleHeight);
        titlePanel.sizeDelta = newSize;
        screenRect = GetComponent<RectTransform>();
        screenWidthHalf = (int)(GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>().ScreenWidth / 2.0f);
        scale = new Vector3();
        //levelManager = GetComponentInParent<LevelManager>();
    }

    void Start() {
        gameObject.SetActive(false);
        enabled = false;
    }

    public void DisplaySuccessScreen() {
        Time.timeScale = 0.0f;
        gameObject.SetActive(true);
        missionStatusList[0].text = "[ S U C C E S S ]";
        int index = 1;
        foreach (SubMissionManager manager in subMissions) {
            if (manager.MissionCompleted) {
                missionStatusList[index].text = "[ S U C C E S S ]";
                missionStatusList[index].color = Color.green;
                numSuccesses++;
            } else {
                missionStatusList[index].text = "[ F A I L U R E ]";
                missionStatusList[index].color = Color.red;
            }
            index++;
        }
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
        StartCoroutine("ShowSuccessTitle");
    }

    IEnumerator ShowSuccessTitle() {
        Vector2 newSize = new Vector2();
        for (int i = 0; i <= 600; i += 50) {
            if (i == 600) i = 575;
            newSize.Set(i, titleHeight);
            yield return titlePanel.sizeDelta = newSize;
        }
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine("PopStars");
        StartCoroutine("ShowSubMissions");
    }

    IEnumerator ShowSubMissions() {
        Vector2 newSize = new Vector2();
        foreach (RectTransform panel in subMissionPanels) {
            for (int i = 0; i <= 700; i += 50) {
                newSize.Set(i, subMissionHeight);
                yield return panel.sizeDelta = newSize;
            }
            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    IEnumerator PopStars() {
        foreach (RawImage star in starImages) {
            if (numSuccesses > 0) {
                for (int i = 0; i <= 6; i++) {
                    ratio = i / 4.0f;
                    scale.Set(ratio, ratio, ratio);
                    yield return star.rectTransform.localScale = scale;
                }
                for (int i = 6; i >= 4; i--) {
                    ratio = i / 4.0f;
                    scale.Set(ratio, ratio, ratio);
                    yield return star.rectTransform.localScale = scale;
                }
                numSuccesses--;
                yield return new WaitForSecondsRealtime(0.5f);
            } else {
                yield break;
            }
        }
    }

    /*public void ReturnToMainMenu() {
        levelManager.BackToMain();
    }

    public void ReplayLevel() {
        levelManager.RetryLevel();
    }

    public void NextLevel() {
        levelManager.GoToNextLevel();
    }*/

    //private void OnEnable() {
        //DisplaySuccessScreen();
    //}
}

