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
            GameObject gameObject = GameObject.Instantiate(figures[0].transform.gameObject, figures[0].transform.position, figures[0].transform.rotation);
            nameTransformFigure = TransformPawn(pawnFigure, gameObject);
            return true;
        }
        nameTransformFigure = null;
        return false;
    }

    public string TransformPawn(GameObject pawn, GameObject figureFromTransformField)
    {
        // ---------------------------------------
        // Здесь должно выезжать окно с фигурами
        // ---------------------------------------
        //FigureField figureField = new FigureField();        // Создание поля для выбора фигуры
        figureFromTransformField.transform.position = pawn.transform.position;
        ChessGameControl.dictionaryOfFigures[pawn.name.Substring(1)] = figureFromTransformField;
        figureFromTransformField.name = figureFromTransformField.name[0].ToString() + pawn.name.Substring(1);
      
        pawn.transform.position = GameObject.Find("Square" + pawn.name[0]).transform.position;
        pawn.tag = "Static";
        pawn.name = pawn.name[0].ToString();
        figureFromTransformField.tag = "Active";
        return figureFromTransformField.name[0].ToString();

    }

    //Увеличение размеров фигуры
    public void IncreaseFigure(GameObject currentFigure) 
    {
        currentFigure.transform.localScale = new Vector3(currentFigure.transform.localScale.x + 3,
                                                       currentFigure.transform.localScale.y + 3, 0f);
    }

    //Уменьшение размеров фигуры
    public void DecreaseFigure(GameObject currentFigure)
    {
        currentFigure.transform.localScale = new Vector3(currentFigure.transform.localScale.x - 3,
                                                       currentFigure.transform.localScale.y - 3, 0f);
    }
}
