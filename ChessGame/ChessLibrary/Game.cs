﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChessLibrary
{
    class Game
    {
        int moveNumber;
        internal Desk desk;
        internal Moves moves;
        internal ChessPlayer player1;
        internal ChessPlayer player2;

        internal Color inGameColor { get; private set; }
        internal ForsythEdwardsNotation notation { get; private set; }
        
        Random rnd = new Random();


        //Начать партию
        internal void StartGame(string fen, Color playerColor)
        {
            notation = JsonConvert.DeserializeObject<ForsythEdwardsNotation>(fen);
            inGameColor = notation.InGameColor;
            moveNumber = notation.MoveNumber;
            desk = new Desk(notation);
            moves = new Moves(desk);
            
            if (playerColor == Color.none)
            {
                Color[] colors = new Color[2] { Color.white, Color.black };
                playerColor = colors[rnd.Next(0, 2)];
            }

            player1 = new Gamer(playerColor, moves, desk);
            player2 = new Bot(playerColor.FlipColor(), moves, desk);
            moves.InitMoves();
        }


        //Сохранить состояние игры(сохр. в формате ФЭН)
        internal void ParseGameToFEN()
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
        internal void PrepareNextMove(PieceMove pieceMove)
        {
            moveNumber++;
            inGameColor = inGameColor.FlipColor();
            ParseGameToFEN();
            moves.UpdateMoves(pieceMove);
        }


        //Получить положение фигур представленное в строке
        string FenPieces()
        {
            Piece[,] pieces = desk.pieces;
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    sb.Append(pieces[x, y] == Piece.nullPiece ? '1' : (char)pieces[x, y].pieceKey);
                if (y > 0) sb.Append('/');
            }
            string eight = "11111111";
            for (int j = 8; j >= 2; j--)
                sb.Replace(eight.Substring(0, j), j.ToString());
            return sb.ToString();
        }


    }
}
