using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessLibrary;

public class TransformFigure
{
    //Получить фигуру из окна возможных фигур
    public bool GetFigureFromTransformField(GameObject pawnFigure, GameManager gameManager, out string nameTransformFigure)
    {
        GameObject choiceFigure = GameObject.Find(ChangeFiguresChoice.yourChoice);
        if (choiceFigure && choiceFigure.tag == "Static" && 
            Constraints.CheckColorFigure(choiceFigure, gameManager) != null)
        {
            GameObject newGameObject = GameObject.Instantiate(choiceFigure, choiceFigure.transform.position, choiceFigure.transform.rotation);
            nameTransformFigure = TransformPawn(pawnFigure, newGameObject);
            return true;
        }
        nameTransformFigure = null;
        return false;
    }

    public string TransformPawn(GameObject pawn, GameObject figureFromTransformField)
    {
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
