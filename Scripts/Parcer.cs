using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Parser
{
    public GameObject Name { get; set; }
    public GameObject SquareFromMove { get; set; }
    public GameObject SquareToMove { get; set; }

    //Парсер хода пешки
    public Parser(string chessMove)
    {
        this.Name = GameObject.Find(chessMove.Substring(0, 3));
        this.SquareFromMove = GameObject.Find(chessMove.Substring(1, 2));
        this.SquareToMove = GameObject.Find(chessMove.Substring(3));
    }
}
