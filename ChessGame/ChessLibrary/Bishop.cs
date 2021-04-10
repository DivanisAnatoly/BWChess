using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Bishop : Piece
    {
        public Bishop(char key, Color pieceColor) : base(key, pieceColor)
        {
        }

        public override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            CanMoveDiagonal(avaibleSquares, desk.deskSquares, ownSquare);
            return avaibleSquares;
        }
    }
}
