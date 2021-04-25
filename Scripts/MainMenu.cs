using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
    private void Start()
    {
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
}
