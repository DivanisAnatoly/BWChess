using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    
    abstract class Piece
    {
        public Color pieceColor { get; private set; }
        public char key { get; private set; }


        public Piece(char key, Color pieceColor)
        {
            this.key = key;
            this.pieceColor = pieceColor;
        }

        //просчет всех возможных ходов фигуры
        public string getPieceMoves(Desk desk, Square ownSquare)
        {
            Square[,] avaibleSquares = new Square[8,8];

            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    avaibleSquares[x, y] = new Square(x, y);

            avaibleSquares = CanMoveTo(avaibleSquares, desk.deskSquares);
            avaibleSquares = CanFigureMove(avaibleSquares, desk, ownSquare);

            string result = "";
            //добавляем ходы в список
            foreach (Square square in avaibleSquares)
                if (square != Square.none)
                {
                    if ((this.key=='P' || this.key == 'p') && (square.Name.Contains('1') || square.Name.Contains('8')) )//превращение пешки
                    {
                        result += this.key + ownSquare.Name + square.Name + (pieceColor==Color.white ? "Q " : "q ");
                        result += this.key + ownSquare.Name + square.Name + (pieceColor == Color.white ? "N " : "n ");
                        result += this.key + ownSquare.Name + square.Name + (pieceColor == Color.white ? "R " : "r ");
                        result += this.key + ownSquare.Name + square.Name + (pieceColor == Color.white ? "B " : "b ");
                    }
                    else//обычные ходы
                    {
                        result += this.key + ownSquare.Name + square.Name + " ";
                    }
                }
                    
            if(result.Length>0) result = result.Remove(result.Length - 1);
            return result;
        }

        //отсеиваем ходы на клетки с фигурами своего цвета
        public Square[,] CanMoveTo(Square[,] avaibleSquares, Square[,] deskSquares)
        {
            foreach (Square square in deskSquares)
            {
                if (square.ownedPiece != null && square.ownedPiece.pieceColor == this.pieceColor)
                    avaibleSquares[square.x, square.y] = Square.none;
            }
            return avaibleSquares;
        }

        //из оставшихся клеток выбираем доступные для конкретной фигуры
        public abstract Square[,] CanFigureMove(Square[,] avaibleSquares, Desk desk, Square ownSquare);


        public void CanMoveStraight(Square[,] avaibleSquares, Square[,] deskSquares, Square ownSquare)
        {
            foreach (Square square in avaibleSquares)
            {
                if (square != Square.none && !(ownSquare.AbsDeltaX(square) == 0 || ownSquare.AbsDeltaY(square) == 0))
                    avaibleSquares[square.x, square.y] = Square.none;
            }
            bool blocked = false;
            for (int x = ownSquare.x + 1; x < 8; x++)
            {
                if (blocked) avaibleSquares[x, ownSquare.y] = Square.none;
                if (avaibleSquares[x, ownSquare.y] == Square.none) blocked = true;
                if (avaibleSquares[x, ownSquare.y] != Square.none && deskSquares[x, ownSquare.y].ownedPiece != null) blocked = true;

            }
            blocked = false;
            for (int x = ownSquare.x - 1; x >= 0; x--)
            {
                if (blocked) avaibleSquares[x, ownSquare.y] = Square.none;
                if (avaibleSquares[x, ownSquare.y] == Square.none) blocked = true;
                if (avaibleSquares[x, ownSquare.y] != Square.none && deskSquares[x, ownSquare.y].ownedPiece != null) blocked = true;
            }
            blocked = false;
            for (int y = ownSquare.y + 1; y < 8; y++)
            {
                if (blocked) avaibleSquares[ownSquare.x, y] = Square.none;
                if (avaibleSquares[ownSquare.x, y] == Square.none) blocked = true;
                if (avaibleSquares[ownSquare.x, y] != Square.none && deskSquares[ownSquare.x, y].ownedPiece != null) blocked = true;

            }
            blocked = false;
            for (int y = ownSquare.y - 1; y >= 0; y--)
            {
                if (blocked) avaibleSquares[ownSquare.x, y] = Square.none;
                if (avaibleSquares[ownSquare.x, y] == Square.none) blocked = true;
                if (avaibleSquares[ownSquare.x, y] != Square.none && deskSquares[ownSquare.x, y].ownedPiece != null) blocked = true;
            }
        }


        public void CanMoveDiagonal(Square[,] avaibleSquares, Square[,] deskSquares, Square ownSquare)
        {
            foreach (Square square in avaibleSquares)
            {
                if (square != Square.none && !(ownSquare.AbsDeltaX(square)  == ownSquare.AbsDeltaY(square)))
                    avaibleSquares[square.x, square.y] = Square.none;
            }

            bool blocked = false;
            int i = 1;
            while ((ownSquare.x + i <= 7) && (ownSquare.y + i <= 7))
            {
                if (blocked) avaibleSquares[ownSquare.x+i, ownSquare.y+i] = Square.none;
                if (avaibleSquares[ownSquare.x + i, ownSquare.y + i] == Square.none) blocked = true;
                if (avaibleSquares[ownSquare.x + i, ownSquare.y + i] != Square.none && deskSquares[ownSquare.x + i, ownSquare.y + i].ownedPiece != null) blocked = true;
                i++;
            }

            blocked = false; i = 1;
            while ((ownSquare.x + i <= 7) && (ownSquare.y - i >= 0))
            {
                if (blocked) avaibleSquares[ownSquare.x + i, ownSquare.y - i] = Square.none;
                if (avaibleSquares[ownSquare.x + i, ownSquare.y - i] == Square.none) blocked = true;
                if (avaibleSquares[ownSquare.x + i, ownSquare.y - i] != Square.none && deskSquares[ownSquare.x + i, ownSquare.y - i].ownedPiece != null) blocked = true;
                i++;
            }

            blocked = false; i = 1;
            while ((ownSquare.x - i >= 0) && (ownSquare.y + i <= 7))
            {
                if (blocked) avaibleSquares[ownSquare.x - i, ownSquare.y + i] = Square.none;
                if (avaibleSquares[ownSquare.x - i, ownSquare.y + i] == Square.none) blocked = true;
                if (avaibleSquares[ownSquare.x - i, ownSquare.y + i] != Square.none && deskSquares[ownSquare.x - i, ownSquare.y + i].ownedPiece != null) blocked = true;
                i++;
            }

            blocked = false; i = 1;
            while ((ownSquare.x - i >= 0) && (ownSquare.y - i >= 0))
            {
                if (blocked) avaibleSquares[ownSquare.x - i, ownSquare.y - i] = Square.none;
                if (avaibleSquares[ownSquare.x - i, ownSquare.y - i] == Square.none) blocked = true;
                if (avaibleSquares[ownSquare.x - i, ownSquare.y - i] != Square.none && deskSquares[ownSquare.x - i, ownSquare.y - i].ownedPiece != null) blocked = true;
                i++;
            }

        }
    }
}
