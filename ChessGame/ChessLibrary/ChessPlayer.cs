using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    abstract class ChessPlayer
    {
        internal Color playerColor { get; private set; }
        internal Moves playersMoves;
        internal string lastMove;


        internal ChessPlayer(Color playerColor, Moves playersMoves, Desk desk)
        {
            this.playerColor = playerColor;
            this.playersMoves = playersMoves;
            CalculateMoves(desk);
        }


        //Просчитать доступные ходы для всех фигур (исключается просчет рокировки)
        internal void CalculateMoves(Desk desk)
        {
            List<Vectors> allPlayerMoves;
            allPlayerMoves = (playerColor == Color.white) ? playersMoves.whiteMoves : playersMoves.blackMoves;

            foreach (Square square in desk.deskSquares)
                if (square.ownedPiece.pieceColor == playerColor)
                    allPlayerMoves.Add(square.ownedPiece.GetPieceMoves(desk, square));
        }


        //Сделать ход
        internal abstract void MakeMove(string move, Desk desk);


    }
}
