using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychoTut : MonoBehaviour {

    public static PsychoTut instance;

    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void OnTap()
    {
        gameObject.SetActive(false);
        MainMenu.instance.gameObject.SetActive(true);
    }

}
