using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    static bool musicIsPlaying = true;
    public GameObject speaker;
    public Sprite SpeakerOn;
    public Sprite SpeakerOff;
    GameObject audioSourse;
    // Start is called before the first frame update
    void Start()
    {
        audioSourse = GameObject.Find("AudioSource");
        if (!MusicManager.musicIsPlaying) speaker.GetComponent<Image>().sprite = SpeakerOff;
        else speaker.GetComponent<Image>().sprite = SpeakerOn;
    }

    public void SwitchMusic()
    {
        
        if (!MusicManager.musicIsPlaying)
        {
            audioSourse.GetComponent<AudioSource>().Play();
            speaker.GetComponent<Image>().sprite = SpeakerOn;

        }
        else if (MusicManager.musicIsPlaying)
        {
            audioSourse.GetComponent<AudioSource>().Pause(); 
            speaker.GetComponent<Image>().sprite = SpeakerOff;
        }
        MusicManager.musicIsPlaying = !MusicManager.musicIsPlaying;
    }
}
