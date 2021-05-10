using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //�����������������, ���� ����������� �������� ���� ����� �� ��������� �� ���� 
    public int inputMinute; 
    public int inputSecond; 
    
    private float timeInSecondsP;
    private StateAction stateAction;
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
        stateAction = PieceM.getState();
        if (timeInSecondsP > 0 && stateAction == StateAction.movePlayer) // �������� ������� �� ����������!!!!! ���� ����� ������ ����� - ������ ��������
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
        else if (timeInSecondsP <= 0)
        {
            PieceM.setState(StateAction.endGame);
            Debug.Log("Mate, You lose.");
        }
    }
 }