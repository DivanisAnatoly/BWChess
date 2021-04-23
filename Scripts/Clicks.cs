using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Clicks
{
    //Получение фигуры по клику с помощью пускания лучей с точки клика
    static public Transform GetItemAt(Vector3 position)
    {
        RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);
        if (figures.Length == 0 || !Constraints.CheckClickFigureOnBoard(figures[0].transform))
            return null;
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
