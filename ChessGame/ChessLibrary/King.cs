using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class King : Piece
    {
        public bool longCastling = false;
        public bool shortCastling = false;

        public string CastlingKey { get { return (shortCastling ? pieceKey : '-').ToString() + ((longCastling ? (pieceColor == Color.white ? 'Q' : 'q') : '-')).ToString(); } }


        internal King(char key, Color pieceColor, string castling) : base(key, pieceColor)
        {
            if (pieceColor == Color.black)
            {
                if (castling.Substring(2, 1) == "k") shortCastling = true;
                if (castling.Substring(3, 1) == "q") longCastling = true;
            }
            if (pieceColor == Color.white)
            {
                if (castling.Substring(0, 1) == "K") shortCastling = true;
                if (castling.Substring(1, 1) == "Q") longCastling = true;
            }
        }


        internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare)
        {
            foreach (Square square in avaibleSquares)
            {
                if (square == ownSquare) { avaibleSquares[square.x, square.y] = Square.none; continue; }

                if (ownSquare.AbsDeltaX(square) > 1 || ownSquare.AbsDeltaY(square) > 1)
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


        //Проверки на отсутствие фигур между королем и ладьями(для длинной рокировки)
        public bool IsLongCastlingPossibleNow(Square[,] deskSquares, Square oS)
        {
            if (deskSquares[oS.x - 1, oS.y].ownedPiece != null
                || deskSquares[oS.x - 2, oS.y].ownedPiece != null
                || deskSquares[oS.x - 3, oS.y].ownedPiece != null) return false;
            return true;
        }


        //Проверки на отсутствие фигур между королем и ладьями(для короткой рокировки)
        public bool IsShortCastlingPossibleNow(Square[,] deskSquares, Square oS)
        {
            if (deskSquares[oS.x + 1, oS.y].ownedPiece != null
                || deskSquares[oS.x + 2, oS.y].ownedPiece != null) return false;
            return true;
        }


    }
}
