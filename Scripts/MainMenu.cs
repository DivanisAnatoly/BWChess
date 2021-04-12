using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GameObject HelpBox;
    private void Start()
    {
        HelpBox = GameObject.Find("HelpBox");
    }
    public void NextScene(int _SceneId)
    {
        SceneManager.LoadScene(_SceneId);
    }
    public void QuitGame()
    {
        Debug.Log("Bye");
        Application.Quit();
    }
    
    public void HelpBoxOnoff()
    { 
        if (HelpBox.activeSelf)
        {
            HelpBox.SetActive(false);
        }
        else HelpBox.SetActive(true);

    }


}
