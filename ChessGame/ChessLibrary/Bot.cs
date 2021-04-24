using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Bot : ChessPlayer
    {
        Random random = new Random();


        public Bot(Color playerColor, Moves playersMoves, Desk desk) : base(playerColor, playersMoves, desk) { }


        internal override void MakeMove(string move, Desk desk)
        {
            List<string> moves = playersMoves.GetPlayerMoves(playerColor);
            lastMove = moves[random.Next(moves.Count)];
            System.Threading.Thread.Sleep(2000);
            desk.UpdatePiecesOnDesk(lastMove, playerColor);
        }


    }
}
