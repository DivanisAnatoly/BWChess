using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{

    //Подсветка клеток
    public void HighlightSquare(GameObject clickedItem, List<Parser> parser)
    {
        Constraints constraints = new Constraints();
        for (int i = 0; i < parser.Count; i++)
        {
            Transform itemToMove = Clicks.GetItemAt(parser[i].SquareToMove.transform.position);
            if (constraints.CheckSquare(clickedItem.transform.position) == constraints.CheckSquare(parser[i].SquareFromMove.transform.position) &&
                clickedItem.name == parser[i].Name.name)
            {
                if (itemToMove && constraints.CheckEnemyFigure(itemToMove, clickedItem))
                {
                    PlaceAMSquare(parser[i].SquareToMove, "Attack");
                    parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                    parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }

        }
        return;
    }

    //Изменение цвета клетки
    public int ReverseColorSquare(GameObject goSquare, List<Parser> parser)
    {
        int check = 0;
        Color colorSquare = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < parser.Count; i++)
        {
            if (goSquare == parser[i].SquareToMove && goSquare.GetComponent<SpriteRenderer>().color == colorSquare)
            {
                check = 1;
                if ((parser[i].Name.name[0] == 'P' && parser[i].SquareToMove.name[1] == '8') ||
                    parser[i].Name.name[0] == 'p' && parser[i].SquareToMove.name[1] == '1')
                    check = 11;
            }
            PlaceAMSquare(parser[i].SquareToMove, "Movement");
            parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
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
