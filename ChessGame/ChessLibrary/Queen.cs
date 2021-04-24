using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Queen : Piece
    {
        internal Queen(PiecesKeys pieceKey, Color pieceColor) : base(pieceKey, pieceColor) { }

        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            Square[,] avaibleSquaresS = new Square[8, 8];
            Square[,] avaibleSquaresD = new Square[8, 8];

            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                {
                    avaibleSquaresS[x, y] = new Square(x, y);
                    avaibleSquaresD[x, y] = new Square(x, y);
                }

            CanMoveStraight(avaibleSquaresS, desk.deskSquares, ownSquare);
            CanMoveDiagonal(avaibleSquaresD, desk.deskSquares, ownSquare);

            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                {
                    if (avaibleSquaresS[x, y] == Square.none && avaibleSquaresD[x, y] == Square.none)
                        avaibleSquares[x, y] = Square.none;
                }

            return avaibleSquares;
        }
    }
}
