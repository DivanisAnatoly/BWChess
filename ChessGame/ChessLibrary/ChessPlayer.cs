using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    abstract class ChessPlayer
    {
        public Color playerColor { get; private set; }
        public List<string> allAvaibleMoves;


        public ChessPlayer(Color playerColor)
        {
            this.playerColor = playerColor;
        }

        //Просчитать доступные ходы
        public void CalculateMoves(Desk desk)//Square[,] deskSquares
        {
            allAvaibleMoves = new List<string>();
            foreach (Square square in desk.deskSquares) {
                if (square.ownedPiece != null)
                    if (square.ownedPiece.pieceColor == playerColor)
                        this.allAvaibleMoves.AddRange((square.ownedPiece.getPieceMoves(desk, square)).Split());
            }
            allAvaibleMoves.RemoveAll(item => item == "");
        }

        //Сделать ход
        internal abstract void MakeMove(string move, Desk desk);
    }
}
