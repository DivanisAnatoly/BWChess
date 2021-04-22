using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Rook : Piece
    {
        internal Rook(PiecesKeys pieceKey, Color pieceColor) : base(pieceKey, pieceColor) { }

        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            CanMoveStraight(avaibleSquares, desk.deskSquares, ownSquare);
            return avaibleSquares;
        }
    }
}
