using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychoIntro : MonoBehaviour {

    public static PsychoIntro instance;

    private bool GameScreenNext = true;

	void Start () {
        instance = this;
        gameObject.SetActive(false);
	}

    public void SetNextScreen(bool GSN) {
        GameScreenNext = GSN;
    }

    public void OnTap() {
        gameObject.SetActive(false);
        if (GameScreenNext)
            MainGame.instance.gameObject.SetActive(true);
        else
            LevelButtonHandler.instance.ShowLevelScreen();
    }

}
