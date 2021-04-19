using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Night : Piece
    {
        internal Night(char key, Color pieceColor) : base(key, pieceColor) {}

        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            foreach (Square square in avaibleSquares)
            {
                if (square == ownSquare) { avaibleSquares[square.x, square.y] = Square.none; continue; }

                if (!((ownSquare.AbsDeltaX(square) == 1 && ownSquare.AbsDeltaY(square) == 2)
                    || (ownSquare.AbsDeltaX(square) == 2 && ownSquare.AbsDeltaY(square) == 1)) )
                    avaibleSquares[square.x, square.y] = Square.none;
                else
                {
                    if (desk.deskSquares[square.x, square.y].ownedPiece != null)
                        if (desk.deskSquares[square.x, square.y].ownedPiece.pieceColor == pieceColor)
                        {
                            avaibleSquares[square.x, square.y] = Square.none;
                            movesVector.occupiedSquares.Add(square.Name);
                        }
                }
            }

            return avaibleSquares;
        }
   
    }
}
