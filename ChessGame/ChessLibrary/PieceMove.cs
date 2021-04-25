using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class PieceMove
    {
        internal readonly string name;
        readonly Color inGameColor;

        internal string from { get; private set; } = "none";
        internal string to { get; private set; } = "none";
        

        internal int fromX { get { return from[0] - 'a';} }
        internal int fromY { get { return from[1] - '1'; } }
        internal int toX { get { return to[0] - 'a'; } }
        internal int toY { get { return to[1] - '1'; } }


        internal bool promotion { get; private set; } = false;
        internal char promotionCharKey { get; private set; }

        internal string castling { get; private set; } = "none";

        internal string fromK { get; private set; } = "none";
        internal string toK { get; private set; } = "none";
        internal string fromR { get; private set; } = "none";
        internal string toR { get; private set; } = "none";

        internal int fromKX { get { return fromK[0] - 'a'; } } 
        internal int fromRX { get { return fromR[0] - 'a'; } }
        internal int toKX { get { return toK[0] - 'a'; } }
        internal int toRX { get { return toR[0] - 'a'; } }
        internal int fromKY { get { return fromK[1] - '1'; } }
        internal int fromRY { get { return fromR[1] - '1'; } }
        internal int toKY { get { return toK[1] - '1'; } }
        internal int toRY { get { return toR[1] - '1'; } }

        internal PiecesKeys pieceKey { get; private set; } = PiecesKeys.none;


        internal PieceMove(Color inGameColor = Color.none, string move = "none")
        {
            this.name = move;
            this.inGameColor = inGameColor;
            if (move != "none") InitMove(move);
        }

        private void InitMove(string move)
        {
            if (move != "0-0-0" && move != " 0-0 ")
            {
                pieceKey = (PiecesKeys)move[0];
                from = move.Substring(1, 2);
                to = move.Substring(3, 2);
                if (name.Length == 6) { promotion = true; promotionCharKey = move[5]; }
            }
            else
            {
                if (move == "0-0-0" && inGameColor == Color.white)
                {
                    castling = "White-Long";
                    fromK = "e1";
                    toK = "c1"; fromR = "a1"; toR = "d1";
                }
                if (move == "0-0-0" && inGameColor == Color.black)
                {
                    castling = "Black-Long";
                    fromK = "e8";
                    toK = "c8"; fromR = "a8"; toR = "d8";
                }

                if (move == " 0-0 " && inGameColor == Color.white)
                {
                    castling = "White-Short";
                    fromK = "e1";
                    toK = "g1"; fromR = "h1"; toR = "f1";
                }
                if (move == " 0-0 " && inGameColor == Color.black)
                {
                    castling = "Black-Short";
                    fromK = "e8";
                    toK = "g8"; fromR = "h8"; toR = "f8";
                }
            }
        }


    }
}
