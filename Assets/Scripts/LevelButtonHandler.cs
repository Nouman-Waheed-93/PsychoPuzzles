using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class LevelButtonHandler : MonoBehaviour {
    public static LevelButtonHandler instance;

    public GameObject LvlBtnsP;
    public Button LvlBtn;
    public RectTransform content;
    public Sprite Done, Locked, PlayableLvl;

    public Text FloorTxt;

    private int currPage;
    private int buttonsPerPage;

    private Button[] AllLvlBtns;

    private JsonData LevelsData;
	// Use this for initialization
	void Start () {
        string jsonString = Resources.Load<TextAsset>("LevelsData").text;
        LevelsData = JsonMapper.ToObject(jsonString);
        currPage = 0;
        CreateButtons();
        instance = this;
        gameObject.SetActive(false);
	}

    void CreateButtons()
    {
        AllLvlBtns = new Button[200];
      for (int i = 0; i < 200; i++) {
            AllLvlBtns[i] = Instantiate(LvlBtn, LvlBtnsP.transform.GetChild(Mathf.FloorToInt(i/20)));
        }
    }

    void ResetButtons() {
        int currLevel = Mathf.Clamp(PlayerPrefs.GetInt("ClearedLevels", 0), 0, 200);

        for (int i = 0; i < 200; i++)
        {
            if (i == currLevel)
            {
                AllLvlBtns[i].interactable = true;
                AllLvlBtns[i].image.sprite = PlayableLvl;
                AllLvlBtns[i].transform.GetChild(1).gameObject.SetActive(true);
                AllLvlBtns[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
            }
            else
            {
                AllLvlBtns[i].interactable = false;
                AllLvlBtns[i].transform.GetChild(1).gameObject.SetActive(false);
                if (i > currLevel)
                {
                    AllLvlBtns[i].image.sprite = Locked;
                    AllLvlBtns[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
                }
                else if (i < currLevel)
                {
                    AllLvlBtns[i].image.sprite = Done;
                    AllLvlBtns[i].GetComponentInChildren<Text>().text = "";
                }
            }
            AllLvlBtns[i].transform.localScale = Vector3.one;
            AllLvlBtns[i].onClick.RemoveAllListeners();
            int LvlIndToLoad = currPage * buttonsPerPage + i + 1;
            AllLvlBtns[i].onClick.AddListener(delegate { LoadLevel(LvlIndToLoad); });
        }
    }

    Vector2 DragStartPos;

    void Update() {
        if (Input.GetMouseButtonDown(0))
            OnDragStart();
        else if (Input.GetMouseButtonUp(0))
            OnDragEnd();
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SoundHandler.instance.PlayClick();
            MainMenu.instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OnDragStart() {
        DragStartPos = Input.mousePosition;
    }

    public void OnDragEnd() {
        float diff = Input.mousePosition.x - DragStartPos.x;
        if (Mathf.Abs(diff) > 50) {
            currPage = (int)Mathf.Clamp(currPage - Mathf.Sign(diff), 0, 9);
            content.anchoredPosition =
                new Vector2(Mathf.Clamp(currPage * -800, -7200, 0), 0);
            FloorTxt.text = "Floor " + currPage;
         }
    }

    public void LoadLevel(int Lvl) {
        SoundHandler.instance.PlayClick();
        Lvl = Mathf.Clamp(Lvl, 1, 200);
        int BN = int.Parse(LevelsData[Lvl.ToString()]["Number Displayed on the Bomb"].ToString());
        int NC = int.Parse(LevelsData[Lvl.ToString()]["Number of Chances"].ToString());
        MainGame.instance.SetLevel(BN, NC, Lvl);
        gameObject.SetActive(false);
    }

    public void ShowLevelScreen() {
        ResetButtons();
        StngsRollDownHandler.instance.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OnLeaderBoardBtnClicked() {
        gameObject.SetActive(false);
        SoundHandler.instance.PlayClick();
        LBScreen.instance.ShowLeaderBoard();
    }

}
