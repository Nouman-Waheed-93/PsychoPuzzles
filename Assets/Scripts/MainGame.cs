using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour {

    public static MainGame instance;
    public Text bmbTxt;
    public Text MovesLeftTxt, WinScoreTxt;
    public Slider timeBar;
    public Text timeTxt;
    public AutoType RemarksTxt;
    public GameObject RedLightGO, GreenLightGO;
    public string[] LoseRemarks, WinRemarks;
    public GameObject LoseGO, WinGO;
    public GameObject ExplosionGO;
    private int currNumChances;
    private float currBmbNumber;
    private int LvlNumChances;
    private float LvlBmbNumber;
    private int currLvlInd;

    private delegate float Calculate(float number);
    Calculate currFunct;

    void Start() {
        instance = this;
        gameObject.SetActive(false);
        GlobalVals.Score = PlayerPrefs.GetInt("Score", 0);
   }

    void Update() {
        timeBar.value -= Time.deltaTime;
        timeTxt.text = (int)timeBar.value + " sec";
        if (timeBar.value < 10 && !RedLightGO.activeSelf)
            RedLightGO.SetActive(true);
        if (timeBar.value <= 0 && !LoseGO.activeSelf)
        {
            LoseGame();
        }
    }

    public void SetLevel(float BmbNumber, int numChances, int currLvlInd) {

        GreenLightGO.SetActive(false);
        RedLightGO.SetActive(false);
        StngsRollDownHandler.instance.gameObject.SetActive(true);
        this.currLvlInd = currLvlInd;
        LvlBmbNumber = currBmbNumber = BmbNumber;
        LvlNumChances = currNumChances = numChances;
        bmbTxt.text = BmbNumber.ToString();
        timeBar.value = 120;
        LoseGO.SetActive(false);
        WinGO.SetActive(false);
        ClockSound.instance.gameObject.SetActive(SoundHandler.instance.AudioOn);
        gameObject.SetActive(true);
        MovesLeftTxt.text = "Moves :" + currNumChances;

    }

    public void SetFunctProd() {
        currFunct = Multiply;
        SoundHandler.instance.PlayClick();
    
    }

    public void SetFunctDiv() {
        currFunct = Divide;
        SoundHandler.instance.PlayClick();
   
    }

    public void setFunctSub() {
        currFunct = Subtract;
        SoundHandler.instance.PlayClick();
   
    }

    public void SetFunctAdd() {
        currFunct = Add;
        SoundHandler.instance.PlayClick();
  
    }

    public void PerformFunct(int num) {
        SoundHandler.instance.PlayClick();
        if (currFunct == null)
            return;
        float answer = currFunct(num);
        if (answer != Mathf.Floor(answer))
        {
            LoseGame();
        }
        bmbTxt.text = answer.ToString();
        currFunct = null;
        currNumChances--;
        MovesLeftTxt.text = "Moves :" + currNumChances;
        if (currNumChances < 2 && !RedLightGO.activeSelf) {
            RedLightGO.SetActive(true);
        }
        if (currNumChances > -1 && currBmbNumber == 10)
        {
            RedLightGO.SetActive(false);
            GreenLightGO.SetActive(true);
            RemarksTxt.message[0] = WinRemarks[Random.Range(0, WinRemarks.Length - 1)];
            RemarksTxt.RestartTyping();
            ClockSound.instance.gameObject.SetActive(false);
            StngsRollDownHandler.instance.gameObject.SetActive(false);
            PlayerPrefs.SetInt("ClearedLevels", currLvlInd);
            GlobalVals.Score = GlobalVals.Score + 250;
            PlayerPrefs.SetInt("Score", GlobalVals.Score);
            WinScoreTxt.text = GlobalVals.Score.ToString();
            Invoke("ActivateWinGO", 1);
        }
        else if (currNumChances < 1) {
            LoseGame();
        }
    }

    public void OnPublishScoreClick() {

        FBLogin.PromptForPublish(CanPublish);


    }

    public void CanPublish() {
        FBShare.PostScore(GlobalVals.Score);
    }

    void ActivateWinGO() {
        WinGO.SetActive(true);
    }

    private float Multiply(float num) {
        return currBmbNumber *= num;
    }

    private float Divide(float num) {
        return currBmbNumber /= num;
    }

    private float Subtract(float num) {
       return currBmbNumber -= num;
    }

    private float Add(float num) {
        return currBmbNumber += num;
    }

    void LoseGame() {
        RedLightGO.SetActive(true);
        RemarksTxt.message[0] = LoseRemarks[Random.Range(0, LoseRemarks.Length - 1)];
        RemarksTxt.RestartTyping();
        ClockSound.instance.gameObject.SetActive(false);
        SoundHandler.instance.PlayExplosion();
        ExplosionGO.SetActive(true);
        StngsRollDownHandler.instance.gameObject.SetActive(false);
        LoseGO.SetActive(true);
    }

    public void DisableExplosion() {
        ExplosionGO.SetActive(false);
    }

    public void Retry() {
        RedLightGO.SetActive(false);
        GreenLightGO.SetActive(false);
        StngsRollDownHandler.instance.gameObject.SetActive(true);
        currBmbNumber = LvlBmbNumber;
        currNumChances = LvlNumChances;
        bmbTxt.text = currBmbNumber.ToString();
        timeBar.value = 120;
        MovesLeftTxt.text = "Moves :" + currNumChances;
        LoseGO.SetActive(false);
        WinGO.SetActive(false);
        gameObject.SetActive(false);
        PsychoIntro.instance.SetNextScreen(true);
        PsychoIntro.instance.gameObject.SetActive(true);
 
    }

    public void QuitToLvlScreen() {
        gameObject.SetActive(false);
        LevelButtonHandler.instance.ShowLevelScreen();
    }

    public void BackToLvlScreen() {
        gameObject.SetActive(false);
        LevelButtonHandler.instance.ShowLevelScreen();
        PsychoIntro.instance.SetNextScreen(false);
        PsychoIntro.instance.gameObject.SetActive(true);
    }

    public void NextLevel() {
        LevelButtonHandler.instance.LoadLevel(currLvlInd + 1);
        PsychoIntro.instance.SetNextScreen(true);
        PsychoIntro.instance.gameObject.SetActive(true);
    }

}
