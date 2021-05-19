using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{

    //Подсветка клеток
    public void HighlightSquare(List<Parser> parser)
    {
        GameObject objectOnTheWay;
        foreach (Parser currentMove in parser)
        {
            objectOnTheWay = ChessGameControl.dictionaryOfFigures[currentMove.SquareToMove.name];  //Объект, который может существовать
            if (objectOnTheWay)
            {
                PlaceAMSquare(currentMove.SquareToMove, ChessGameControl.attack);
                currentMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
            else
                currentMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        return;
    }

    //Изменение цвета клетки в исходное состояние и установка прозрачности клеток
    public bool ReverseColorSquare(GameObject clickedSquare, List<Parser> parser)
    {
        bool checkVar = false;
        if (clickedSquare && clickedSquare.GetComponent<SpriteRenderer>().color == new Color(1f, 1f, 1f, 1f)) 
            checkVar = true;
        foreach (Parser currentMove in parser)
        {   
            PlaceAMSquare(currentMove.SquareToMove, ChessGameControl.movement);
            currentMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        return checkVar;
    }

    public void LightUpTrackSquare(Parser chessMove)
    {
        PlaceAMSquare(chessMove.SquareToMove, ChessGameControl.track);
        PlaceAMSquare(chessMove.SquareFromMove, ChessGameControl.track);
        chessMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        chessMove.SquareFromMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        return;
    }

    public void ReverseLightUpTrackSquare(Parser chessMove)
    {
        PlaceAMSquare(chessMove.SquareToMove, ChessGameControl.movement);
        PlaceAMSquare(chessMove.SquareFromMove, ChessGameControl.movement);
        chessMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        chessMove.SquareFromMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        return;
    }

    //Меняет спрайт указанной клетки на указанный спрайт, обычно используется для замены Attack/Movement спрайта
    static public void PlaceAMSquare(GameObject square, GameObject spriteName)
    {
        GameObject Sprite = spriteName;
        square.GetComponent<SpriteRenderer>().sprite = Sprite.GetComponent<SpriteRenderer>().sprite;
    }
}
