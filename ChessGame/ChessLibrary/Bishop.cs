using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Bishop : Piece
    {
        internal Bishop(char key, Color pieceColor) : base(key, pieceColor)
        {
        }


        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            CanMoveDiagonal(avaibleSquares, desk.deskSquares, ownSquare);
            return avaibleSquares;
        }


    }
}
