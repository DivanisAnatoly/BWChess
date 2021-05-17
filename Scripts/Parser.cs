using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Parser
{
    public string chessmove { get; private set; }
    public GameObject Name { get; set; }
    public GameObject SquareFromMove { get; set; }
    public GameObject SquareToMove { get; set; }
    public char PossibilityTransform { get; set; }
       
    //Парсер хода пешки
    public Parser(string chessMove, string currentTeamColor)
    {
        chessmove = chessMove;
        if (chessMove == "Ke1g1" || chessMove == "ke8g8")
        {
            chessmove = " 0-0 ";
        }
        else if (chessMove == "Ke1c1" || chessMove == "ke8c8")
            chessMove = "0-0-0";
        if (chessMove == " 0-0 ")
        {
            if (currentTeamColor == "white")
            {
                chessMove = "Ke1g1";
            }
            else chessMove = "ke8g8";
        }
        else if (chessMove == "0-0-0")
        {
            if (currentTeamColor == "white")
                chessMove = "Ke1c1";
            else chessMove = "ke8c8";
        }
        if (chessMove.Substring(5) != "")
        {
            PossibilityTransform = chessMove[5];
            chessMove = chessMove.Remove(chessMove.Length - 1);
        }
        else PossibilityTransform = '.';
        this.Name = GameObject.Find(chessMove.Substring(0, 3));
        this.SquareFromMove = GameObject.Find(chessMove.Substring(1, 2));
        this.SquareToMove = GameObject.Find(chessMove.Substring(3));
    }
}
