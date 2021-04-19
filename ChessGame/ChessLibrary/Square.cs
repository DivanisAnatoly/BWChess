using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Square
    {
        internal static Square none = new Square(-1, -1);

        internal readonly int x;
        internal readonly int y;


        internal Piece ownedPiece { get; private set; }
        internal string Name { get { return ((char)('a' + x)).ToString() + (y + 1).ToString(); } }


        internal Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public static bool operator ==(Square a, Square b) { return a.x == b.x && a.y == b.y; }
        public static bool operator !=(Square a, Square b) { return !(a == b); }


        internal int DeltaX(Square square) { return square.x - this.x; }
        internal int DeltaY(Square square) { return square.y - this.y; }


        internal int AbsDeltaX(Square square) { return Math.Abs(DeltaX(square)); }
        internal int AbsDeltaY(Square square) { return Math.Abs(DeltaY(square)); }


        internal void SetPieceOnSquare(Piece piece)
        {
            ownedPiece = piece;
        }

    }
}
