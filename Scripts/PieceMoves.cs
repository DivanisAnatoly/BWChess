using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PieceMoves
{
    Constraints constraints = new Constraints();
    
    
    TransformFigure transformFigure = new TransformFigure();
    Square square = new Square();
    string newPawn;

    enum State
    {
        none,
        drop,
        transform
    }

    State state;
    GameObject item;
    Transform figureFromTranform;

    public PieceMoves()
    {
        state = State.none;
        item = null;
    }

    public bool Action()
    {
        switch (state)
        {
            case State.none:
                if (Clicks.IsMouseButtonPressed())
                    PickUp();
                break;
            case State.drop:
                if (Clicks.IsMouseButtonPressed())
                {
                    Drop();
                    return false;
                }
                break;
            case State.transform:
                
                if (Clicks.IsMouseButtonPressed() && (figureFromTranform = transformFigure.GetFigureFromTransformField()))
                {
                    newPawn += transformFigure.TransformPawn(item, figureFromTranform);
                    Debug.Log(newPawn);
                    state = State.none;
                    item = null;
                    return false;
                }
                break;

        }
        return false;
    }

    //Взятие фигуры по клику мыши
    void PickUp()
    {
        Vector2 clickPosition = Clicks.GetClickPosition();
        Transform clickedItem = Clicks.GetItemAt(clickPosition);
        if (clickedItem == null) return;
        Vector2 coordsSquare = Constraints.CheckSquare(clickPosition);   //Можно удалить, существует только ради вывода имени клетки
        square.HighlightSquare(clickedItem.gameObject);
        item = clickedItem.gameObject;
        item.transform.localScale = new Vector3(17, 17, 0f);  //Попробовать использовать clickedItem, улучшить увеличение
        state = State.drop;
        Debug.Log("pickedUp " + item.name);

    }

    //По повторному клику мыши фигура падает на доску
    void Drop()
    {
        Vector2 coordsSquare = Constraints.CheckSquare(Clicks.GetClickPosition());   //Можно удалить, существует только ради вывода имени клетки
        Vector2 coordsItem = Constraints.CheckSquare(item.transform.position);   //А здесь использовать метод GetItemAt
        GameObject goSquare = GameObject.Find("" + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1));
        int check = square.ReverseColorSquare(goSquare);
        if (check == 1 || check == 11)
            if (goSquare && (coordsItem != coordsSquare) && constraints.CheckTryCutFigure(goSquare, item))
            {
                item.transform.position = goSquare.transform.position;
                item.name = item.name + goSquare.name;
                Debug.Log("Drop " + item.name[0] + (char)(coordsItem.x + 'a') + "" + (coordsItem.y + 1)
                              + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1));
            }
        item.transform.localScale = new Vector3(14, 14, 0f);
        if (check == 11)
        {
            newPawn = item.name[0] + (char)(coordsItem.x + 'a') + "" + (coordsItem.y + 1)
                      + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1);
            state = State.transform;
            Debug.Log("Кликните по нужной фигуре");
        }
        else
        {
            state = State.none;
            item = null;
        }
    }

    public void GenerateFigureMove(Parser chessMove) //Функция автоматического перемещения фигур
    {
        Constraints constraints = new Constraints();

        if (chessMove.SquareFromMove == null || chessMove.SquareToMove == null || chessMove.Name == null ||
             chessMove.SquareFromMove == chessMove.SquareToMove) return;
        if (constraints.CheckTryCutFigure(chessMove.SquareToMove, chessMove.Name))  //не И, или
        {
            chessMove.Name.transform.position = chessMove.SquareToMove.transform.position;
            Debug.Log(chessMove);
        }
    }

}