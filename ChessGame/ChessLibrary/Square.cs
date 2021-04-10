using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Square
    {
        public static Square none = new Square(-1, -1);
        public int x { get; private set; }
        public int y { get; private set; }
        public Piece ownedPiece { get; private set; }
        public string Name { get { return ((char)('a' + x)).ToString() + (y + 1).ToString(); } }


        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public static bool operator == (Square a, Square b)
        {
            return a.x == b.x && a.y == b.y;
        }


        public static bool operator != (Square a, Square b)
        {
            return !(a == b);
        }


        public int DeltaX(Square square) { return square.x - this.x; }
        public int DeltaY(Square square) { return square.y - this.y; }

        public int AbsDeltaX(Square square) { return Math.Abs(DeltaX(square)); } 
        public int AbsDeltaY(Square square) { return Math.Abs(DeltaY(square)); } 

        public int SignX(Square square) { return Math.Sign(DeltaX(square)); } 
        public int SignY(Square square) { return Math.Sign(DeltaY(square)); } 


        public void SetPieceOnSquare(Piece piece)
        {
            ownedPiece = piece;
        }


    }
}
