using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFiguresChoice : MonoBehaviour
{
    static bool choiceMenuIsOpen = true;
    static public GameObject personalChoice;
    static public List<GameObject> children;
    static public string yourChoice = null;

    void Start()
    {
        personalChoice = GameObject.Find("PersonalChoice");
        children = new List<GameObject> { };
        int counter = 0;
        // �������� ��������� �������� � ����
        foreach (Transform child in personalChoice.GetComponentInChildren<Transform>())
        {
            children.Add(child.gameObject);
            counter++;
        }
        Debug.Log("Clidren counter = " +  counter);
        personalChoice.SetActive(false);

    }

    // ��������� � ��������� �� ������ ���� � ������� �����
    // �������� ���� ����� � �����, ����� ���� �������� �� ���������� ���������� ���� ����� ��� �����,
    // ����� ��������� �������� ���� ����� ������ �� ����� �������
    public static void OpenPersonalChoice()
    {
        if (choiceMenuIsOpen)
        {
            personalChoice.SetActive(true);
        }
        else { personalChoice.SetActive(false); }
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
