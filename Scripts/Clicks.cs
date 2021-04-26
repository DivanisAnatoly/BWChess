using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Clicks
{
    //Получение фигуры по клику с помощью пускания лучей с точки клика
    static public Transform GetItemAt(Vector3 position)
    {
        Constraints constraints = new Constraints();
        position.z = 100f;
        Vector3 direction = new Vector3(position.x, position.y, 0f);
        RaycastHit2D[] figures = Physics2D.RaycastAll(position, direction, 0.5f);
        Debug.Log("GetItemAT");
        Debug.Log(figures.Length);
        if (figures.Length == 0 || !constraints.CheckClickFigureOnBoard(figures[0].transform))
        {
            Debug.Log("nullllll");
            return null;
        }
        return figures[0].transform;
    }

    //Возвращение события, была ли нажата кнопка мыши?
    static public bool IsMouseButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    //Возвращение координат клика мышкой
    static public Vector2 GetClickPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
