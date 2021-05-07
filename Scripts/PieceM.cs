using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChessLibrary;
using Newtonsoft.Json;

public class PieceM
{
    private Constraints constraints;                                //Ограничения области клика
    private TransformFigure transformFigure;                        //Превращение пешки
    private GameManager gameManager;
    ForsythEdwardsNotation notation;

    private List<Parser> Parser { get; set; }                       //Список распарсенных ходов

    private Square square = new Square();                           //Подсветка клеток
    private string newPawn;                                         //Вывод превращения пешки

    private StateMove stateMove;                                    //Состояние хода
    private StateAction stateAction;                                //Кто ходит?
    private GameObject currentFigure;                               //Кликнутая фигура
    private TeamColor teamColor;
    private TypeOfGame typeOfGame;


    public PieceM(GameManager gameManager, TypeOfGame typeOfGame, ForsythEdwardsNotation notation)
    {
        constraints = new Constraints();
        transformFigure = new TransformFigure();
        this.gameManager = gameManager;
        this.typeOfGame = typeOfGame;
        stateMove = StateMove.pick;
        stateAction = StateAction.movePlayer; //Кто первый ходит?
        teamColor = notation.InGameColor;
        currentFigure = null;
     
    }

    public void ActionPlayerWithBot()
    {
        if (stateAction == StateAction.movePlayer)
        {
            switch (stateMove)
            {
                case StateMove.pick:
                    if (Clicks.IsMouseButtonPressed())
                    {
                        PickUpFigure();
                    }
                    break;

                case StateMove.drop:
                    if (Clicks.IsMouseButtonPressed())
                    {
                        DropFigure();
                    }
                    break;

                case StateMove.transform:
                    SetTransformField();  //Делает активным нужное поле
                    if (ChangeFiguresChoice.yourChoice != null)
                    {
                        TransformPiece(currentFigure);                        
                    }
                    break;
            }
        }
        else if (stateAction == StateAction.moveBot)
        {
            BotMove();
        }
        else Debug.Log(gameManager.GameState());
        return;
    }

    private void PickUpFigure()
    {
        Parser = new List<Parser>();
        Clicks.GetItemAt(Clicks.GetClickPosition(), out GameObject clickedObject);              //Попытка получить кликнутый объект
        if (!CheckObjectOnProbablyMove(clickedObject)) return;                                  //Проверка, можно ли данной фигурой ходить?
        currentFigure = clickedObject;                                                          //Присвоение глобальной переменной
        GetParseListForMoves(gameManager.GetAllAvaibleMoves(currentFigure.name.Substring(1)));  //Парсинг ходов кликнутой фигуры
        string list = null;
        foreach (string i in gameManager.GetAllAvaibleMoves(currentFigure.name.Substring(1)))
            list += " " + i;
        Debug.Log(list);
        Debug.Log(gameManager.GetGameFen());
        transformFigure.IncreaseFigure(currentFigure);                                          //Увеличение кликнутой фигуры
        square.HighlightSquare(currentFigure, Parser);                                          //Подсветка возможных ходов
        stateMove = StateMove.drop;                                                             //Изменить состояние фигуры в состояние падения
        Debug.Log("pickedUp " + currentFigure.name);
    }

    private void DropFigure()
    {
        constraints.GetClickSquare(Clicks.GetClickPosition(), out GameObject clickedSquare);                            //Получение клетки, по которой совершён клик
        transformFigure.DecreaseFigure(currentFigure);                                                                  //Уменьшение кликнутой фигуры
        if (!square.ReverseColorSquare(clickedSquare, Parser)) clickedSquare = null;
        if (!CheckSquareOnGenerateMove(clickedSquare)) return;                                  
        GenerateFigureMove(new Parser(currentFigure.name + clickedSquare.name, gameManager.GetInGameColor()));          //Генерация хода
        CheckStatusGame();                                                                                              //Проверка статуса игры
    }

    private void TransformPiece(GameObject pawnFigure)
    {
        if (transformFigure.GetFigureFromTransformField(pawnFigure, gameManager, out string nameTransformFigure))    
        {
            newPawn += nameTransformFigure;
            FlipMovePlayerVsBot(newPawn);
            Debug.Log(newPawn);
        }
        ChangeFiguresChoice.yourChoice = null;
        ChangeFiguresChoice.personalChoiceW.SetActive(false);
        ChangeFiguresChoice.personalChoiceB.SetActive(false);
    }

    //Ход бота
    private void BotMove()
    {
        if (gameManager.GameState() == "MATE\nYOU WIN!")
        {
            stateAction = StateAction.endGame;
            return;
        }
        gameManager.BotMove();
        Debug.Log("Бот сделал ход " + gameManager.GetLastMove());
        GenerateFigureMove(new Parser(gameManager.GetLastMove(), gameManager.GetOpponentColor()));
        if (gameManager.GameState() == "MATE\nYOU LOSE!")
        {
            stateAction = StateAction.endGame;
            return;
        }
    }

    private void SetTransformField()
    {
        if (gameManager.GetInGameColor() == "white") ChangeFiguresChoice.personalChoiceW.SetActive(true);
        else ChangeFiguresChoice.personalChoiceB.SetActive(true);
    }

    private bool CheckObjectOnProbablyMove(GameObject clickedObject)
    {
        if (clickedObject != null)
            if (Constraints.CheckColorFigure(clickedObject, gameManager) != null)
                return true;
        return false;
    }

    private bool CheckSquareOnGenerateMove(GameObject clickedSquare)
    {
        if (clickedSquare == null)
        {
            stateMove = StateMove.pick;
            currentFigure = null;
            return false;
        }
        return true;
    }

    //Получение ходов из библиотеки
    private void GetParseListForMoves(List<string> ProbableMoves)
    {
        foreach (string probableMoves in ProbableMoves)
        {
            Parser.Add(new Parser(probableMoves, gameManager.GetMyColor()));
        }
        return;
    }

    //Метод автоматического перемещения фигур
    private void GenerateFigureMove(Parser chessMove)
    {
        Debug.Log($"Генерация {chessMove.chessmove}");
        //Проверка, на кликнутой клетке есть ли фигура и что с ней делать?
        if (constraints.CheckTryCutFigure(ChessGameControl.dictionaryOfFigures[chessMove.SquareToMove.name], chessMove.Name))  
        {
            chessMove.Name.transform.position = chessMove.SquareToMove.transform.position;
            ChessGameControl.dictionaryOfFigures[chessMove.SquareFromMove.name] = null;
            chessMove.Name.name = chessMove.Name.name[0] + chessMove.SquareToMove.name;
            ChessGameControl.dictionaryOfFigures[chessMove.SquareToMove.name] = chessMove.Name;
            if (CheckTransformMove(chessMove)) return;
            TryGenerateEnPassan(chessMove);                                          //Нужно доделать
            if (TryGenerateCastlingMove(chessMove.Name.name[0].ToString() + chessMove.SquareFromMove.name + chessMove.SquareToMove.name))
            {
                stateMove = StateMove.pick;
                FlipMovePlayerVsBot(chessMove.chessmove);
                return;
            }  
            FlipMovePlayerVsBot(chessMove.chessmove);                       //Сюда добавлять другие возможные партии
        }
        else                                                                //Если совершён клик по союзной фигуре
        {
            stateAction = StateAction.movePlayer;
            stateMove = StateMove.pick;
            currentFigure = null;
            return;
        }
    }

    private bool TryGenerateCastlingMove(string chessMove)
    {
        if (chessMove == "ke8g8")
        {
            stateMove = StateMove.Castling;
            GenerateFigureMove(new Parser("rh8f8", "black"));
            return true;
        }
        else if (chessMove == "ke8c8")
        {
            stateMove = StateMove.Castling;
            GenerateFigureMove(new Parser("ra8d8", "black"));
            return true;
        } 
        else if (chessMove == "Ke1g1")
        {
            stateMove = StateMove.Castling;
            GenerateFigureMove(new Parser("Rh1f1", "white"));;
            return true;
        }  
        else if (chessMove == "Ke1c1")
        {
            stateMove = StateMove.Castling;
            GenerateFigureMove(new Parser("Ra1d1", "white"));
            return true;
        }
        return false;
    }

    private void TryGenerateEnPassan(Parser chessMove)
    {
        notation = JsonConvert.DeserializeObject<ForsythEdwardsNotation>(gameManager.GetGameFen());   //Бот ходит цветом опонента
        if (notation.EnPassant == "-") return;
        string squarePawnForKilledW = notation.EnPassant[0].ToString() + (char)(notation.EnPassant[1] + 1);
        string squarePawnForKilledB = notation.EnPassant[0].ToString() + (char)(notation.EnPassant[1] - 1);
        Debug.Log(notation.InGameColor);
        if (chessMove.Name.name[0] == 'P' && chessMove.SquareToMove.name == notation.EnPassant && notation.InGameColor == TeamColor.white)
        {   
            constraints.MovingFigureOnDefeat(ChessGameControl.dictionaryOfFigures[squarePawnForKilledB]);
            ChessGameControl.dictionaryOfFigures[squarePawnForKilledB] = null;
            Debug.Log("Пешка уничтожена");
        }
        else if  (chessMove.Name.name[0] == 'p' && chessMove.SquareToMove.name == notation.EnPassant && notation.InGameColor == TeamColor.black)
        {
            constraints.MovingFigureOnDefeat(ChessGameControl.dictionaryOfFigures[squarePawnForKilledW]);
            ChessGameControl.dictionaryOfFigures[squarePawnForKilledB] = null;
            Debug.Log("Пешка уничтожена");
        }
    }

    private bool CheckTransformMove(Parser chessMove)
    {
        if (stateAction == StateAction.movePlayer && (chessMove.Name.name[0] == 'P' && chessMove.SquareToMove.name[1] == '8'
            || chessMove.Name.name[0] == 'p' && chessMove.SquareToMove.name[1] == '1'))
        {
            stateMove = StateMove.transform;
            newPawn = chessMove.chessmove;
            Debug.Log("Кликните по нужной фигуре");
            return true;
        }
        else if (chessMove.PossibilityTransform != '.')
        {
            GameObject spriteFigure = GameObject.Find(chessMove.PossibilityTransform.ToString());
            GameObject transformPawn = GameObject.Instantiate(spriteFigure, chessMove.Name.transform.position, chessMove.Name.transform.rotation);
            transformFigure.TransformPawn(chessMove.Name, transformPawn);
            FlipMovePlayerVsBot(chessMove.chessmove);
            return true;
        }
        return false;
    }

    private void CheckStatusGame()
    {
        Debug.Log($"состояние хода = {stateMove}, состояние партии = {gameManager.GameState()}");
        Debug.Log(gameManager.GetGameFen());
    }

    private void FlipMovePlayerVsBot(string chessMove)
    {
        if (typeOfGame == TypeOfGame.PlayerVsBot && stateAction == StateAction.movePlayer && stateMove != StateMove.Castling)
        {
            Debug.Log("FLIP");
            Debug.Log("Игрок сделал ход " + chessMove);
            gameManager.PlayerMove(chessMove);
            stateAction = StateAction.moveBot;
            stateMove = StateMove.pick;
            currentFigure = null;
        }
        else if (typeOfGame == TypeOfGame.PlayerVsBot && stateAction == StateAction.moveBot && stateMove != StateMove.Castling)
        {
            stateAction = StateAction.movePlayer;
            stateMove = StateMove.pick;
            currentFigure = null;
        }
    }
}
