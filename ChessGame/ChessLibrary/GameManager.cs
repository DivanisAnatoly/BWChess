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
        ChessPlayer player1,player2;
        Moves moves;
        Desk desk;

        //Начать партию(начальные условия в json строке)
        public void StartGame(string fen = @"{ 'PiecePosition': 'rnbqkbnr/1ppppppp/8/pP1P1P1P/2B5/3B4/8/RNBQK2R','InGameColor':'white','Castling': 'KQkq','EnPassant': 'a6','HalfMoveClock': 0,'MoveNumber': 1 }", string playerColor="none")
        {
            Color color = Color.none;
            if (playerColor == "white" || playerColor == "White") color = Color.white;
            else if (playerColor == "black" || playerColor == "Black") color = Color.black;

            game.StartGame(fen,color);
            player1 = game.player1;
            player2 = game.player2;
            desk = game.desk;
            moves = game.moves;
        }


        //Получить ФЭН игры (json строка)
        public string GetGameFen()
        {
            game.ParseGameToFEN();
            return JsonConvert.SerializeObject(game.notation);
        }


        //Получить возможные ходы
        public List<string> GetAllAvaibleMoves(string pieceSquare="none")
        {
            if (pieceSquare == "none")
                return moves.GetPlayerMoves(player1.playerColor);
            else
                return moves.GetPieceMoves(player1.playerColor, pieceSquare);
        }


        //Ход игрока
        public void PlayerMove(string moveName)
        {
            PieceMove move = new PieceMove(player1.playerColor, moveName);
            List<string> playerMoves = moves.GetPlayerMoves(player1.playerColor);
            if (playerMoves.Count != 0 && player1.playerColor == game.inGameColor)
            {
                if (playerMoves.Exists(item => item == moveName))
                {
                    player1.MakeMove(move,desk);
                    game.PrepareNextMove(move);
                }
            }
        }


        //Ход бота
        public void BotMove()
        {
            List<string> opponentMoves = moves.GetPlayerMoves(player2.playerColor);
            if (opponentMoves.Count != 0 && player2.playerColor == game.inGameColor)
            {
                player2.MakeMove(new PieceMove(), desk);
                game.PrepareNextMove(player2.lastMove);
            }
        }


        //Получить цвет который ходит в данный момент
        public string GetInGameColor() { return game.inGameColor == Color.white ? "white" : "black"; }


        //Получить цвет игрока
        public string GetMyColor() { return player1.playerColor == Color.white ? "white" : "black"; }


        //Получить цвет оппонета
        public string GetOpponentColor() { return player2.playerColor == Color.white ? "white" : "black"; }


        //Получить фигуру по координатам доски
        public char GetFigureAt(int x, int y) {
            if ((x < 0) || (x > 7) || (y < 0) || (y > 7)) return 'E'; //E-ошибка (если координаты выходят за пределы доски)
            return desk.GetFigureAt(x, y); 
        }


        public char GetFigureAt(string pieceSquare)
        {
            int pX = pieceSquare[0] - 'a';
            int pY = pieceSquare[1] - '1'; 
            
            if(pieceSquare.Length!=2 || ((pX < 0) || (pX > 7) || (pY < 0) || (pY > 7))) return 'E'; //E-ошибка (если координаты выходят за пределы доски)
            return desk.GetFigureAt(pX, pY);
        }


        //Получить инфу о статусе партии
        public string GameState()
        {
            if(moves.GetPlayerMoves(game.inGameColor).Count == 0)
            {
                if (moves.IsPate()) return "PATE";
                if (moves.IsMate())
                {
                    if (game.inGameColor == player1.playerColor)
                        return "MATE\nYOU LOSE!";
                    else
                        return "MATE\nYOU WIN!";
                }
            }
            if(moves.IsCheck()) return "CHECK";
            return "GAME IN PROGRESS";
        }


        //получить последний сделаный ход
        public string GetLastMove()
        {
            if (moves.movesStory.Count != 0) return moves.movesStory.Last();
            else return "there is no last move";
        }


        //получить список клеток, на которых находятся фигуры с пересчитанными(невалидными) векторами
        public List<string> RecalculatedPieces() { return new List<string>(moves.RecalculatedPiecesPosition); }


    }
}
