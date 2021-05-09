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
        bool CanJump;

        internal Pawn(PiecesKeys pieceKey, Color pieceColor) : base(pieceKey, pieceColor)
        {
            stepY = pieceColor == Color.white ? 1 : -1;
        }


        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {

            //пешки ходят на два хода только со своей начальной горизонтали(2-я для белыхых, 7-я для черных)
            CanJump = ((ownSquare.y == 1 && pieceKey == PiecesKeys.whitePawn) || (ownSquare.y == 6 && pieceKey == PiecesKeys.blackPawn));

            Piece pieceOnSquare;
            int absDeltaX, deltaY;


            foreach (Square square in avaibleSquares)
            {
                pieceOnSquare = desk.deskSquares[square.x, square.y].ownedPiece;
                absDeltaX = ownSquare.AbsDeltaX(square);
                deltaY = ownSquare.DeltaY(square);


                if (CanJump && absDeltaX == 0 && deltaY == 2 * stepY)
                {
                    if (desk.deskSquares[square.x, square.y - stepY].ownedPiece != Piece.nullPiece)
                    {
                        avaibleSquares[square.x, square.y] = Square.none;
                        continue;
                    }

                    if (pieceOnSquare != Piece.nullPiece)
                    {
                        movesVector.occupiedSquares.Add(square.Name);
                        avaibleSquares[square.x, square.y] = Square.none;
                        continue;
                    }

                    continue;
                }


                if (deltaY == stepY && absDeltaX <= 1)
                {
                    if (absDeltaX == 0)
                    {
                        if (pieceOnSquare == Piece.nullPiece) continue;

                        movesVector.occupiedSquares.Add(square.Name);
                        avaibleSquares[square.x, square.y] = Square.none;
                    }
                    else
                    {
                        if (desk.notation.EnPassant == square.Name && desk.notation.InGameColor == pieceColor) 
                        { 
                            movesVector.status = "recalculate"; 
                            continue; 
                        }
                        if (pieceOnSquare.pieceColor != pieceColor.FlipColor())
                        {
                            movesVector.occupiedSquares.Add(square.Name);////////////////////
                            avaibleSquares[square.x, square.y] = Square.none;
                        }
                    }

                    continue;
                }
                else
                    avaibleSquares[square.x, square.y] = Square.none;

            }

            return avaibleSquares;
        }


    }
}
