using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //раскомментировать, если понадобится задавать иное время до поражения на вход 
    public int inputMinute; 
    public int inputSecond; 
    
    private float timeInSecondsP;
    public static int minutsP;
    public static int secondsP;
    public Text minutes;
    public Text seconds;

    private void Start()
    {
        //timeInSecondsP = 420;// На вход подается 7 минут
        timeInSecondsP = inputMinute * 60 + inputSecond; //Раскомментировать,если понадобится на вход подавать иное время до поражения
    }
    void Update()
    {
        if(timeInSecondsP >= 0 ) // Дописать условие из библиотеки!!!!! Если ходит другой игрок - таймер замирает
        {
            timeInSecondsP -= Time.deltaTime;
            secondsP = (int)(timeInSecondsP % 60); 
            minutsP = (int)(timeInSecondsP / 60); 
            if (minutsP < 10) // Чтобы значение <10 в строке  не теряли 0 в старшем разряде 
                minutes.text = 0 + minutsP.ToString();
            else 
                minutes.text = minutsP.ToString();
            if (secondsP < 10)
                seconds.text = 0 + secondsP.ToString(); 
            else
                seconds.text = secondsP.ToString();
        }
    }
    /*
    bool isMyTurn() {
        return () ? true : false;
    }
    */
 }