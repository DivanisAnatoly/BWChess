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
        public void StartGame(string fen = @"{ 'PiecePosition': 'rnbqk2r/PppppppP/8/8/8/8/PPPPPPPP/RNBQK2R','InGameColor':'white','Castling': 'KQkq','EnPassant': false,'HalfMoveClock': 0,'MoveNumber': 1 }",string playerColor="none")
        {
            Color color = Color.none;
            if (playerColor == "white" || playerColor == "White") color = Color.white;
            else if (playerColor == "black" || playerColor == "Black") color = Color.black;

            game.StartGame(fen,color);
        }


        //Получить ФЭН игры (json строка)
        public string GetGameFen()
        {
            game.ParseGameToFEN();
            return JsonConvert.SerializeObject(game.notation);
        }


        //Просчитать возможные ходы
        public List<string> GetAllAvaibleMoves(string pieceSquare="none")
        {
            if (pieceSquare == "none")
                return game.moves.GetPlayerMoves(game.player1.playerColor);
            else
                return game.moves.GetPieceMoves(game.player1.playerColor, pieceSquare);
        }


        //Ход игрока
        public void PlayerMove(string move)
        {
            List<string> playerMoves = GetAllAvaibleMoves();
            if (playerMoves.Count != 0 && game.player1.playerColor == game.inGameColor)
            {
                if (playerMoves.Exists(item => item == move))
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
        public string GetInGameColor() { return game.inGameColor == Color.white ? "white" : "black"; }


        //Получить цвет игрока
        public string GetMyColor() { return game.player1.playerColor == Color.white ? "white" : "black"; }


        //Получить цвет оппонета
        public string GetOpponentColor() { return game.player2.playerColor == Color.white ? "white" : "black"; }


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


        //Получить инфу о статусе партии
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


        //получить последний сделаный ход
        public string GetLastMove()
        {
            if (game.moves.movesStory.Count != 0) return game.moves.movesStory.Last();
            else return "there is no last move";
        }


        //получить список клеток, на которых находятся фигуры с пересчитанными(невалидными) векторами
        public List<string> RecalculatedPieces() 
        {
            List<string> result = new List<string>(game.moves.RecalculatedPiecesPosition);
            return result;
        }

    }
}
