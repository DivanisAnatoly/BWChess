using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Pawn : Piece
    {
        readonly int stepY;
        

        internal Pawn(PiecesKeys pieceKey, Color pieceColor) : base(pieceKey, pieceColor)
        {
            stepY = pieceColor == Color.white ? 1 : -1;
        }


        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
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
            CanJump = (ownSquare.y == 1 || ownSquare.y == 6);

            //очень сложный просчет доступных ходов 
            foreach (Square square in avaibleSquares)
            {
                if (square == ownSquare) { avaibleSquares[square.x, square.y] = Square.none; continue; }

                if (ownSquare.DeltaY(square) == stepY)
                {
                    if (ownSquare.AbsDeltaX(square) == 0)
                    {
                        if (desk.deskSquares[square.x, square.y].ownedPiece != Piece.nullPiece)
                        {
                            avaibleSquares[square.x, square.y] = Square.none;
                            movesVector.occupiedSquares.Add(square.Name);
                        }
                        continue;
                    }
                    if (ownSquare.AbsDeltaX(square) == 1)
                    {
                        if (desk.notation.EnPassant == square.Name) continue;
                        if (desk.deskSquares[square.x, square.y].ownedPiece == Piece.nullPiece || desk.deskSquares[square.x, square.y].ownedPiece.pieceColor == pieceColor)
                        {
                            avaibleSquares[square.x, square.y] = Square.none;
                            movesVector.occupiedSquares.Add(square.Name);
                        }
                        continue;
                    }
                    avaibleSquares[square.x, square.y] = Square.none;
                    continue;
                }
                if (CanJump && ownSquare.AbsDeltaX(square) == 0 && ownSquare.DeltaY(square) == stepY * 2)
                {
                    if (desk.deskSquares[square.x, square.y].ownedPiece != Piece.nullPiece || avaibleSquares[square.x, square.y - stepY] == Square.none)
                    {
                        movesVector.occupiedSquares.Add(square.Name);
                        avaibleSquares[square.x, square.y] = Square.none;
                    }
                    continue;
                }

                avaibleSquares[square.x, square.y] = Square.none;
            }

            return avaibleSquares;
        }


    }
}
