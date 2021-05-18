using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public static GameObject BlackWins, WhiteWins, Draw;// Start is called before the first frame update
    void Start()
    {
        BlackWins = GameObject.Find("BlackWins");
        WhiteWins = GameObject.Find("WhiteWins");
        Draw = GameObject.Find("Draw");

        BlackWins.SetActive(false);
        WhiteWins.SetActive(false);
        Draw.SetActive(false);
    }
    
    public static void OpenBlackEndGame()
    {
        BlackWins.SetActive(true);
    }

    public static void OpenWhiteEndGame()
    {
        WhiteWins.SetActive(true);
    }

    public static void OpenDrawEndGame()
    {
        Draw.SetActive(true);
    }
}
