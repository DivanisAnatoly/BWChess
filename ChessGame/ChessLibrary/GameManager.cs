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
        public void StartGame(string fen = @"{ 'PiecePosition': 'rnbqkbnr/Pppppppp/8/7Q/2B5/8/PPPPPPPP/RNBQK2R','InGameColor':'white','Castling': 'KQkq','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }")
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
        public List<string> GetAllAvaibleMoves(string pieceSquare="none")
        {
            if (pieceSquare == "none")
                return game.moves.GetPlayerMoves(game.inGameColor);
            else
                return game.moves.GetPieceMoves(game.inGameColor, pieceSquare);
        }


        //Ход игрока
        public void PlayerMove(string move)
        {
            List<string> playerMoves = GetAllAvaibleMoves();
            if (playerMoves.Count != 0 && game.player1.playerColor == game.inGameColor)
            {
                if (playerMoves.Exists(item => item.Contains(move)))
                {
                    game.player1.MakeMove(move,game.desk);
                    game.PrepareNextMove(move);
                }
            }
        }


        //Ход бота
        public void BotMove()
        {
            List<string> opponentMoves = GetAllAvaibleMoves();
            if (opponentMoves.Count != 0 && game.player2.playerColor == game.inGameColor)
            {
                game.player2.MakeMove("", game.desk);
                game.PrepareNextMove(game.player2.lastMove);
            }
        }


        //Получить цвет который ходит в данный момент
        public char GetInGameColor() { return game.inGameColor == Color.white ? 'w' : 'b'; }


        //Получить цвет игрока
        public char GetMyColor() { return game.player1.playerColor == Color.white ? 'w' : 'b'; }


        //Получить фигуру по координатам доски
        public char GetFigureAt(int x, int y) {
            if ((x < 0) || (x > 7) || (y < 0) || (y > 7)) return 'E'; //E-ошибка (если координаты выходят за пределы доски)
            return game.desk.GetFigureAt(x, y); 
        }


        public char GetFigureAt(string pieceSquare)
        {
            int pX = pieceSquare[0] - 'a';
            int pY = pieceSquare[1] - '1'; 
            
            if(pieceSquare.Length!=2 || ((pX < 0) || (pX > 7) || (pY < 0) || (pY > 7))) return 'E'; //E-ошибка (если координаты выходят за пределы доски)
            return game.desk.GetFigureAt(pX, pY);
        }
    
        
        public string GameState()
        {
            if(GetAllAvaibleMoves().Count == 0)
            {
                if (game.moves.IsPate()) return "PATE";
                if (game.moves.IsMate())
                {
                    if (game.inGameColor == game.player1.playerColor)
                        return "MATE\nYOU LOSE!";
                    else
                        return "MATE\nYOU WIN!";
                }
            }
            if(game.moves.IsCheck()) return "CHECK";
            return "GAME IN PROGRESS";
        }


    }
}
