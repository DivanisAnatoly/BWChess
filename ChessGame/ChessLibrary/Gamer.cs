using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Gamer : ChessPlayer
    {
        public Gamer(Color playerColor, Moves playersMoves, Desk desk) : base(playerColor, playersMoves, desk) { }


        internal override void MakeMove(string move, Desk desk)
        {
            desk.UpdatePiecesOnDesk(move, playerColor);
        }


    }
}
