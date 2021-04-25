using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeFiguresChoice : MonoBehaviour
{
    bool choiceMenuIsOpen = true;
    public GameObject personalChoice;
    List<GameObject> children;
    void Start()
    {
        children = new List<GameObject> { };
        int counter = 0;
        // �������� ��������� �������� � ����
        foreach (GameObject child in personalChoice.GetComponentInChildren<Transform>())
        {
            children.Add(child);
            counter++;
        }
        Debug.Log("Clidren counter = " +  counter);
        personalChoice.SetActive(false);

    }
    // ��������� � ��������� �� ������ ���� � ������� �����
    // �������� ���� ����� � �����, ����� ���� �������� �� ���������� ���������� ���� ����� ��� �����,
    // ����� ��������� �������� ���� ����� ������ �� ����� �������
    public void OpenPersonalChoice(GameObject personalChoice)
    {
        if (choiceMenuIsOpen)
        {
            personalChoice.SetActive(true);
        }
        else { personalChoice.SetActive(false); }
        foreach (GameObject child in children)
        {
            if (choiceMenuIsOpen)
                child.SetActive(true);
            else child.SetActive(false);
            Debug.Log(child.name);
        }
        choiceMenuIsOpen = !choiceMenuIsOpen;
    }

    // �������� ���� �� ������� ����� �� ����� � ������ ������ ����� �����������
    // ���� ������ ������� ��� �������� ������ � �������
    // ��� ���������� �������� ������������ ��������
    public void GetPersonalChoice(GameObject currentChoice)
    {
        Debug.Log("Selected figure - " + currentChoice.name[currentChoice.name.Length-1]);
        choiceMenuIsOpen = false;
        OpenPersonalChoice(personalChoice);
    }
}
