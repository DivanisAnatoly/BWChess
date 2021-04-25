using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    using static PiecesKeys;
    class King : Piece
    {
        public bool longCastling = false;
        public bool shortCastling = false;

        //ключ рокировки
        public string CastlingKey
        {
            get
            {
                return (shortCastling ? (char)pieceKey : '-').ToString()
                    + ((longCastling ? (pieceColor == Color.white ? (char)whiteQueen : (char)blackQueen) : '-')).ToString();
            }
        }


        internal King(PiecesKeys pieceKey, Color pieceColor, string castling) : base(pieceKey, pieceColor)
        {
            if (pieceColor == Color.black)
            {
                if (castling[2] == (char)blackKing) shortCastling = true;
                if (castling[3] == (char)blackQueen) longCastling = true;
            }
            if (pieceColor == Color.white)
            {
                if (castling[0] == (char)whiteKing) shortCastling = true;
                if (castling[1] == (char)whiteQueen) longCastling = true;
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
        public bool CanMakeLongCastling(Square[,] deskSquares, Square ownsquare)
        {
            if(!longCastling) return false;
            if (deskSquares[ownsquare.x - 1, ownsquare.y].ownedPiece != nullPiece
                || deskSquares[ownsquare.x - 2, ownsquare.y].ownedPiece != nullPiece
                || deskSquares[ownsquare.x - 3, ownsquare.y].ownedPiece != nullPiece) return false;
            return true;
        }


        //Проверки на отсутствие фигур между королем и ладьями(для короткой рокировки)
        public bool CanMakeShortCastling(Square[,] deskSquares, Square ownsquare)
        {
            if (!shortCastling) return false;
            if (deskSquares[ownsquare.x + 1, ownsquare.y].ownedPiece != nullPiece
                || deskSquares[ownsquare.x + 2, ownsquare.y].ownedPiece != nullPiece) return false;
            return true;
        }


    }
}
