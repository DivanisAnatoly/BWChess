using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFigure
{
    //Получить фигуру из окна возможных фигур
    public bool GetFigureFromTransformField(GameObject pawnFigure, out string nameTransformFigure)
    {
        Vector2 clickPosition = Clicks.GetClickPosition();
        RaycastHit2D[] figures = Physics2D.RaycastAll(clickPosition, clickPosition, 0.5f);
        if (figures.Length != 0 && figures[0].transform.gameObject.tag == "Static")
        {
            nameTransformFigure = TransformPawn(pawnFigure, figures[0].transform);
            return true;
        }
        nameTransformFigure = null;
        return false;
    }

    public string TransformPawn(GameObject pawn, Transform figureFromTransformField)
    {
        // ---------------------------------------
        // Здесь должно выезжать окно с фигурами
        // ---------------------------------------
        //FigureField figureField = new FigureField();        // Создание поля для выбора фигуры

        figureFromTransformField.position = pawn.transform.position;
        figureFromTransformField.name += pawn.name.Substring(1);
        pawn.transform.position = GameObject.Find("Square" + pawn.name).transform.position;
        pawn.tag = "Static";
        figureFromTransformField.tag = "Active";
        return figureFromTransformField.name.Substring(0, 1);

    }

    public Vector3 IncreaseFigure(Vector3 localScale)
    {
        return new Vector3(localScale.x + 3, localScale.y + 3, 0f);
    }

    public Vector3 DecreaseFigure(Vector3 localScale)
    {
        return new Vector3(localScale.x - 3, localScale.y - 3, 0f);
    }
}
