using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Rook : Piece
    {
        internal Rook(char key, Color pieceColor) : base(key, pieceColor) { }

        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            CanMoveStraight(avaibleSquares, desk.deskSquares, ownSquare);
            return avaibleSquares;
        }
    }
}
