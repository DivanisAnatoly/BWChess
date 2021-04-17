using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PieceMoves
{
    Board board = new Board();
    static List<string> ProbableMoves = new List<string>() { "Pa2a4", "Pa2a3", "Nb1c3", "Nb1a3", "Pb2b4", "Nc3a4","pa4a3", "Ph7h8"};
    ChessPiece transformPawn = new ChessPiece();
    string newPawn;

    enum State
    {
        none,
        drop,
        transform
    }


    State state;
    GameObject item;

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
                if (IsMouseButtonPressed())
                    PickUp();
                break;
            case State.drop:
                if (IsMouseButtonPressed())
                {
                    Drop();
                    return false;
                }
                break;
            case State.transform:
                if (IsMouseButtonPressed())
                {
                    newPawn += transformPawn.TransformPawn(item);
                    Debug.Log(newPawn);
                    state = State.none;
                    item = null;
                    return false;
                }
                break;

        }
        return false;
    }

    static public bool IsMouseButtonPressed()
    {
        return Input.GetMouseButtonDown(0);
    }

    void PickUp()
    {
        Vector2 clickPosition = GetClickPosition();
        Transform clickedItem = Board.GetItemAt(clickPosition);
        if (clickedItem == null) return;
        Vector2 coordsSquare = Board.CheckSquare(GetClickPosition());   //Можно удалить, существует только ради вывода имени клетки
        HighlightSquare(clickedItem);
        item = clickedItem.gameObject;
        item.transform.localScale = new Vector3(17, 17, 0f);  //Попробовать использовать clickedItem, улучшить увеличение
        state = State.drop;
        Debug.Log("pickedUp " + item.name + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1));

    }

    static public Vector2 GetClickPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Drop()
    {
        Vector2 coordsSquare = Board.CheckSquare(GetClickPosition());   //Можно удалить, существует только ради вывода имени клетки
        Vector2 coordsItem = Board.CheckSquare(item.transform.position);   //А здесь использовать метод GetItemAt
        GameObject goSquare = GameObjects.TryGetObject("" + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1));
        int check = ReverseColorSquare(goSquare);
        if (check == 1 || check == 11)
            if (goSquare && (coordsItem != coordsSquare) && board.CheckTryCutFigure(goSquare, item))
            {
                item.transform.position = goSquare.transform.position;
                Debug.Log("Drop " + item.name + (char)(coordsItem.x + 'a') + "" + (coordsItem.y + 1)
                              + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1));
            }
        item.transform.localScale = new Vector3(14, 14, 0f);
        if (check == 11)
        {
            newPawn = item.name + (char)(coordsItem.x + 'a') + "" + (coordsItem.y + 1)
                      + (char)(coordsSquare.x + 'a') + (coordsSquare.y + 1);
            state = State.transform;
        }
        else
        {
            state = State.none;
            item = null;
        }
    }

    void HighlightSquare(Transform clickedItem)
    {
        List<Parser> parser = GetParseListForMoves();

        for (int i = 0; i < ProbableMoves.Count; i++)
        {
            Transform itemToMove = Board.GetItemAt(parser[i].SquareToMove.transform.position);
            if (Board.CheckSquare(clickedItem.position) == Board.CheckSquare(parser[i].SquareFromMove.transform.position) && 
                clickedItem.name.ToString() == parser[i].Name.name.ToString())
            {
                if (itemToMove && board.CheckEnemyFigure(itemToMove, clickedItem.gameObject))
                {
                    board.PlaceAMSquare(parser[i].SquareToMove, "Attack");
                    parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        
        }
        return;
    }

    int ReverseColorSquare(GameObject goSquare)
    {
        int check = 0;
        List<Parser> parser = GetParseListForMoves();
        Color colorSquare = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < ProbableMoves.Count; i++)
        {
            if (goSquare == parser[i].SquareToMove && goSquare.GetComponent<SpriteRenderer>().color == colorSquare)
            {
                check = 1;
                if ((parser[i].Name.name == 'P'.ToString() && parser[i].SquareToMove.name[1] == '8') ||
                    parser[i].Name.name == 'p'.ToString() && parser[i].SquareToMove.name[1] == '1')
                    check = 11;
            }
            board.PlaceAMSquare(parser[i].SquareToMove, "Movement");
            parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        return check;
    }

    List<Parser> GetParseListForMoves()
    {
        List<Parser> parser = new List<Parser>(ProbableMoves.Count);
        for (int i = 0; i < ProbableMoves.Count; i++)
        {
            parser.Add(new Parser(ProbableMoves[i]));

        }
        return parser;

    }


}



