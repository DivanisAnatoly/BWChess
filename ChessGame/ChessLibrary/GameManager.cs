using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessLibrary
{
    public class GameManager
    {
        Game game = new Game();

        //Начать партию(начальные условия в json строке)
        public void StartGame(string fen = @"{ 'PiecePosition': 'rnbqkbnr/Pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR','InGameColor':'white','Castling': 'KQkq','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }")
        {
            game.StartGame(fen);
        }

        //Получить ФЭН игры (json строка)
        public string getGameFen()
        {
            game.ParseGameToFEN();
            return JsonConvert.SerializeObject(game.notation);
        }

        //Просчитать возможные ходы
        private void GetAllMoves()
        {
            List<string> result = new List<string>();
            King k;
            Square ksq;

            if (game.player1.playerColor == game.inGameColor)
            {
                game.player1.CalculateMoves(game.desk);
                result = game.player1.allAvaibleMoves;
            }    
            else
            {
                game.player2.CalculateMoves(game.desk);
                result = game.player2.allAvaibleMoves;
            }
           //отсеить ходы приведущие к мату
            ksq = game.desk.FindKing(game.inGameColor);
            k = (King)ksq.ownedPiece;
            result.RemoveAll(item => IsMateAfterMove(item));

            //проверить возможность рокировки(добавить их в список ходов)
            if (k.shortCastling && !IsCheck() && k.IsShortCastlingPossibleNow(game.desk.deskSquares,ksq))
            {
                if (k.pieceColor == Color.black && !IsMateAfterMove("ke8f8") && !IsMateAfterMove("ke8g8")) result.Add(" 0-0 ");
                if (k.pieceColor == Color.white && !IsMateAfterMove("ke1f1") && !IsMateAfterMove("ke1g1")) result.Add(" 0-0 ");
            }

            if (k.longCastling && !IsCheck() && k.IsLongCastlingPossibleNow(game.desk.deskSquares, ksq))
            {
                if (k.pieceColor == Color.black && !IsMateAfterMove("ke8d8") && !IsMateAfterMove("ke8c8")) result.Add("0-0-0");
                if (k.pieceColor == Color.white && !IsMateAfterMove("ke1d1") && !IsMateAfterMove("ke1c1")) result.Add("0-0-0");
            }
        }

        //Вернуть строку с возможными ходами
        public List<string> GetAllAvaibleMoves()
        {
            GetAllMoves();
            List<string> userResult = new List<string>(); 
            if (game.player1.playerColor == game.inGameColor)
                userResult.AddRange(game.player1.allAvaibleMoves);
            else
                userResult.AddRange(game.player2.allAvaibleMoves);
            return userResult;
        }

        //Проверка хода на угрозу мата
        private bool IsMateAfterMove(string move) 
        {
            ForsythEdwardsNotation copyN = (ForsythEdwardsNotation)game.notation.Clone();
            game.ParseGameToFEN();
            Desk copyDesk = new Desk(copyN);
            ChessPlayer op;
            copyDesk.UpdatePiecesOnDesk(move, game.inGameColor);
            op = new Bot(game.inGameColor.FlipColor());
            op.CalculateMoves(copyDesk);
            return copyDesk.IsKingInDanger(op.allAvaibleMoves, game.inGameColor);
        }

        //Проверка на шах
        public bool IsCheck()
        {
            ForsythEdwardsNotation copyN = (ForsythEdwardsNotation)game.notation.Clone();
            game.ParseGameToFEN();
            Desk copyDesk = new Desk(copyN);
            ChessPlayer op;
            op = new Bot(game.inGameColor.FlipColor());
            op.CalculateMoves(copyDesk);
            return copyDesk.IsKingInDanger(op.allAvaibleMoves, game.inGameColor);
        }

        //Проверка на мат
        public bool IsMate()
        {
            if(GetAllAvaibleMoves().Count==0 && IsCheck())return true;
            return false;
        }

        //Проверка на пат
        public bool IsPate()
        {
            if (GetAllAvaibleMoves().Count == 0 && !IsCheck()) return true;
            return false;
        }

        //Ход игрока
        public void PlayerMove(string move)
        {
            GetAllMoves();
            if (game.player1.allAvaibleMoves.Count != 0 && game.player1.playerColor == game.inGameColor)
            {
                if (game.player1.allAvaibleMoves.Contains(move))
                {
                    game.player1.MakeMove(move,game.desk);
                    game.PrepareNextMove();
                }
            }
        }

        //Ход бота
        public void BotMove()
        {
            if (game.player2.allAvaibleMoves.Count != 0 && game.player2.playerColor == game.inGameColor)
            {
                GetAllMoves();
                game.player2.MakeMove("-------", game.desk);
                game.PrepareNextMove();
            }
        }

        //Получить цвет который ходит в данный момент
        public char GetInGameColor() { return game.inGameColor == Color.white ? 'w' : 'b'; }

        //Получить цвет игрока
        public char GetMyColor() { return game.player1.playerColor == Color.white ? 'w' : 'b'; }

        //Получить фигуру по координатам доски
        public char GetFigureAt(int x, int y) { return game.desk.GetFigureAt(x, y); }
    }
}
