using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StngsRollDownHandler : MonoBehaviour {

    public static StngsRollDownHandler instance;
    public Image StngBtn, StngRollDn;
    public Image SoundBtnImg, MusicBtnImg;
    public Sprite SoundOnSprt, SoundOffSprt, MusicOnSprt, MusicOffSprt;
    public GameObject MusicGO;

    void Start () {

        gameObject.SetActive(false);
        instance = this;

	}

    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            if (StngRollDn.gameObject.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(StngRollDn.rectTransform, Input.mousePosition)){
                StngBtn.gameObject.SetActive(true);
                StngRollDn.gameObject.SetActive(false);
                SoundHandler.instance.PlayClick();
            }
        }
	}

    public void OnHidStngBtn() {
        StngBtn.gameObject.SetActive(true);
        StngRollDn.gameObject.SetActive(false);
        SoundHandler.instance.PlayClick();
    }

    public void OnStngBtnClicked() {
        SoundHandler.instance.PlayClick();
        StngBtn.gameObject.SetActive(false);
        StngRollDn.gameObject.SetActive(true);
    }

    public void OnSoundBtnClicked() {
        if (SoundHandler.instance.AudioOn)
        {
            SoundHandler.instance.AudioOn = false;
            ClockSound.instance.gameObject.SetActive(false);
            SoundBtnImg.sprite = SoundOffSprt;
        }
        else {
            SoundHandler.instance.AudioOn = true;
            ClockSound.instance.gameObject.SetActive(true);
            SoundBtnImg.sprite = SoundOnSprt;
            SoundHandler.instance.PlayClick();
        }
    }

    public void OnMusicBtnClicked() {
        if (MusicGO.activeSelf)
        {
            MusicGO.SetActive(false);
            MusicBtnImg.sprite = MusicOffSprt;
        }
        else {
            MusicGO.SetActive(true);
            MusicBtnImg.sprite = MusicOnSprt;
        }
    }

}
