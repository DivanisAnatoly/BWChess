using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessLibrary
{
    class Game
    {

        public Color inGameColor { get; set; }
        public int moveNumber { get; private set; }
        public ForsythEdwardsNotation notation { get; private set; }
        public Desk desk;
        public ChessPlayer player1;
        public ChessPlayer player2;

        //Начать партию
        public void StartGame(string fen)
        {
            notation = JsonConvert.DeserializeObject<ForsythEdwardsNotation>(fen);
            inGameColor = notation.InGameColor;
            moveNumber = notation.MoveNumber;
            desk = new Desk(notation);

            player1 = new Gamer(Color.white);
            player2 = new Bot(Color.black);
        }

        //Сохранить состояние игры(сохр. в формате ФЭН)
        public void ParseGameToFEN()
        {
            Square sqBK = desk.FindKing(Color.black);
            Square sqWK = desk.FindKing(Color.white);
            King bk = (King)sqBK.ownedPiece;
            King wk = (King)sqWK.ownedPiece;

            notation.PiecePosition = FenPieces();
            notation.InGameColor = inGameColor;
            notation.Castling = bk.CastlingKey + wk.CastlingKey;
            //notation.EnPassant = false;
            notation.HalfMoveClock = 0;
            notation.MoveNumber = moveNumber;
        }

        //Подготовка к след. ходу
        internal void PrepareNextMove()
        {
            moveNumber++;
            inGameColor = inGameColor.FlipColor();
            ParseGameToFEN();
        }

        //Получить положение фигур представленное в строке
        string FenPieces()
        {
            Piece[,] pieces = desk.pieces;
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    sb.Append(pieces[x, y] == null ? '1' : pieces[x, y].key);
                if (y > 0) sb.Append('/');
            }
            string eight = "11111111";
            for (int j = 8; j >= 2; j--)
                sb.Replace(eight.Substring(0, j), j.ToString());
            return sb.ToString();
        }
    }
}
