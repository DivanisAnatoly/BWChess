using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{

    //Подсветка клеток
    public void HighlightSquare(GameObject clickedItem, List<Parser> parser)
    {
        Constraints constraints = new Constraints();
        foreach (Parser currentMove in parser)
        {
            Transform itemToMove = Clicks.GetItemAt(currentMove.SquareToMove.transform.position);  //Объект, который может существовать
            if (clickedItem.name == currentMove.Name.name)
            {
                if (itemToMove && constraints.CheckEnemyFigure(itemToMove, clickedItem))
                {
                    PlaceAMSquare(currentMove.SquareToMove, "Attack");
                    currentMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                    currentMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }

        }
        return;
    }

    //Изменение цвета клетки
    public void ReverseColorSquare(GameObject toMoveSquare, List<Parser> parser, out TypesOfMove typeMove)
    {
        typeMove = TypesOfMove.Null;
        Color colorSquare = new Color(1f, 1f, 1f, 1f);
        foreach (Parser currentMove in parser)
        {
            if (toMoveSquare == currentMove.SquareToMove && toMoveSquare.GetComponent<SpriteRenderer>().color == colorSquare)
            {
                typeMove = TypesOfMove.Normal;
                if ((currentMove.Name.name[0] == 'P' && currentMove.SquareToMove.name[1] == '8') ||
                    currentMove.Name.name[0] == 'p' && currentMove.SquareToMove.name[1] == '1')
                    typeMove = TypesOfMove.Transform;
                else if (currentMove.Name.name == "Ke1" && currentMove.SquareToMove.name == "g1" || 
                    currentMove.Name.name == "ke8" && currentMove.SquareToMove.name == "g8")
                {
                    typeMove = TypesOfMove.SCastling;
                }
                else if (currentMove.Name.name == "Ke1" && currentMove.SquareToMove.name == "c1" || 
                    currentMove.Name.name == "ke8" && currentMove.SquareToMove.name == "c8")
                {
                    typeMove = TypesOfMove.LCastling;
                }
            }
            PlaceAMSquare(currentMove.SquareToMove, "Movement");
            currentMove.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        return;
    }

    //Меняет спрайт указанной клетки на указанный спрайт, обычно используется для замены Attack/Movement спрайта
    static public void PlaceAMSquare(GameObject goSquare, string spriteName)
    {
        GameObject Sprite = GameObject.Find(spriteName);
        goSquare.GetComponent<SpriteRenderer>().sprite = Sprite.GetComponent<SpriteRenderer>().sprite;
    }
}
