using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
    // Start is called before the first frame update
    PieceMoves pieceMoves = new PieceMoves();
  
    public static string fen = @"RNBQK111/PP111PPP/8/8/8/8/pppppppP/rnbqkbn1";

    void Start()
    {
        ShowFigures();
       // Parser parser; //Нужекн клик мышей
       // parser = new Parser("qd8d7");

    }

    private void Update()
    {
        pieceMoves.Action();
        
    }

    // Update is called once per frame
    void ShowFigures()
    {
        char figure;
        int countCreatedFigures = 0;
        for (int y = 7; y >= 0; y--)
            for (int x = 0; x < 8; x++)
            {
                
                figure = GetFigureAt(x, y);
                if (figure == '.') continue;
                PlaceFigure(figure.ToString(), x, y);
                countCreatedFigures++;
                Debug.Log(countCreatedFigures);
            }
    }

    void PlaceFigure(string figure, int x, int y)
    {
        Debug.Log(" " + figure + (char)(x + 'a') + (y));
        GameObject goFigure = GameObjects.TryGetObject(figure);
        GameObject goSquare = GameObjects.TryGetObject("" + (char)(x + 'a') + (y + 1));
        goFigure.transform.position = goSquare.transform.position;
        goFigure.tag = "Active";
    }

    
    char GetFigureAt(int x, int y)
    {
        
        for (int j = 8; j >= 2; j--)
            fen = fen.Replace(j.ToString(), (j - 1).ToString() + "1");
        fen = fen.Replace("1", ".");

        string[] linesBoard = fen.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        return linesBoard[y][x];
    }
}
