using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    
    private float timeInSecondsP = 900;
    private StateAction stateAction;
    int minutsP;
    int secondsP;
    GameObject minutes;
    GameObject seconds;
    GameObject resultSideChoice;

    private void Start()
    {
        resultSideChoice = GameObject.Find("ResultSideChoice");
        minutes = GameObject.Find("Current minutes");
        seconds = GameObject.Find("Current seconds");
        //timeInSecondsP = inputMinute * 60 + inputSecond; //Раскомментировать,если понадобится на вход подавать иное время до поражения
    }
    void Update()
    {
        if (resultSideChoice.GetComponent<Text>().text != "")
        {
            stateAction = PieceM.GetState();
            if (timeInSecondsP > 0 && stateAction == StateAction.movePlayer) // Дописать условие из библиотеки!!!!! Если ходит другой игрок - таймер замирает
            {
                timeInSecondsP -= Time.deltaTime;
                secondsP = (int)(timeInSecondsP % 60);
                minutsP = (int)(timeInSecondsP / 60);
                if (minutsP < 10) // Чтобы значение <10 в строке  не теряли 0 в старшем разряде 
                    minutes.GetComponent<Text>().text = 0 + minutsP.ToString();
                else
                    minutes.GetComponent<Text>().text = minutsP.ToString();
                if (secondsP < 10)
                    seconds.GetComponent<Text>().text = 0 + secondsP.ToString();
                else
                    seconds.GetComponent<Text>().text = secondsP.ToString();
            }
            else if (timeInSecondsP <= 0)
            {
                if (resultSideChoice.GetComponent<Text>().text == "black")
                {
                    PieceM.SetState(StateAction.mateWhite);
                    
                }
                else if (resultSideChoice.GetComponent<Text>().text == "white")
                {
                    PieceM.SetState(StateAction.mateBlack);   
                }
                Debug.Log(PieceM.GetState() == StateAction.mateBlack ? "MateBlack" : "MateWhite"  );
            }
        }
    }
 }