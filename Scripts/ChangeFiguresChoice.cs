using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFiguresChoice : MonoBehaviour
{
    static bool choiceMenuIsOpen = true;
    static public GameObject personalChoiceW;
    static public GameObject personalChoiceB;
    static public List<GameObject> children;
    static public string yourChoice = null;

    void Start()
    {
        personalChoiceW = GameObject.Find("PersonalChoiceW");
        personalChoiceB = GameObject.Find("PersonalChoiceB");
        children = new List<GameObject> { };
        int counter = 0;
        // �������� ��������� �������� � ����
        foreach (Transform child in personalChoiceW.GetComponentInChildren<Transform>())
        {
            children.Add(child.gameObject);
            counter++;
        }

        foreach (Transform child in personalChoiceB.GetComponentInChildren<Transform>())
        {
            children.Add(child.gameObject);
            counter++;
        }
        Debug.Log("Clidren counter = " +  counter);
        personalChoiceW.SetActive(false);
        personalChoiceB.SetActive(false);

    }

    // ��������� � ��������� �� ������ ���� � ������� �����
    // �������� ���� ����� � �����, ����� ���� �������� �� ���������� ���������� ���� ����� ��� �����,
    // ����� ��������� �������� ���� ����� ������ �� ����� �������
    public static void OpenPersonalChoice()
    {
        if (choiceMenuIsOpen)
        {
            personalChoiceW.SetActive(true);
            personalChoiceB.SetActive(true);
        }
        else 
        { 
            personalChoiceW.SetActive(false);
            personalChoiceB.SetActive(true);
        }
        choiceMenuIsOpen = !choiceMenuIsOpen;
    }

    // �������� ���� �� ������� ����� �� ����� � ������ ������ ����� �����������
    // ���� ������ ������� ��� �������� ������ � �������
    // ��� ���������� �������� ������������ ��������
    public void GetPersonalChoice(GameObject currentChoice)
    {
        yourChoice = currentChoice.name[currentChoice.name.Length-1].ToString();
        choiceMenuIsOpen = false;
        OpenPersonalChoice();
    }
}
