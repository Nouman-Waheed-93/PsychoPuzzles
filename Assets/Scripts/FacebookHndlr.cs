using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System;

public class FacebookHndlr : MonoBehaviour {

    private static readonly List<string> readPermissions = new List<string> { "public_profile", "user_friends" };
    private static readonly List<string> publishPermissions = new List<string> { "publish_actions" };

    public static FacebookHndlr instance;

    public Button LoginButton;
    public Button ShareButton;
    public GameObject PlayerLoggedINUI;
    public Image FBProfilePhoto;
    public Text UsernameTxt;
    // Use this for initialization
    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
            OnLoginComplete();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFBBClicked()
    {
        FB.LogInWithReadPermissions(readPermissions, delegate (ILoginResult result)
        {
            LoginButton.interactable = false;
            Debug.Log("LoginCallback");
            if (FB.IsLoggedIn)
            {
                Debug.Log("Donee od");
            }
            else
            {
                if (result.Error != null)
                {
                    Debug.LogError(result.Error);
                }
                Debug.Log("Not Logged In");
            }

            OnLoginComplete();

        });
    }

    private void OnLoginComplete()
    {
        Debug.Log("OnLoginComplete");

        if (!FB.IsLoggedIn)
        {
            // Reenable the Login Button
            LoginButton.gameObject.SetActive(true);
            PlayerLoggedINUI.SetActive(false);
            // ShareButton.interactable = false;
            return;
        }

        // Show loading animations
        LoginButton.gameObject.SetActive(false);
        PlayerLoggedINUI.SetActive(true);
        GetPlayerInfo();

    }

    public static void GetPlayerInfo()
    {
        string queryString = "/me?fields=id,first_name";
        FB.API(queryString, HttpMethod.GET, GetPlayerInfoCallback);
    }


    private static void GetPlayerInfoCallback(IGraphResult result)
    {
        Debug.Log("GetPlayerInfoCallback");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }
        Debug.Log(result.RawResult);

        // Save player name
        string name;
        if (result.ResultDictionary.TryGetValue("first_name", out name))
        {
            instance.UsernameTxt.text = name;
            FB.API("/me/picture?redirect=false", HttpMethod.GET, ProfilePhotoCallback);

        }


    }

    private static void ProfilePhotoCallback(IGraphResult result)
    {
        if (String.IsNullOrEmpty(result.Error) && !result.Cancelled)
        {
            IDictionary data = result.ResultDictionary["data"] as IDictionary;
            string photoURL = data["url"] as String;

            instance.StartCoroutine(fetchProfilePic(photoURL));
        }
    }

    private static IEnumerator fetchProfilePic(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        instance.FBProfilePhoto.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
    }

    public void OnShareClicked()
    {
        FB.LogInWithPublishPermissions(publishPermissions, delegate (ILoginResult result)
        {

            if (result.Error != null || result.Cancelled)
            {
                Debug.LogError(result.Error);
            }
            else
                FB.FeedShare("", new Uri("http://aigamedev.com/open/article/cover-strategies/"), "Be A Fighter Pilot", "Fly your favourite aircraft and suppress your opponents.",
"Just testing some feature for my game, Move on!", new Uri("http://i.imgur.com/dQzWXZG.jpg"), "What source?", SharingDone);

            Debug.Log("Not Logged In");

        });
    }

    public void ShareAchievement(string Title, string Detail, string Link)
    {

        FB.LogInWithPublishPermissions(publishPermissions, delegate (ILoginResult result)
        {

            if (result.Error != null || result.Cancelled)
            {
                Debug.LogError(result.Error);
            }
            else
                FB.FeedShare("", new Uri("http://aigamedev.com/open/article/cover-strategies/"), Title, Detail,
Detail, new Uri(Link), "What source?", AchievmentShared);

            Debug.Log("Not Logged In");

        });

    }

    void AchievmentShared(IShareResult result)
    {

    }

    void SharingDone(IShareResult result)
    {
        if (result.Cancelled || result.Error != null)
            return;
        ShareButton.gameObject.SetActive(false);
    }

}
