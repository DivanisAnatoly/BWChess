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
                }
                break;
            case State.transform:
                if (Clicks.IsMouseButtonPressed() && transformFigure.GetFigureFromTransformField(currentFigure, out string nameTransformFigure))
                {
                    newPawn += nameTransformFigure;
                    gameManager.PlayerMove(newPawn);
                    Debug.Log(newPawn);
                    state = State.moveBot;
                    currentFigure = null;
                }
                break;
            case State.moveBot:
                {
                    if (gameManager.GetInGameColor() == "black")
                    {
                        gameManager.BotMove();
                        Parser botMove = new Parser(gameManager.GetLastMove(), "black");
                        GenerateFigureMove(new Parser(gameManager.GetLastMove(), "black"));
                        if (botMove.PossibilityTransform)
                        {
                            GameObject gameObject = GameObject.Find(gameManager.GetLastMove().Substring(5));
                            transformFigure.TransformPawn(botMove.Name, gameObject.transform);
                        } 
                    }
                    state = State.none;
                    break;
                }       
        }
        return false;
    }

    //Взятие фигуры по клику мыши
    void PickUp()
    {
        Vector2 clickPosition = Clicks.GetClickPosition();
        Transform clickedItem = Clicks.GetItemAt(clickPosition);
        if (clickedItem == null || !CheckColor(clickedItem)) { return; }
        parser = new List<Parser>();
        parser = GetParseListForMoves(gameManager.GetAllAvaibleMoves(clickedItem.gameObject.name.Substring(1)));
        if (parser == null) return;
        string list = null;
        foreach (Parser i in parser)  //Для отладки, можно в будущем удалить
        {
            Debug.Log(i);
            list += " " + i.Name.name[0] + i.SquareFromMove.name + i.SquareToMove.name;
        }
            
        Debug.Log("Ходы белого = " + list);

        square.HighlightSquare(clickedItem.gameObject, parser);
        currentFigure = clickedItem.gameObject;
        currentFigure.transform.localScale = transformFigure.IncreaseFigure(currentFigure.transform.localScale); 
        state = State.drop;
        Debug.Log("pickedUp " + currentFigure.name);
    }

    //По повторному клику мыши фигура падает на доску
    void Drop()
    {
        Vector2 coordsClickSquare = constraints.CheckSquare(Clicks.GetClickPosition());  //Координаты кликнутой клетки
        Vector2 coordsCurrentItem = constraints.CheckSquare(currentFigure.transform.position);   //Координаты текущей фигуры
        GameObject toMoveSquare = GameObject.Find("" + (char)(coordsClickSquare.x + 'a') + (coordsClickSquare.y + 1)); //Клетка, по которой сделан клик, объект
        square.ReverseColorSquare(toMoveSquare, parser, out TypesOfMove typeMove);
        if (typeMove != TypesOfMove.Null)
            if (toMoveSquare && (coordsCurrentItem != coordsClickSquare) && constraints.CheckTryCutFigure(toMoveSquare, currentFigure))
            {
                currentFigure.transform.position = toMoveSquare.transform.position;
                currentFigure.name = currentFigure.name[0] + toMoveSquare.name;
                Debug.Log("Drop " + currentFigure.name[0] + (char)(coordsCurrentItem.x + 'a') + "" + (coordsCurrentItem.y + 1)
                              + (char)(coordsClickSquare.x + 'a') + (coordsClickSquare.y + 1));
            }
        newPawn = "" + currentFigure.name[0] + (char)(coordsCurrentItem.x + 'a') + "" + (coordsCurrentItem.y + 1)
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
        if (chessMove.Name.name == "ke8")
            if (chessMove.SquareToMove.name == "c8")
                GenerateFigureMove(new Parser("ra8d8", "black"));
            else if (chessMove.SquareToMove.name == "g8")
                GenerateFigureMove(new Parser("rh8f8", "black"));
    }

    void TypeMove(TypesOfMove typeMove, Vector2 coordsCurrentItem, Vector2 coordsClickSquare)
    {
        if (typeMove == TypesOfMove.Null)
        {
            state = State.none;
            currentFigure = null;
        }
        else if (typeMove == TypesOfMove.SCastling)
        {
            Debug.Log(" 0-0 ");
            if (gameManager.GetMyColor() == "white")
            {
                GenerateFigureMove(new Parser("Rh1f1", "white")); 
            }
            else if (gameManager.GetMyColor() == "black") GenerateFigureMove(new Parser("rh8f8", "black"));
            gameManager.PlayerMove(" 0-0 ");
            state = State.moveBot;
            currentFigure = null;
        }
        else if (typeMove == TypesOfMove.LCastling)
        {
            Debug.Log("0-0-0");
            if (gameManager.GetMyColor() == "white")
            {
                GenerateFigureMove(new Parser("Ra1d1", "white"));
            }
            else if (gameManager.GetMyColor() == "black") GenerateFigureMove(new Parser("ra8d8", "black"));
            gameManager.PlayerMove("0-0-0");
            state = State.moveBot;
            currentFigure = null;
        }
        else if (typeMove == TypesOfMove.Transform)
        {
            state = State.transform;
            Debug.Log("Кликните по нужной фигуре");
        }
        else if (typeMove == TypesOfMove.Normal)
        {
            gameManager.PlayerMove("" + currentFigure.name[0] + (char)(coordsCurrentItem.x + 'a') + "" + (coordsCurrentItem.y + 1)
                              + (char)(coordsClickSquare.x + 'a') + (coordsClickSquare.y + 1));
            state = State.moveBot;
            currentFigure = null;
        }
    }

    //Получение ходов из будущей библиотеки
    private List<Parser> GetParseListForMoves(List<string> ProbableMoves)
    {
        foreach (string probableMoves in ProbableMoves)
        {
            parser.Add(new Parser(probableMoves, gameManager.GetMyColor()));
        }
        return parser;
    }

    bool CheckColor(Transform ClickObject)
    {
        if ((ClickObject.name[0] >= 'A' && ClickObject.name[0] <= 'Z' && gameManager.GetMyColor() == "white") ||
           (ClickObject.name[0] >= 'a' && ClickObject.name[0] <= 'z' && gameManager.GetMyColor() == "black"))
           return true;
        return false;
    }
}