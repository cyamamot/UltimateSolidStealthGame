using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeSelectionScreen : MonoBehaviour {

    [SerializeField]
    GameObject currSwipeTextObject;
    [SerializeField]
    List<string> swipeTypeNames;

    Text currSwipeText;
    string currSwipeName;
    int count;
    int currIndex;

	void Awake () {
        count = swipeTypeNames.Count;
        currIndex = PlayerPrefs.GetInt("SwipeType", 0);
        currSwipeText = currSwipeTextObject.GetComponent<Text>();
	}

    private void Update() {
        currSwipeText.text = swipeTypeNames[currIndex];
    }

    public void LeftButtonClick() {
        currIndex = (currIndex - 1 < 0) ? count - 1 : currIndex - 1;
        PlayerPrefs.SetInt("SwipeType", currIndex);
    }

    public void RightButtonClick() {
        currIndex = (currIndex + 1 >= count) ? 0 : currIndex + 1;
        PlayerPrefs.SetInt("SwipeType", currIndex);
    }

    public void ReturnToMenuClick() {
        gameObject.SetActive(false);
    }
}
