using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessPiece 
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string TransformPawn(GameObject pawn)
    {
        Debug.Log("Кликните по нужной фигуре");
        Vector2 clickPosition;
        clickPosition = PieceMoves.GetClickPosition();
        RaycastHit2D[] figures = Physics2D.RaycastAll(clickPosition, clickPosition, 0.5f);
        if (figures.Length != 0)
        {
            GameObjects.TryGetObject(figures[0].transform.name).transform.position = pawn.transform.position;
            pawn.transform.position = GameObjects.TryGetObject("SquareP").transform.position;
            pawn.tag = "Static";
            figures[0].transform.gameObject.tag = "Active";
            return figures[0].transform.name;
        }
           
        return null;

    }
}
