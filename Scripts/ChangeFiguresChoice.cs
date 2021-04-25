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
        // Сохраняю найденных потомков в лист
        foreach (GameObject child in personalChoice.GetComponentInChildren<Transform>())
        {
            children.Add(child);
            counter++;
        }
        Debug.Log("Clidren counter = " +  counter);
        personalChoice.SetActive(false);

    }
    // Открываем и закрываем по кнопке окно с выбором фигур
    // Вызвыаем этот метод в месте, когда идет проверка на достижение последнего ряда доски для пешки,
    // Чтобы корректно работало надо будет кнопку со сцены удалить
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

    // Выбираем одну из четырех фигур на выбор и панель выбора фигур закрывается
    // Пока просто выводит имя выбраной фигуры в консоли
    // При интеграции заменить возвращаемое значение
    public void GetPersonalChoice(GameObject currentChoice)
    {
        Debug.Log("Selected figure - " + currentChoice.name[currentChoice.name.Length-1]);
        choiceMenuIsOpen = false;
        OpenPersonalChoice(personalChoice);
    }
}
