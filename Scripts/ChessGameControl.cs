using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ChessLibrary;
using Newtonsoft.Json;


public class ChessGameControl : MonoBehaviour
{
    private static string fen = @"{ 'PiecePosition': 'rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR','InGameColor':'white',
                                                'Castling': 'KQkq','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }";
    private PieceM pieceMoves;
    private GameManager gameManager;
    private PieceCreator pieceCreator;
    static public Dictionary<string, GameObject> dictionaryOfFigures;
    static public GameObject movement;
    static public GameObject attack;
    static public GameObject track;

    TypeOfGame typeOfGame;

    public ForsythEdwardsNotation Notation { get; private set; }

    void Awake()
    { 
        Notation = JsonConvert.DeserializeObject<ForsythEdwardsNotation>(fen);
        dictionaryOfFigures = new Dictionary<string, GameObject>();
        pieceCreator = new PieceCreator(Notation.PiecePosition);
        pieceCreator.ShowFigures();
        typeOfGame = TypeOfGame.PlayerVsBot;
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = new GameManager();
        movement = GameObject.Find("Movement");
        attack = GameObject.Find("Attack");
        track = GameObject.Find("Track");
        Debug.Log(track);
        StartNewGame();
    }

    private void StartNewGame()
    {
        gameManager.StartGame(fen, "white");
        typeOfGame = TypeOfGame.PlayerVsBot;
        pieceMoves = new PieceM(gameManager, typeOfGame, Notation);
    }

    // Update is called once per frame
    private void Update()
    {
        pieceMoves.ActionPlayerWithBot();
    }

    public static void SetFen(string _fen)
    {
        fen = _fen;
    }

}
