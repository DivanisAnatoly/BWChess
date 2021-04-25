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


        internal override void MakeMove(PieceMove move, Desk desk)
        {
            List<string> moves = playersMoves.GetPlayerMoves(playerColor);
            lastMove = new PieceMove(playerColor, moves[random.Next(moves.Count)]);

            desk.UpdatePiecesOnDesk(lastMove, playerColor);
        }


    }
}
