using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameControl : MonoBehaviour
{
    private List<string> ProbableMoves;
    private List<Parser> parser;
    PieceMoves pieceMoves;

    // Start is called before the first frame update
    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        ProbableMoves = new List<string>() { "Pa2a3", "Pb2b7" };
        parser = new List<Parser>(ProbableMoves.Count);
        parser = GetParseListForMoves();
        pieceMoves = new PieceMoves(parser);
        pieceMoves.GenerateFigureMove(parser[1]);
    }

    // Update is called once per frame
    private void Update()
    {
        pieceMoves.Action();
    }

    //Получение ходов из будущей библиотеки
    private List<Parser> GetParseListForMoves()
    {
        for (int i = 0; i < ProbableMoves.Count; i++)
        {
            parser.Add(new Parser(ProbableMoves[i]));
        }
        return parser;
    }
}
