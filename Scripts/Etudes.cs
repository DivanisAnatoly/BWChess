using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Etudes : MonoBehaviour
{
    string filePath = @"Assets\Libraries\Fens.txt";

    private void Start()
    {
        
    }
    public void CatchFen(GameObject etude)
    {
        ChessGameControl.SetFen(readParticularFen(etude));
    }
    private string readParticularFen(GameObject etude)
    {
        using (StreamReader fileIn = new StreamReader(filePath, System.Text.Encoding.Default))
        {
            int i = 0;
            string fen;
            while ((fen = fileIn.ReadLine()) != null)
            {
                i++;
                if (etude.name == i.ToString())
                {
                    Debug.Log(etude.name + " --- " + fen);
                    return fen;
                }
            }
            fileIn.Close();
        }
        return "{ 'PiecePosition': 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR','InGameColor':'white','Castling': 'KQkq','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }";
    }
}
