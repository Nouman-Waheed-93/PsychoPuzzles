using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBScreen : MonoBehaviour {

    public static LBScreen instance;
    public LeaderBoardElement UserLBElement;

	// Use this for initialization
	void Start () {

        instance = this;
        gameObject.SetActive(false);

	}

    public void ShowLeaderBoard() {
        gameObject.SetActive(true);
        GameMenu.instance.RedrawUI();
    }

    public void BackToLevelScreen() {
        SoundHandler.instance.PlayClick();
        gameObject.SetActive(false);
        LevelButtonHandler.instance.ShowLevelScreen();
    }

}
