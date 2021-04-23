using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{

    Constraints constraints = new Constraints();

    //Подсветка клеток
    public void HighlightSquare(GameObject clickedItem)
    {
        for (int i = 0; i < PieceCreator.ProbableMoves.Count; i++)
        {
            Transform itemToMove = Clicks.GetItemAt(PieceCreator.parser[i].SquareToMove.transform.position);
            if (Constraints.CheckSquare(clickedItem.transform.position) == Constraints.CheckSquare(PieceCreator.parser[i].SquareFromMove.transform.position) &&
                clickedItem.name == PieceCreator.parser[i].Name.name)
            {
                if (itemToMove && constraints.CheckEnemyFigure(itemToMove, clickedItem))
                {
                    PlaceAMSquare(PieceCreator.parser[i].SquareToMove, "Attack");
                    PieceCreator.parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                    PieceCreator.parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }

        }
        return;
    }

    //Изменение цвета клетки
    public int ReverseColorSquare(GameObject goSquare)
    {
        int check = 0;
        Color colorSquare = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < PieceCreator.ProbableMoves.Count; i++)
        {
            if (goSquare == PieceCreator.parser[i].SquareToMove && goSquare.GetComponent<SpriteRenderer>().color == colorSquare)
            {
                check = 1;
                if ((PieceCreator.parser[i].Name.name[0] == 'P' && PieceCreator.parser[i].SquareToMove.name[1] == '8') ||
                    PieceCreator.parser[i].Name.name[0] == 'p' && PieceCreator.parser[i].SquareToMove.name[1] == '1')
                    check = 11;
            }
            PlaceAMSquare(PieceCreator.parser[i].SquareToMove, "Movement");
            PieceCreator.parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
        return check;
    }

    //Меняет спрайт указанной клетки на указанный спрайт, обычно используется для замены Attack/Movement спрайта
    static public void PlaceAMSquare(GameObject goSquare, string spriteName)
    {
        GameObject Sprite = GameObject.Find(spriteName);
        goSquare.GetComponent<SpriteRenderer>().sprite = Sprite.GetComponent<SpriteRenderer>().sprite;
    }
}
