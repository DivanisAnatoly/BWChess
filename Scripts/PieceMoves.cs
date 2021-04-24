using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PieceMoves
{
    private Constraints constraints;  //Ограничения области клика
    private TransformFigure transformFigure;  //Превращение пешки

    List<Parser> parser;  //Список распарсенных ходов

    private Square square = new Square(); //Подсветка клеток
    private string newPawn;  //Вывод превращения пешки

    private State state;  //Состояние фигуры
    private GameObject currentFigure; //Кликнутая фигура

    public PieceMoves(List<Parser> parser)
    {
        constraints = new Constraints();
        transformFigure = new TransformFigure();
        this.parser = parser;
        state = State.none;
        currentFigure = null;
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
                }
                break;
            case State.transform:
                if (Clicks.IsMouseButtonPressed() && transformFigure.GetFigureFromTransformField(currentFigure, out string nameTransformFigure))
                {
                    newPawn += nameTransformFigure;
                    Debug.Log(newPawn);
                    state = State.none;
                    currentFigure = null;
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
        Vector2 coordsSquare = constraints.CheckSquare(clickPosition);   //Можно удалить, существует только ради вывода имени клетки
        square.HighlightSquare(clickedItem.gameObject, parser);
        currentFigure = clickedItem.gameObject;
        currentFigure.transform.localScale = new Vector3(17, 17, 0f);  //Попробовать использовать clickedItem, улучшить увеличение
        state = State.drop;
        Debug.Log("pickedUp " + currentFigure.name);

    }

    //По повторному клику мыши фигура падает на доску
    void Drop()
    {
        Vector2 coordsSquare = constraints.CheckSquare(Clicks.GetClickPosition());   //Можно удалить, существует только ради вывода имени клетки
        Vector2 coordsItem = constraints.CheckSquare(currentFigure.transform.position);   //А здесь использовать метод GetItemAt
        GameObject goSquare = GameObject.Find("" + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1));
        int check = square.ReverseColorSquare(goSquare, parser);
        if (check == 1 || check == 11)
            if (goSquare && (coordsItem != coordsSquare) && constraints.CheckTryCutFigure(goSquare, currentFigure))
            {
                currentFigure.transform.position = goSquare.transform.position;
                currentFigure.name = currentFigure.name + goSquare.name;
                Debug.Log("Drop " + currentFigure.name[0] + (char)(coordsItem.x + 'a') + "" + (coordsItem.y + 1)
                              + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1));
            }
        currentFigure.transform.localScale = new Vector3(14, 14, 0f);
        if (check == 11)
        {
            newPawn = currentFigure.name[0] + (char)(coordsItem.x + 'a') + "" + (coordsItem.y + 1)
                      + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1);
            state = State.transform;
            Debug.Log("Кликните по нужной фигуре");
        }
        else
        {
            state = State.none;
            currentFigure = null;
        }
    }

    //Функция автоматического перемещения фигур
    public void GenerateFigureMove(Parser chessMove) 
    {
        Constraints constraints = new Constraints();
        Debug.Log(chessMove.Name.name);
        if (chessMove.SquareFromMove == null || chessMove.SquareToMove == null || chessMove.Name == null ||
             chessMove.SquareFromMove == chessMove.SquareToMove) return;
        if (constraints.CheckTryCutFigure(chessMove.SquareToMove, chessMove.Name))  //не И, или
        {
            chessMove.Name.transform.position = chessMove.SquareToMove.transform.position;
            Debug.Log(chessMove);
        }
    }

}