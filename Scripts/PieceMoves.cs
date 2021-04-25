using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChessLibrary;

public class PieceMoves
{
    private Constraints constraints;  //Ограничения области клика
    private TransformFigure transformFigure;  //Превращение пешки
    GameManager gameManager;

    List<Parser> parser;  //Список распарсенных ходов

    private Square square = new Square(); //Подсветка клеток
    private string newPawn;  //Вывод превращения пешки

    private State state;  //Состояние фигуры
    private GameObject currentFigure; //Кликнутая фигура

    public PieceMoves(GameManager gameManager)
    {
        constraints = new Constraints();
        transformFigure = new TransformFigure();
        this.gameManager = gameManager;
        parser = new List<Parser>();
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
                    if (gameManager.GetInGameColor() == "black") gameManager.BotMove();
                    GenerateFigureMove(new Parser(gameManager.GetLastMove()));
                }
                break;
            case State.transform:
                if (Clicks.IsMouseButtonPressed() && transformFigure.GetFigureFromTransformField(currentFigure, out string nameTransformFigure))
                {
                    newPawn += nameTransformFigure;
                    gameManager.PlayerMove(newPawn);
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
        parser = GetParseListForMoves(gameManager.GetAllAvaibleMoves(clickedItem.gameObject.name.Substring(1)));
        square.HighlightSquare(clickedItem.gameObject, parser);
        currentFigure = clickedItem.gameObject;
        currentFigure.transform.localScale = transformFigure.IncreaseFigure(currentFigure.transform.localScale); 
        state = State.drop;
        Debug.Log("pickedUp " + currentFigure.name);

    }

    //По повторному клику мыши фигура падает на доску
    void Drop()
    {
        Vector2 coordsClickSquare = constraints.CheckSquare(Clicks.GetClickPosition());
        Vector2 coordsCurrentItem = constraints.CheckSquare(currentFigure.transform.position);   
        GameObject toMoveSquare = GameObject.Find("" + (char)(coordsClickSquare.x + 'a') + (coordsClickSquare.y + 1));
        square.ReverseColorSquare(toMoveSquare, parser, out TypesOfMove typeMove);
        Debug.Log(typeMove);
        if (typeMove != TypesOfMove.Null)
            if (toMoveSquare && (coordsCurrentItem != coordsClickSquare) && constraints.CheckTryCutFigure(toMoveSquare, currentFigure))
            {
                currentFigure.transform.position = toMoveSquare.transform.position;
                currentFigure.name = currentFigure.name[0] + toMoveSquare.name;
                Debug.Log("Drop " + currentFigure.name[0] + (char)(coordsCurrentItem.x + 'a') + "" + (coordsCurrentItem.y + 1)
                              + (char)(coordsClickSquare.x + 'a') + (coordsClickSquare.y + 1));
            }
        newPawn = currentFigure.name[0] + (char)(coordsCurrentItem.x + 'a') + "" + (coordsCurrentItem.y + 1)
                      + (char)(coordsClickSquare.x + 'a') + (coordsClickSquare.y + 1);
        currentFigure.transform.localScale = transformFigure.DecreaseFigure(currentFigure.transform.localScale);
        TypeMove(typeMove, coordsCurrentItem, coordsClickSquare);
    }

    //Функция автоматического перемещения фигур
    public void GenerateFigureMove(Parser chessMove) 
    {
        Constraints constraints = new Constraints();
        if (chessMove.SquareFromMove == null || chessMove.SquareToMove == null || chessMove.Name == null ||
             chessMove.SquareFromMove == chessMove.SquareToMove) return;
        if (constraints.CheckTryCutFigure(chessMove.SquareToMove, chessMove.Name))  //не И, или
        {
            chessMove.Name.transform.position = chessMove.SquareToMove.transform.position;
            chessMove.Name.name = chessMove.Name.name[0] + chessMove.SquareToMove.name;
        }
    }

    void TypeMove(TypesOfMove typeMove, Vector2 coordsCurrentItem, Vector2 coordsClickSquare)
    {
        if (typeMove == TypesOfMove.SCastling)
        {
            Debug.Log(" 0-0 ");
            GenerateFigureMove(new Parser("Rh1f1"));
            gameManager.PlayerMove(" 0-0 ");
        }
        else if (typeMove == TypesOfMove.LCastling)
        {
            Debug.Log("0-0-0");
            GenerateFigureMove(new Parser("Ra1d1"));
            gameManager.PlayerMove("0-0-0");
        }
        if (typeMove == TypesOfMove.Transform)
        {
            state = State.transform;
            Debug.Log("Кликните по нужной фигуре");
        }
        else
        {
            gameManager.PlayerMove("" + currentFigure.name[0] + (char)(coordsCurrentItem.x + 'a') + "" + (coordsCurrentItem.y + 1)
                              + (char)(coordsClickSquare.x + 'a') + (coordsClickSquare.y + 1));
            state = State.none;
            currentFigure = null;
        }
    }

    //Получение ходов из будущей библиотеки
    private List<Parser> GetParseListForMoves(List<string> ProbableMoves)
    {
        foreach (string probableMoves in ProbableMoves)
        {
            parser.Add(new Parser(probableMoves));
        }
        return parser;
    }
}