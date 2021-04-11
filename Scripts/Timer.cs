using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //�����������������, ���� ����������� �������� ���� ����� �� ��������� �� ���� 
    public int inputMinute; 
    public int inputSecond; 
    
    private float timeInSecondsP;
    public static int minutsP;
    public static int secondsP;
    public Text minutes;
    public Text seconds;

    private void Start()
    {
        //timeInSecondsP = 420;// �� ���� �������� 7 �����
        timeInSecondsP = inputMinute * 60 + inputSecond; //�����������������,���� ����������� �� ���� �������� ���� ����� �� ���������
    }
    void Update()
    {
        if(timeInSecondsP >= 0 ) // �������� ������� �� ����������!!!!! ���� ����� ������ ����� - ������ ��������
        {
            timeInSecondsP -= Time.deltaTime;
            secondsP = (int)(timeInSecondsP % 60); 
            minutsP = (int)(timeInSecondsP / 60); 
            if (minutsP < 10) // ����� �������� <10 � ������  �� ������ 0 � ������� ������� 
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