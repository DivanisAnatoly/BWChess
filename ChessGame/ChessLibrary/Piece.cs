using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    using static PiecesKeys;
    abstract class Piece
    {
        protected Vectors movesVector;

        internal readonly Color pieceColor;
        internal readonly PiecesKeys pieceKey;
        internal static Piece nullPiece = new NullPiece(none, Color.none);

        private class NullPiece : Piece
        {
            internal NullPiece(PiecesKeys pieceKey, Color pieceColor) : base(pieceKey, pieceColor) { }
            internal override Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare) => throw new NotImplementedException();
        }


        internal Piece(PiecesKeys pieceKey, Color pieceColor)
        {
            this.pieceKey = pieceKey;
            this.pieceColor = pieceColor;
        }


        //просчет всех возможных ходов фигуры
        internal Vectors GetPieceMoves(Desk desk, Square ownSquare)
        {
            Square[,] avaibleSquares = new Square[8, 8];
            movesVector = new Vectors { startPosition = ownSquare.Name, vectorPieceKey = pieceKey };

            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    avaibleSquares[x, y] = new Square(x, y);

            avaibleSquares = CanFigureMove(avaibleSquares, desk, ownSquare);

            //добавляем ходы в список
            foreach (Square sq in avaibleSquares)
                if (sq != Square.none)
                {
                    string sqName = sq.Name;
                    if ((pieceKey == whitePawn || pieceKey == blackPawn) && (sqName.Contains('1') || sqName.Contains('8')))//превращение пешки
                    {
                        string pawnTransfMoves = (pieceColor == Color.white)
                            ? sqName + (char)whiteQueen + " " + sqName + (char)whiteKnight + " " + sqName + (char)whiteRook + " " + sqName + (char)whiteBishop
                            : sqName + (char)blackQueen + " " + sqName + (char)blackKnight + " " + sqName + (char)blackRook + " " + sqName + (char)blackBishop;
                        movesVector.avaibleSquares.AddRange(pawnTransfMoves.Split());
                    }
                    else//обычные ходы
                        movesVector.avaibleSquares.Add(sqName);
                }

            return movesVector;
        }


        //обнуляет клетки недоступные фигуре для хода 
        internal abstract Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare);


        //для ладьи и ферзя
        protected void CanMoveStraight(Square[,] avaibleSquares, Square[,] deskSquares, Square ownSquare)
        {
            foreach (Square square in avaibleSquares)
            {
                if (square == ownSquare) { avaibleSquares[square.x, square.y] = Square.none; continue; }

                if (!(ownSquare.AbsDeltaX(square) == 0 || ownSquare.AbsDeltaY(square) == 0))
                    avaibleSquares[square.x, square.y] = Square.none;
            }

            bool blocked = false;
            for (int x = ownSquare.x + 1; x < 8; x++)
            {
                Square sq = deskSquares[x, ownSquare.y];
                CheckSquare(avaibleSquares, sq, x, ownSquare.y, ref blocked);
            }

            blocked = false;
            for (int x = ownSquare.x - 1; x >= 0; x--)
            {
                Square sq = deskSquares[x, ownSquare.y];
                CheckSquare(avaibleSquares, sq, x, ownSquare.y, ref blocked);
            }

            blocked = false;
            for (int y = ownSquare.y + 1; y < 8; y++)
            {
                Square sq = deskSquares[ownSquare.x, y];
                CheckSquare(avaibleSquares, sq, ownSquare.x, y, ref blocked);
            }

            blocked = false;
            for (int y = ownSquare.y - 1; y >= 0; y--)
            {
                Square sq = deskSquares[ownSquare.x, y];
                CheckSquare(avaibleSquares, sq, ownSquare.x, y, ref blocked);
            }
        }


        //вспомогательный метод для CanMoveStraight() и CanMoveDiagonal()
        private void CheckSquare(Square[,] avaibleSquares, Square square, int x, int y, ref bool blocked)
        {
            if (blocked) { avaibleSquares[x, y] = Square.none; return; }

            if (square.ownedPiece.pieceColor == pieceColor)
            {
                avaibleSquares[x, y] = Square.none;
                movesVector.occupiedSquares.Add(square.Name);
            }
            blocked = true;
        }


        //для слона и ферзя
        protected void CanMoveDiagonal(Square[,] avaibleSquares, Square[,] deskSquares, Square ownSquare)
        {
            foreach (Square square in avaibleSquares)
            {
                if (square == ownSquare) { avaibleSquares[square.x, square.y] = Square.none; continue; }
                if (!(ownSquare.AbsDeltaX(square) == ownSquare.AbsDeltaY(square)))
                    avaibleSquares[square.x, square.y] = Square.none;
            }

            bool blocked = false;
            int i = 1;
            while ((ownSquare.x + i <= 7) && (ownSquare.y + i <= 7))
            {
                Square sq = deskSquares[ownSquare.x + i, ownSquare.y + i];
                CheckSquare(avaibleSquares, sq, ownSquare.x + i, ownSquare.y + i, ref blocked);
                i++;
            }

            blocked = false; i = 1;
            while ((ownSquare.x + i <= 7) && (ownSquare.y - i >= 0))
            {
                Square sq = deskSquares[ownSquare.x + i, ownSquare.y - i];
                CheckSquare(avaibleSquares, sq, ownSquare.x + i, ownSquare.y - i, ref blocked);
                i++;
            }

            blocked = false; i = 1;
            while ((ownSquare.x - i >= 0) && (ownSquare.y + i <= 7))
            {
                Square sq = deskSquares[ownSquare.x - i, ownSquare.y + i];
                CheckSquare(avaibleSquares, sq, ownSquare.x - i, ownSquare.y + i, ref blocked);
                i++;
            }

            blocked = false; i = 1;
            while ((ownSquare.x - i >= 0) && (ownSquare.y - i >= 0))
            {
                Square sq = deskSquares[ownSquare.x - i, ownSquare.y - i];
                CheckSquare(avaibleSquares, sq, ownSquare.x - i, ownSquare.y - i, ref blocked);
                i++;
            }
        }




    }
}
