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

        public Bot(Color playerColor) : base(playerColor)
        {
        }

        internal override void MakeMove(string move, Desk desk)
        {
            string botMove = allAvaibleMoves[random.Next(allAvaibleMoves.Count)];

            System.Threading.Thread.Sleep(2000);
            desk.UpdatePiecesOnDesk(botMove, playerColor);
        }
    }
}
