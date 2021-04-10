using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Night : Piece
    {
        public Night(char key, Color pieceColor) : base(key, pieceColor)
        {
        }

        public override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            foreach (Square square in avaibleSquares)
            {
                if (square != Square.none 
                    && !((ownSquare.AbsDeltaX(square) == 1 && ownSquare.AbsDeltaY(square) == 2)
                    || (ownSquare.AbsDeltaX(square) == 2 && ownSquare.AbsDeltaY(square) == 1)) )
                    avaibleSquares[square.x, square.y] = Square.none;
            }
            return avaibleSquares;
        }
    }
}
