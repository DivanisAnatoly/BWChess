using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GameObjects
{

    static public GameObject CornerDownOnSquare()
    {
        return GameObject.Find("CornerDownOnSquare");
    }

    static public GameObject LeftBottomCornerDownOnBoard()
    {
        return GameObject.Find("LeftBottomCornerDownOnBoard");
    }

    static public GameObject RightBottomCornerDownOnBoard()
    {
        return GameObject.Find("RightBottomCornerDownOnBoard");
    }

    static public GameObject LeftBottomCornerUpOnBoard()
    {
        return GameObject.Find("LeftBottomCornerUpOnBoard");
    }

    static public double SquareSize()
    {
        return CornerDownOnSquare().transform.position.x - LeftBottomCornerDownOnBoard().transform.position.x;
    }

    static public GameObject CentreSquareOnBoard()
    {
        return GameObject.Find("CentreSquareOnBoard");
    }
}

