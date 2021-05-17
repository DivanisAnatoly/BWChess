using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChessLibrary;
using UnityEngine.UI;

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

    //Попытка срубить фигуру, если на клетке противник - перемещение фигуры на поле поверженных фигур
    public void CheckTryCutFigure(GameObject toMoveFigure)
    {
        if (!toMoveFigure) return;
        MovingFigureOnDefeat(toMoveFigure);
    }

    public static string CheckColorFigure(GameObject ClickObject, GameManager gameManager)
    {
        string InGameColor = gameManager.GetInGameColor();
        if (ClickObject.name[0] >= 'A' && ClickObject.name[0] <= 'Z' && InGameColor == "white") return "white";
        else if (ClickObject.name[0] >= 'a' && ClickObject.name[0] <= 'z' && InGameColor == "black") return "black";
        return null;
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

    public void MovingFigureOnDefeat(GameObject toMoveFigure)
    {
        GameObject fieldForDefeatFigures = GameObject.Find("Square" + toMoveFigure.name[0]);
        toMoveFigure.transform.position = fieldForDefeatFigures.transform.position;
        toMoveFigure.name = toMoveFigure.name[0].ToString();
        toMoveFigure.tag = "Static";
        RaycastHit2D[] figures = Physics2D.RaycastAll(fieldForDefeatFigures.transform.position, fieldForDefeatFigures.transform.position, 0.5f); 
        ChangeCounterFigure(toMoveFigure, figures.Length);
    }

    void ChangeCounterFigure(GameObject defeatFigure, int countFigureOnDefeatField)
    {
        GameObject defeatField = GameObject.Find(defeatFigure.name + "GraveCounter");
        defeatField.GetComponent<Text>().text = "x" + countFigureOnDefeatField.ToString();
    }
}
