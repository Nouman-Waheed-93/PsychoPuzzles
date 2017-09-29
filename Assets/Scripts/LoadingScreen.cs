using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public static LoadingScreen instance;
    public Slider LoadingSlider;
    public Text LoadingTxt;
    public Text StatusTxt;
    
	void Start () {
        instance = this;
        LoadingTxt.text = "20%";
        LoadingSlider.value = 0.2f;
        Invoke("Load60P", 0.5f);
        Invoke("Load100P", 1f);
        Invoke("LoadingDone", 1.5f);
	}

   public void Load0P() {

        LoadingSlider.value = 0f;
        LoadingTxt.text = "0 %";

    }

   public void Load60P() {

        LoadingSlider.value = 0.6f;
        LoadingTxt.text = "60 %";

    }

   public void Load100P() {

        LoadingSlider.value = 0.6f;
        LoadingTxt.text = "100 %";
        PsychoTut.instance.gameObject.SetActive(true);

    }

    public void SetFBText() {
        StatusTxt.text = "Connecting ...";
    }

    public void ShowLoading() {
        gameObject.SetActive(true);
    }

    public void LoadingDone() {
        gameObject.SetActive(false);
    }

}
