using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PieceCreator : MonoBehaviour
{
    private string fen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

    void Awake()
    {
        Debug.Log(GameObjects.CentreSquareOnBoard().transform.InverseTransformPoint(transform.position));
        ShowFigures();
    }

    //Разместить фигуры на доске
    public void ShowFigures()
    {
        char figure;
        int invertBoard = -4;
        TranformFen(out string newfen);
        string[] linesBoard = newfen.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        ShowSquare();
        for (int y = 7; y >= 0; y--)
        {
            invertBoard++;
            for (int x = 0; x < 8; x++)
            {
                figure = linesBoard[y][x];
                if (figure == '.') continue;
                PlaceFigure(figure.ToString(), x, y, invertBoard);
            }
        }  
    }

    //Создать клоны объекта фигуры и разместить на указанной позиции
    void PlaceFigure(string figure, int x, int y, int invertBoard)
    {

        GameObject spriteFigure = GameObject.Find(figure);
        GameObject currentSquare = GameObject.Find("" + (char)(x + 'a') + (y + 2*invertBoard));
        GameObject currentFigure = Instantiate(spriteFigure, currentSquare.transform.position, currentSquare.transform.rotation);
        currentFigure.name = spriteFigure.name + currentSquare.name;
        Debug.Log($" Создан клон фигуры {currentFigure.name[0]}");

        currentFigure.tag = "Active";
    }

    void ShowSquare()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                PlaceSquare(x, y);
            }
        }
    }

    void PlaceSquare(int x, int y)
    {
        GameObject board = GameObject.Find("Board");
        GameObject instanteSquare = GameObjects.CentreSquareOnBoard();
        Vector3 positionSquare = new Vector3(instanteSquare.transform.position.x + x * 53.62f, instanteSquare.transform.position.y + y * 53.62f, 0f);
        GameObject currentSquare = Instantiate(instanteSquare, positionSquare, instanteSquare.transform.rotation, board.transform);
        currentSquare.name = "" + (char)(x + 'a') + (y + 1);
    }

    //Превращение ФЕНа
    void TranformFen(out string newfen)
    {
        newfen = fen;
        for (int j = 8; j >= 2; j--)
            newfen = newfen.Replace(j.ToString(), (j - 1).ToString() + "1");
        newfen = newfen.Replace("1", ".");
        return;
    }

}
