using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour {

    public static SoundHandler instance;
    public bool AudioOn = true;

    public AudioClip clickSnd, ExplosionSnd;
    private AudioSource mySrc;

    void Start() {
        instance = this;
        mySrc = GetComponent<AudioSource>();
    }

    public void PlayClick() {
        if (AudioOn)
            mySrc.PlayOneShot(clickSnd);
    }

    public void PlayExplosion() {
        if (AudioOn)
            mySrc.PlayOneShot(ExplosionSnd);
    }
    
}
