using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Pawn : Piece
    {
        int stepY;
        public Pawn(char key, Color pieceColor) : base(key, pieceColor)
        {   
            stepY = pieceColor == Color.white ? 1 : -1;
        }

        public override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            bool CanJump;
            //пешки не могут ходить на 1-ой и 8-ой горизонтали (края доски)
            if (ownSquare.y < 1 || ownSquare.y > 6)
            {
                for (int y = 7; y >= 0; y--)
                    for (int x = 0; x < 8; x++)
                        avaibleSquares[x, y] = Square.none;

                return avaibleSquares;
            }

            //пешки ходят на два хода только со своей начальной горизонтали(2-я для белыхых, 7-я для черных)
            if (ownSquare.y == 1 || ownSquare.y == 6)
                CanJump = true;
            else
                CanJump = false;

            //очень сложный просчет доступных ходов 
            foreach (Square square in avaibleSquares)
            {
                if (square != Square.none)
                {
                    if(ownSquare.DeltaY(square) == stepY && ownSquare.AbsDeltaX(square)<=1)
                    {
                        if(ownSquare.AbsDeltaX(square) == 1 && (desk.deskSquares[square.x, square.y].ownedPiece == null && String.Compare(desk.notation.EnPassant,square.Name)!=0))
                            avaibleSquares[square.x, square.y] = Square.none;
                    
                        if (ownSquare.AbsDeltaX(square) == 0 && desk.deskSquares[square.x, square.y].ownedPiece != null)
                            avaibleSquares[square.x, square.y] = Square.none;   
                    }
                    else 
                    {
                        if (ownSquare.DeltaY(square) == 2 * stepY && CanJump && avaibleSquares[ownSquare.x, ownSquare.y + 1] != Square.none && ownSquare.AbsDeltaX(square) == 0 && desk.deskSquares[square.x, square.y].ownedPiece == null)
                            continue;
                        else
                            avaibleSquares[square.x, square.y] = Square.none;
                    }
                }
            }

            return avaibleSquares;

        }
    }
}
