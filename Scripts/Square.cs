using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{

    //Подсветка клеток
    public void HighlightSquare(GameObject clickedFigure, List<Parser> parser)
    {
        Constraints constraints = new Constraints();
        GameObject objectOnTheWay;
        foreach (Parser currentMove in parser)
        {
            objectOnTheWay = ChessGameControl.dictionaryOfFigures[currentMove.SquareToMove.name];  //Объект, который может существовать
            if (objectOnTheWay && constraints.CheckEnemyFigure(objectOnTheWay, clickedFigure))
            {
                PlaceAMSquare(currentMove.SquareToMove, "Attack");
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
            PlaceAMSquare(currentMove.SquareToMove, "Movement");
            currentMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        Debug.Log(checkVar);
        return checkVar;
    }

    //Меняет спрайт указанной клетки на указанный спрайт, обычно используется для замены Attack/Movement спрайта
    static public void PlaceAMSquare(GameObject square, string spriteName)
    {
        GameObject Sprite = GameObject.Find(spriteName);
        square.GetComponent<SpriteRenderer>().sprite = Sprite.GetComponent<SpriteRenderer>().sprite;
    }
}
