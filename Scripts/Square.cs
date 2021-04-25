using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{

    //Подсветка клеток
    public void HighlightSquare(GameObject clickedItem, List<Parser> parser)
    {
        Constraints constraints = new Constraints();
        foreach (Parser i in parser)
        {
            Transform itemToMove = Clicks.GetItemAt(i.SquareToMove.transform.position);
            if (constraints.CheckSquare(clickedItem.transform.position) == constraints.CheckSquare(i.SquareFromMove.transform.position) &&
                clickedItem.name == i.Name.name)
            {
                if (itemToMove && constraints.CheckEnemyFigure(itemToMove, clickedItem))
                {
                    PlaceAMSquare(i.SquareToMove, "Attack");
                    i.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                    i.SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }

        }
        return;
    }

    //Изменение цвета клетки
    public void ReverseColorSquare(GameObject goSquare, List<Parser> parser, out TypesOfMove typeMove)
    {
        typeMove = TypesOfMove.Null;
        Color colorSquare = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < parser.Count; i++)
        {
            Debug.Log("" + parser[i].Name.name + "" + parser[i].SquareToMove.name);
            if (goSquare == parser[i].SquareToMove && goSquare.GetComponent<SpriteRenderer>().color == colorSquare)
            {

                typeMove = TypesOfMove.Normal;
                if ((parser[i].Name.name[0] == 'P' && parser[i].SquareToMove.name[1] == '8') ||
                    parser[i].Name.name[0] == 'p' && parser[i].SquareToMove.name[1] == '1')
                    typeMove = TypesOfMove.Transform;
                else if (parser[i].Name.name == "Ke1" && parser[i].SquareToMove.name == "g1")
                {
                    typeMove = TypesOfMove.SCastling;
                }
                else if (parser[i].Name.name == "Ke1" && parser[i].SquareToMove.name == "c1")
                {
                    typeMove = TypesOfMove.LCastling;
                }
            }
            PlaceAMSquare(parser[i].SquareToMove, "Movement");
            parser[i].SquareToMove.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
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
