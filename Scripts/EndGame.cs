using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public static GameObject BlackWins;
    public static GameObject WhiteWins ;
    public static GameObject Draw ;
    GameObject resultSideChoice;
    void Start()
    {
        BlackWins = GameObject.Find("BlackWins");
        WhiteWins = GameObject.Find("WhiteWins");
        Draw = GameObject.Find("Draw");
        resultSideChoice = GameObject.Find("ResultSideChoice");
        BlackWins.SetActive(false);
        WhiteWins.SetActive(false);
        Draw.SetActive(false);
        Debug.Log(BlackWins.name + WhiteWins.name + Draw.name + resultSideChoice.name);
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

    public void Surrend()
    {
        if (resultSideChoice.GetComponent<Text>().text == "black")
        {
            PieceM.SetState(StateAction.mateWhite);
        }
        else if (resultSideChoice.GetComponent<Text>().text == "white")
        {
            PieceM.SetState(StateAction.mateBlack);
        }
    }
}
