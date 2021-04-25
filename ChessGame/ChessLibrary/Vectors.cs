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
        internal PiecesKeys vectorPieceKey;
        internal List<string> avaibleSquares = new List<string>();
        internal List<string> occupiedSquares = new List<string>();

        internal int StartPositionX { get { return (startPosition[0] - 'a'); } }
        internal int StartPositionY { get { return (startPosition[1] - '1'); } }

        //получить ходы из вектора в виде списка строк
        internal List<string> GetMoves()
        {
            List<string> vectorPieceMoves = new List<string>();
            foreach (string squareName in avaibleSquares)
            {
                if (squareName != "0-0-0" && squareName != " 0-0 ")
                    vectorPieceMoves.Add((char)vectorPieceKey + startPosition + squareName);
                else
                    vectorPieceMoves.Add(squareName);
            }
            return vectorPieceMoves;
        }
    }
}
