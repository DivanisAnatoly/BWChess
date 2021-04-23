using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PieceCreator : MonoBehaviour
{
    static public List<string> ProbableMoves = new List<string>() { "Pa2a3", "Pb2b7" };
    static public List<Parser> parser = new List<Parser>(PieceCreator.ProbableMoves.Count);
    private string fen = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
    PieceMoves pieceMoves;

    void Start()
    {
        ShowFigures();
        parser = GetParseListForMoves();
        pieceMoves = new PieceMoves();
    }

    void Update()
    {
        pieceMoves.Action();
    }

    //Разместить фигуры на доске
    public void ShowFigures()
    {
        char figure;
        int invertBoard = -4;
        int countCreatedFigures = 0;
        TranformFen(out string newfen);
        string[] linesBoard = newfen.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        
        for (int y = 7; y >= 0; y--)
        {
            invertBoard++;
            for (int x = 0; x < 8; x++)
            {
                figure = linesBoard[y][x];
                if (figure == '.') continue;
                PlaceFigure(figure.ToString(), x, y, invertBoard);
                countCreatedFigures++;
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

    //Превращение ФЕНа
    void TranformFen(out string newfen)
    {
        newfen = fen;
        for (int j = 8; j >= 2; j--)
            newfen = newfen.Replace(j.ToString(), (j - 1).ToString() + "1");
        newfen = newfen.Replace("1", ".");
        return;
    }

    //Получение ходов из будущей библиотеки
    static public List<Parser> GetParseListForMoves()
    {
        for (int i = 0; i < ProbableMoves.Count; i++)
        {
            parser.Add(new Parser(ProbableMoves[i]));
        }
        return parser;
    }
}
