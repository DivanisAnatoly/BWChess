using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public class Etudes : MonoBehaviour
{
    string filePath = @"Assets\Libraries\fens1.Json";

    List<string >etudes = new List<string>() {"{ 'PiecePosition': 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR','InGameColor':'white','Castling': 'KQkq','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': 'k1r5/p1r4R/QpPp1p2/1P1Bpp2/8/P7/2P2PP1/1K6', 'InGameColor':'white', 'Castling': '----', 'EnPassant': false, 'HalfMoveClock': 0, 'MoveNumber': 1 }",
                      "{ 'PiecePosition': '4r1k1/3R1ppp/6r1/p2P4/8/P1N3PP/3B1P1K/3Q2RB','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '2k5/1p1p1p2/3K4/8/6Q1/8/8/8','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '1kr5/1pp4Q/p1n5/q2r4/1b1p1NB1/1P1P2PP/PBPn4/K1R4R','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '1k4r1/pp3R2/3b4/3p4/Q1pPp2R/1PP1P3/PR6/1KN4Q','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '8/7Q/6p1/7p/6qk/8/7K/8','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '2rkqb1r/Q2np1pp/1p3p2/1BppN3/3Pb1qk/8/PPP2PPP/RN1K3R','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': 'r2q1bnr/pp1bk1pp/4p3/3pPp1B/3n4/6Q1/PPP2PPP/R1B1K2R','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': 'rb4rk/p4p1p/1q3p1B/2R2N2/3pb2Q/P5P1/1P2BP1P/6K1','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': 'r1bqrk2/pp2bppB/2pn3p/3pN2Q/3P1P2/2N5/PP4PP/R4RK1','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': 'r1bq1r1k/1pppNppp/p7/4R2Q/n7/8/PPP2PPP/R1B3K1','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '1k3bnr/1ppr1qpp/p4p2/QN2p3/4p1P1/4B2P/PPP2P2/1KRR4','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': 'r3nrk1/2p1Q1pp/p1b1Pp2/np4B1/6q1/1B3N2/PP3PPP/R3R1K1','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '2nqk1n1/1rp1p3/5b2/8/8/2B5/3P1PR1/1N1KQN2','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",
                      "{ 'PiecePosition': '4nkbq/4pppr/1R6/8/8/6r1/RPPP4/QBKN4','InGameColor':'white','Castling': '----','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }" };
    private void Start()
    {

    }
    public void CatchFen(GameObject etude)
    {
        ChessGameControl.SetFen(readParticularFen(etude));
    }
    private string readParticularFen(GameObject etude)
    {
        int i = 0;
        while (i < etudes.Count)
        {
            if (etude.name == i.ToString())
            {
                Debug.Log(etude.name + " --- " + etudes[i]);
                return etudes[i];
            }
            i++;
        }
        return etudes[0];
    }
}       

