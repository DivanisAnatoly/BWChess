using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
    // Start is called before the first frame update
    PieceMoves pieceMoves = new PieceMoves();
  
    public static string fen = @"R1BQK11R/PPPP2PP/2N2P2/p3P3/4p3/3q4/1ppp1pp1/rnb1kbnr";

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
                PlaceFigure("box" + countCreatedFigures, figure.ToString(), x, y);
                countCreatedFigures++;
            }
    }

    void PlaceFigure(string box, string figure, int x, int y)
    {
        Debug.Log(box + " " + figure + (char)(x + 'a') + (y));
        GameObject goBox = GameObjects.TryGetObject(box);
        GameObject goFigure = GameObjects.TryGetObject(figure);
        GameObject goSquare = GameObjects.TryGetObject("" + (char)(x + 'a') + (y + 1));


        var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        var spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;
        goBox.transform.position = goSquare.transform.position;
        goBox.name = Convert.ToString(figure);
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
