using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance;
   
    void Start() {
        instance = this;
        gameObject.SetActive(false);
    }

    public void ToLevelScreen() {
        SoundHandler.instance.PlayClick();
        gameObject.SetActive(false);
        LevelButtonHandler.instance.ShowLevelScreen();
    }

}
