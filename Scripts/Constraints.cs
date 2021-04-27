using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChessLibrary;

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
    public bool CheckClickFigureOnBoard(GameObject figure)
    {
        if (figure == null) return false;
        if (CheckSquare(figure.transform.position).x == 9999.9f && CheckSquare(figure.transform.position).y == 9999.9f)
            return false;
        return true;
    }

    //Проверка, фигура - противник (true) или союзник (false)?
    public bool CheckEnemyFigure(GameObject toMoveFigure, GameObject currentFigure)
    {
        if ((toMoveFigure.name[0] >= 'A' && toMoveFigure.name[0] <= 'Z' && currentFigure.name[0] >= 'a' && currentFigure.name[0] <= 'z') ||
           (toMoveFigure.name[0] >= 'a' && toMoveFigure.name[0] <= 'z' && currentFigure.name[0] >= 'A' && currentFigure.name[0] <= 'Z'))
        {
            return true;
        }
        return false;
    }

    //Попытка срубить фигуру, если на клетке противник - перемещение фигуры на поле поверженных фигур
    public bool CheckTryCutFigure(GameObject toMoveFigure, GameObject currentFigure)
    {
        if (!toMoveFigure) return true;
        RaycastHit2D[] figures;
        GameObject fieldForDefeatFigures = GameObject.Find("Square" + toMoveFigure.name[0]);
        if (CheckEnemyFigure(toMoveFigure, currentFigure))
        {
            toMoveFigure.transform.position = fieldForDefeatFigures.transform.position;
            toMoveFigure.name = toMoveFigure.name[0].ToString();
            toMoveFigure.tag = "Static";
            figures = Physics2D.RaycastAll(fieldForDefeatFigures.transform.position, fieldForDefeatFigures.transform.position, 0.5f); //Заготовка для счётчика
            Debug.Log($" На поле поверженных фигур {toMoveFigure.name} = {figures.Length}");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckColorFigure(GameObject ClickObject, string InGameColor)
    {
        if ((ClickObject.name[0] >= 'A' && ClickObject.name[0] <= 'Z' && InGameColor == "white") ||
           (ClickObject.name[0] >= 'a' && ClickObject.name[0] <= 'z' && InGameColor == "black"))
            return true;
        return false;
    }

    public void GetClickSquare(Vector2 coordClick, out GameObject clickedSquare)
    {
        coordClick = CheckSquare(coordClick);  //Превращение глобальных координат в локальные целые
        if (coordClick == new Vector2(9999.9f, 9999.9f))  //Проверка на клик за пределы доски
        {
            clickedSquare = null;
            return;
        }
        clickedSquare = GameObject.Find("" + (char)(coordClick.x + 'a') + (coordClick.y + 1));
    
    }
}
