using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Parser
{
    public GameObject Name { get; set; }
    public GameObject SquareFromMove { get; set; }
    public GameObject SquareToMove { get; set; }
    public bool PossibilityTransform
    {
        get;
        set;
    }

    //Парсер хода пешки
    public Parser(string chessMove, string currentTeamColor)
    {
        if (chessMove == " 0-0 ")
        {
            if (currentTeamColor == "white")
            {
                chessMove = "Ke1g1";
            }
            else if (currentTeamColor == "black")
                chessMove = "ke8g8";
        }
        else if (chessMove == "0-0-0")
        {
            if (currentTeamColor == "white")
                chessMove = "Ke1c1";
            else if (currentTeamColor == "black")
                chessMove = "ke8c8";
        }
        if (chessMove.Substring(5) != "")
        {
            chessMove = chessMove.Remove(chessMove.Length - 1);
            PossibilityTransform = true;
        }
        else PossibilityTransform = false;
        this.Name = GameObject.Find(chessMove.Substring(0, 3));
        this.SquareFromMove = GameObject.Find(chessMove.Substring(1, 2));
        this.SquareToMove = GameObject.Find(chessMove.Substring(3));
    }
}
