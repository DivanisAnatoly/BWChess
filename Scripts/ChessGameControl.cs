using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChessLibrary;

public class ChessGameControl : MonoBehaviour
{
    PieceMoves pieceMoves;
    GameManager gameManager;
    string fen = @"{ 'PiecePosition': 'r111k11r/pppppppp/8/8/8/8/PPPPPPPP/R111K11RR','InGameColor':'white','Castling': 'KQkq','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }";
    string playerColor = "White";


    // Start is called before the first frame update
    private void Start()
    {
        gameManager = new GameManager();
        StartNewGame();
    }

    private void StartNewGame()
    {
        gameManager.StartGame(fen, playerColor);
        pieceMoves = new PieceMoves(gameManager);
    }

    // Update is called once per frame
    private void Update()
    {
        pieceMoves.Action();
    }

    
}
