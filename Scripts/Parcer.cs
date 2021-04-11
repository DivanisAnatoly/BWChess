using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Parser
{
    public GameObject Name { get; set; }
    public GameObject SquareFromMove { get; set; }
    public GameObject SquareToMove { get; set; }

    public Parser(string chessMove)
    {
        this.Name = GameObjects.TryGetObject(chessMove.Substring(0, 1));
        this.SquareFromMove = GameObject.Find(chessMove.Substring(1, 2));
        this.SquareToMove = GameObject.Find(chessMove.Substring(3));

    }

    public void ParseFigureMove(string chessMove) //Функция автоматического перемещения фигур
    {
        Board board = new Board();
        if (SquareFromMove == null || SquareToMove == null || Name == null || 
            SquareFromMove.transform.position != Name.transform.position || SquareFromMove == SquareToMove) return;
        if (board.CheckTryCutFigure(SquareToMove, Name))  //не И, или
        {
            Name.transform.position = SquareToMove.transform.position;
            Debug.Log(chessMove);
        }


    }   

}
