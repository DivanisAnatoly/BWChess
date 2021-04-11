using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
    // Start is called before the first frame update
    PieceMoves pieceMoves = new PieceMoves();
  
    string fen = @"RNBQKBNR/PPPPPPPP/8/8/8/8/pppppppp/rnbqkbnr";

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
            string figure;
        int countCreatedFigures = 0;
        for (int y = 7; y >= 0; y--)
            for (int x = 0; x < 8; x++)
            {

                figure = GetFigureAt(x, y);
                if (figure == "8") 
                {
                    continue;
                }
                
                PlaceFigure("box" + countCreatedFigures, figure, x, y);
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

    
    string GetFigureAt(int x, int y)
    {
        string[] linesBoard = fen.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (x >= linesBoard[y].Length) return "8";
        return linesBoard[y][x].ToString();
    }

    
}
