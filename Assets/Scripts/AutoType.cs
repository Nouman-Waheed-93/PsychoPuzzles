using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AutoType : MonoBehaviour
{

    public float letterPause = 0.02f;
    public float LetterTypedTime;
    public AudioSource sound;

    public Text SpeechTxt;

    public string[] message;
    
    int currLetterInd = 0;
    int currMessageInd = 0;
    float currMessageDisplayTime;

    public void RestartTyping() {
        currLetterInd = 0;
        currMessageInd = 0;
        SpeechTxt.text = "";
    }

    void Update()
    {
       
        
        LetterTypedTime += Time.deltaTime;
        if (LetterTypedTime > letterPause && currMessageInd < message.Length && currLetterInd < message[currMessageInd].Length)
        {
            LetterTypedTime = 0;
            SpeechTxt.text += message[currMessageInd][currLetterInd];
            currLetterInd++;
            if (!sound.isPlaying)
                sound.Play();
        }
        else if (currMessageInd < message.Length && currLetterInd >= message[currMessageInd].Length) {
            currMessageDisplayTime += Time.deltaTime;
            if (currMessageDisplayTime > 2 && currMessageInd < message.Length -1) {
                SpeechTxt.text = "";
                currMessageDisplayTime = 0;
                currMessageInd++;
                currLetterInd = 0;
            }
            if(sound.isPlaying)
                sound.Stop();
        }
      
    }
}