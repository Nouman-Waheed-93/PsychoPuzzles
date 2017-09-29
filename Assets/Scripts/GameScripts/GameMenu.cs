/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Facebook.Unity;

public class GameMenu : MonoBehaviour
{
    // UI Element References (Set in Unity Editor)
 //   public GameObject LoadingText;
    public GameObject[] LoginButton;
    //  Leaderboard
    public GameObject LeaderboardPanel;
    public GameObject LeaderboardItemPrefab;
    public ScrollRect LeaderboardScrollRect;
    public static GameMenu instance;

    #region Built-In
    void Awake()
    {
        instance = this;
        // Initialize FB SDK
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback);
        }
    }

    // OnApplicationPause(false) is called when app is resumed from the background
    void OnApplicationPause (bool pauseStatus)
    {
        // Do not do anything in the Unity Editor
        if (Application.isEditor) {
            return;
        }

        // Check the pauseStatus for an app resume
        if (!pauseStatus)
        {
            if (FB.IsInitialized)
            {
                // App Launch events should be logged on app launch & app resume
                // See more: https://developers.facebook.com/docs/app-events/unity#quickstart
                FBAppEvents.LaunchEvent();
            }
            else
            {
                FB.Init(InitCallback);
            }
        }
    }

    // OnLevelWasLoaded is called when we return to the main menu
    void OnLevelWasLoaded(int level)
    {
        Debug.Log("OnLevelWasLoaded");
        if (level == 0 && FB.IsInitialized)
        {
            Debug.Log("Returned to main menu");

            // We've returned to the main menu so let's complete any pending score activity
            if (FB.IsLoggedIn)
            {
                RedrawUI();

                // Post any pending High Score
                if (GameStateManager.highScorePending)
                {
                    GameStateManager.highScorePending = false;
                    FBShare.PostScore(GameStateManager.HighScore);
                }
            }
        }
    }
    #endregion

    #region FB Init
    private void InitCallback()
    {
        Debug.Log("InitCallback");

        // App Launch events should be logged on app launch & app resume
        // See more: https://developers.facebook.com/docs/app-events/unity#quickstart
        FBAppEvents.LaunchEvent();

        if (FB.IsLoggedIn) 
        {
            Debug.Log("Already logged in");
            OnLoginComplete();
        }
    }
    #endregion

    #region Login
    public void OnLoginClick ()
    {
        Debug.Log("OnLoginClick");

        // Disable the Login Button
        foreach( GameObject LB in LoginButton)
            LB.gameObject.SetActive(false);
        LoadingScreen.instance.SetFBText();
        LoadingScreen.instance.ShowLoading();
        LoadingScreen.instance.Load0P();
        // Call Facebook Login for Read permissions of 'public_profile', 'user_friends', and 'email'
        FBLogin.PromptForLogin(OnLoginComplete);
    }

    private void OnLoginComplete()
    {
        Debug.Log("OnLoginComplete");

        if (!FB.IsLoggedIn)
        {
            // Reenable the Login Button
            foreach(GameObject LB in LoginButton)
                LB.gameObject.SetActive(true);
            LoadingScreen.instance.LoadingDone();
            return;
        }

        // Show loading animations
        //    LoadingText.SetActive(true);
        LoadingScreen.instance.Load60P();
        foreach(GameObject LB in LoginButton)
            LB.gameObject.SetActive(false);
        // Begin querying the Graph API for Facebook data
        FBGraph.GetPlayerInfo();
        FBGraph.GetFriends();
        
        FBGraph.GetScores();
    }
    #endregion

    #region GUI
    // Method to update the Game Menu User Interface
    public void RedrawUI ()
    {

        if (GameStateManager.UserTexture != null && !string.IsNullOrEmpty(GameStateManager.Username))
        {

            // Disable loading animation
            //         LoadingText.SetActive(false);
            LoadingScreen.instance.LoadingDone();
        }

        var scores = GameStateManager.Scores;
        if (GameStateManager.ScoresReady && scores.Count > 0)
        {
            // Clear out previous leaderboard
            Transform[] childLBElements = LeaderboardPanel.GetComponentsInChildren<Transform>();
            foreach(Transform childObject in childLBElements)
            {
                if(!LeaderboardPanel.transform.IsChildOf(childObject.transform))
                {
                    Destroy(childObject.gameObject);
                }
            }
            

            // Populate leaderboard
            LeaderboardPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 170 * scores.Count);
            // lBRect.Set(0, 0, lBRect.width, 150 * scores.Count);
           for (int i=0; i<scores.Count; i++)
            {
                var entry = (Dictionary<string, object>)scores[i];
                var user = (Dictionary<string, object>)entry["user"];
                string userId = (string)user["id"];

                if (string.Equals(userId, AccessToken.CurrentAccessToken.UserId))
                {
                    // This entry is the current player
                    LBScreen.instance.UserLBElement.SetupElement(i + 1, scores[i]);
                }
                else
                {
                    GameObject LBgameObject = Instantiate(LeaderboardItemPrefab) as GameObject;
                    LeaderBoardElement LBelement = LBgameObject.GetComponent<LeaderBoardElement>();
                    LBelement.SetupElement(i + 1, scores[i]);

                    LBelement.transform.SetParent(LeaderboardPanel.transform, false);
                }
            }

            // Scroll to top
            LeaderboardScrollRect.verticalNormalizedPosition = 1f;
        }

     }
    #endregion
    

    private static void SharedCallbackHo(IShareResult result)
    {
        Debug.Log("ShareCallback");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
        Debug.Log(result.RawResult);
    }
    
}
