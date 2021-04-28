using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    bool musicIsPlaying = true;
    public GameObject speaker;
    public Sprite SpeakerOn;
    public Sprite SpeakerOff;
    // Start is called before the first frame update
    void Start()
    {
        //speaker.GetComponent<AudioSource>().Play();
    }

    public void SwitchMusic()
    {
        GameObject audioSourse = GameObject.Find("AudioSource");
        if (!musicIsPlaying)
        {
            audioSourse.GetComponent<AudioSource>().Play();
            speaker.GetComponent<Image>().sprite = SpeakerOn;

        }
        else if (musicIsPlaying)
        {
            audioSourse.GetComponent<AudioSource>().Pause(); 
            speaker.GetComponent<Image>().sprite = SpeakerOff;
        }
        musicIsPlaying = !musicIsPlaying;
    }
}
