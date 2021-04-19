using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Vectors
    {
        internal string startPosition;
        internal char vectorPieceKey;
        internal List<string> avaibleSquares = new List<string>();
        internal List<string> occupiedSquares = new List<string>();


        internal int StartPositionX { get { return (startPosition[0] - 'a'); } }
        internal int StartPositionY { get { return (startPosition[1] - '1'); } }


        internal List<string> GetMoves()
        {
            List<string> vectorPieceMoves = new List<string>();
            foreach (string square in avaibleSquares)
            {
                if (square != "0-0-0" && square != " 0-0 ")
                    vectorPieceMoves.Add(vectorPieceKey + startPosition + square);
                else
                    vectorPieceMoves.Add(square);
            }
            return vectorPieceMoves;
        }
    }
}
