using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Constraints
{
    //Проверка, нахождение точных координат клетки после клика, если клик не на доске, то возвращает невозможную константу
    public Vector2 CheckSquare(Vector2 clickPosition)
    {
        Vector2 newCoordsSquare = clickPosition;

        if (newCoordsSquare.x < GameObjects.RightBottomCornerDownOnBoard().transform.position.x &&
            newCoordsSquare.y < GameObjects.LeftBottomCornerUpOnBoard().transform.position.y &&
            newCoordsSquare.x > GameObjects.LeftBottomCornerDownOnBoard().transform.position.x &&
            newCoordsSquare.y > GameObjects.LeftBottomCornerDownOnBoard().transform.position.y)
        {
            newCoordsSquare.x = Convert.ToInt32(Math.Truncate((newCoordsSquare.x - GameObjects.LeftBottomCornerDownOnBoard().transform.position.x) / GameObjects.SquareSize()));
            newCoordsSquare.y = Convert.ToInt32(Math.Truncate((newCoordsSquare.y - GameObjects.LeftBottomCornerDownOnBoard().transform.position.y) / GameObjects.SquareSize()));
        }
        else newCoordsSquare = new Vector2(9999.9f, 9999.9f);
        return newCoordsSquare;
    }

    //Проверка, находится ли кликнутая фигура на доске (true/false)?
    public bool CheckClickFigureOnBoard(Transform figure)
    {
        if (figure == null) return false;
        if (CheckSquare(figure.position).x == 9999.9f && CheckSquare(figure.position).y == 9999.9f)
            return false;
        return true;
    }

    //Проверка, фигура - противник (true) или союзник (false)?
    public bool CheckEnemyFigure(Transform goFigure, GameObject item)
    {
        if ((goFigure.name[0] >= 'A' && goFigure.name[0] <= 'Z' && item.name[0] >= 'a' && item.name[0] <= 'z') ||
           (goFigure.name[0] >= 'a' && goFigure.name[0] <= 'z' && item.name[0] >= 'A' && item.name[0] <= 'Z'))
        {
            return true;
        }
        return false;
    }

    //Попытка срубить фигуру, если на клетке противник - перемещение фигуры на поле поверженных фигур
    public bool CheckTryCutFigure(GameObject goSquare, GameObject item)
    {
        Transform goFigure = Clicks.GetItemAt(goSquare.transform.position);
        if (!goFigure) return true;
        GameObject fieldForDefeatFigures = GameObject.Find("Square" + goFigure.name[0]);
        bool checkEnemy = CheckEnemyFigure(goFigure, item);

        if (checkEnemy)
        {
            goFigure.transform.position = fieldForDefeatFigures.transform.position;
            goFigure.tag = "Static";
            RaycastHit2D[] figures = Physics2D.RaycastAll(fieldForDefeatFigures.transform.position, fieldForDefeatFigures.transform.position, 0.5f); //Заготовка для счётчика
            Debug.Log(figures.Length);
            return true;
        }
        else
        {
            Debug.Log(goFigure.name);
            return false;
        }
    }
}
