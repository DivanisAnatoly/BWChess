using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    bool musicIsPlaying = true;
    public GameObject speaker;
    public GameObject audisourse;
    public Sprite SpeakerOn;
    public Sprite SpeakerOff;
    // Start is called before the first frame update
    void Start()
    {
        audisourse.GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(audisourse);
    }

    // Update is called once per frame
    public void SwitchMusic()
    {
        if (!musicIsPlaying)
        {
            audisourse.GetComponent<AudioSource>().Play();
            speaker.GetComponent<Image>().sprite = SpeakerOn;

        }
        else if (musicIsPlaying)
        {
            audisourse.GetComponent<AudioSource>().Pause();
            speaker.GetComponent<Image>().sprite = SpeakerOff;
        }
        musicIsPlaying = !musicIsPlaying;
    }
}
