using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Transform goFigure;

    static public Transform GetItemAt(Vector3 position)
    {
        
        RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);
        if (figures.Length == 0 || !CheckClickFigureOnBoard(figures[0].transform))
            return null;
        return figures[0].transform;
    }

    static public Vector2 CheckSquare(Vector2 clickPosition)
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
        else newCoordsSquare = new Vector2(99999, 99999);
        return newCoordsSquare;
    }

    public bool CheckTryCutFigure(GameObject goSquare, GameObject item)
    {
        goFigure = GetItemAt(goSquare.transform.position);
        if (!goFigure) return true;
        GameObject fieldForDefeatFigures = GameObjects.TryGetObject("Square" + goFigure.name[0]);
        
        if ((goFigure.name[0] >= 'A' && goFigure.name[0] <= 'Z' && item.name[0] >= 'a' && item.name[0] <= 'z') ||
           (goFigure.name[0] >= 'a' && goFigure.name[0] <= 'z' && item.name[0] >= 'A' && item.name[0] <= 'Z'))
        {
            goFigure.transform.position = fieldForDefeatFigures.transform.position;
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

    static private bool CheckClickFigureOnBoard(Transform figure)
    {
        if (figure == null) return false;
        if (CheckSquare(figure.position).x == 99999 && CheckSquare(figure.position).y == 99999)
            return false;
        return true;
    }
}
