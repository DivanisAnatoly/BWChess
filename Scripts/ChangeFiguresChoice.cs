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
        // Сохраняю найденных потомков в лист
        foreach (Transform child in personalChoice.GetComponentInChildren<Transform>())
        {
            children.Add(child.gameObject);
            counter++;
        }
        Debug.Log("Clidren counter = " +  counter);
        personalChoice.SetActive(false);

    }

    // Открываем и закрываем по кнопке окно с выбором фигур
    // Вызвыаем этот метод в месте, когда идет проверка на достижение последнего ряда доски для пешки,
    // Чтобы корректно работало надо будет кнопку со сцены удалить
    public static void OpenPersonalChoice()
    {
        if (choiceMenuIsOpen)
        {
            personalChoice.SetActive(true);
        }
        else { personalChoice.SetActive(false); }
        choiceMenuIsOpen = !choiceMenuIsOpen;
    }

    // Выбираем одну из четырех фигур на выбор и панель выбора фигур закрывается
    // Пока просто выводит имя выбраной фигуры в консоли
    // При интеграции заменить возвращаемое значение
    public void GetPersonalChoice(GameObject currentChoice)
    {
        yourChoice = currentChoice.name[currentChoice.name.Length-1].ToString();
        choiceMenuIsOpen = false;
        OpenPersonalChoice();
    }
}
