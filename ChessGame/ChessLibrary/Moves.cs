using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Moves
    {
        public List<Vectors> whiteMoves = new List<Vectors>();
        public List<Vectors> blackMoves = new List<Vectors>();
        private readonly Desk desk;


        public Moves(Desk desk)
        {
            this.desk = desk;
        }


        public List<string> GetPlayerMoves(Color playerColor)
        {
            List<Vectors> moves;
            List<string> result = new List<string>();

            moves = (playerColor == Color.white) ? whiteMoves : blackMoves;

            foreach (Vectors vector in moves)
                result.AddRange(vector.GetMoves());

            return result;
        }


        public List<string> GetPieceMoves(Color playerColor, string pieceSquare)
        {
            List<string> result = new List<string>();
            Vectors pieceVector = (playerColor == Color.white)
                ? whiteMoves.Find(item => item.startPosition == pieceSquare)
                : blackMoves.Find(item => item.startPosition == pieceSquare);

            result.AddRange(pieceVector.GetMoves());

            return result;
        }


        public void CheckCastling(Color kingColor)
        {
            Square ksq = desk.FindKing(kingColor); ;
            King k = (King)ksq.ownedPiece;
            Vectors kingVector = (desk.notation.InGameColor == Color.white)
                ? whiteMoves.Find(item => item.vectorPieceKey == 'K')
                : blackMoves.Find(item => item.vectorPieceKey == 'k');


            if (!IsCheck())
            {
                if (k.shortCastling && k.IsShortCastlingPossibleNow(desk.deskSquares, ksq))
                {
                    if (k.pieceColor == Color.black && !IsMateAfterMove("ke8f8") && !IsMateAfterMove("ke8g8")) kingVector.avaibleSquares.Add(" 0-0 ");
                    if (k.pieceColor == Color.white && !IsMateAfterMove("ke1f1") && !IsMateAfterMove("ke1g1")) kingVector.avaibleSquares.Add(" 0-0 ");
                }

                if (k.longCastling && k.IsLongCastlingPossibleNow(desk.deskSquares, ksq))
                {
                    if (k.pieceColor == Color.black && !IsMateAfterMove("ke8d8") && !IsMateAfterMove("ke8c8")) kingVector.avaibleSquares.Add("0-0-0");
                    if (k.pieceColor == Color.white && !IsMateAfterMove("ke1d1") && !IsMateAfterMove("ke1c1")) kingVector.avaibleSquares.Add("0-0-0");
                }
            }

        }


        internal void UpdateMoves(string move = "none")
        {
            List<Vectors> activePlayerMoves;
            List<Vectors> waitingPlayerMoves;
            List<string> result = new List<string>();

            Color inGameColor = desk.notation.InGameColor;
            activePlayerMoves = (inGameColor == Color.white) ? whiteMoves : blackMoves;
            waitingPlayerMoves = (inGameColor == Color.white) ? blackMoves : whiteMoves;

            Vectors kingVector = inGameColor == Color.white
                ? whiteMoves.Find(item => item.vectorPieceKey == 'K')
                : blackMoves.Find(item => item.vectorPieceKey == 'k');

            if (kingVector != null)
            {
                kingVector.avaibleSquares.Remove("0-0-0");
                kingVector.avaibleSquares.Remove(" 0-0 ");
            }

            if (move == "none")
            {
                foreach (Vectors vector in activePlayerMoves)
                    vector.avaibleSquares.RemoveAll(item => IsMateAfterMove(vector.vectorPieceKey + vector.startPosition + item));
                CheckCastling(inGameColor);
                return;
            }

            if (move == "0-0-0" || move == " 0-0 ")
            {
                string fromK, toK, fromR, toR;

                if (inGameColor.FlipColor() == Color.white)
                {
                    fromK = "e1";
                    if (move == "0-0-0") 
                         { toK = "c1"; fromR = "a1"; toR = "d1"; }
                    else { toK = "g1"; fromR = "h1"; toR = "f1"; }
                }
                else
                {
                    fromK = "e8";
                    if (move == "0-0-0") 
                         { toK = "c8"; fromR = "a8"; toR = "d8"; }
                    else { toK = "g8"; fromR = "h8"; toR = "f8"; }
                }

                int indexK = waitingPlayerMoves.FindIndex(item => item.startPosition == fromK);
                int indexR = waitingPlayerMoves.FindIndex(item => item.startPosition == fromR);

                waitingPlayerMoves[indexK] = RecalculateVector(waitingPlayerMoves[indexK], toK);
                waitingPlayerMoves[indexR] = RecalculateVector(waitingPlayerMoves[indexR], toR);

                UpdateMovesLists(fromK, toK, ref activePlayerMoves, ref waitingPlayerMoves);
                UpdateMovesLists(fromR, toR, ref activePlayerMoves, ref waitingPlayerMoves);
            }
            else
            {
                string from = move.Substring(1, 2);
                string to = move.Substring(3, 2);

                activePlayerMoves.RemoveAll(item => item.startPosition == to);

                int index = waitingPlayerMoves.FindIndex(item => item.startPosition == from);
                waitingPlayerMoves[index] = RecalculateVector(waitingPlayerMoves[index], to);

                UpdateMovesLists(from, to, ref activePlayerMoves, ref waitingPlayerMoves);
            }

            foreach (Vectors vector in activePlayerMoves)
                vector.avaibleSquares.RemoveAll(item => IsMateAfterMove(vector.vectorPieceKey + vector.startPosition + item));
            CheckCastling(inGameColor);
        }


        private void UpdateMovesLists(string from, string to, ref List<Vectors> activePlayerMoves, ref List<Vectors> waitingPlayerMoves)
        {
            for (int i = 0; i < activePlayerMoves.Count; i++)
                if (InvalidVector(activePlayerMoves[i], from, to))
                    activePlayerMoves[i] = RecalculateVector(activePlayerMoves[i]);

            for (int i = 0; i < waitingPlayerMoves.Count; i++)
                if (InvalidVector(waitingPlayerMoves[i], from, to))
                    waitingPlayerMoves[i] = RecalculateVector(waitingPlayerMoves[i]);
        }


        private Vectors RecalculateVector(Vectors vector, string to = "none")
        {
            if(to!="none") vector.startPosition = to;
            Square pieceSqure = desk.deskSquares[vector.StartPositionX, vector.StartPositionY];
            return pieceSqure.ownedPiece.GetPieceMoves(desk, pieceSqure);
        }


        internal bool InvalidVector(Vectors vector, string from, string to)
        {
            if (vector.occupiedSquares.Exists(item => item == from) || vector.occupiedSquares.Exists(item => item == to)
                || vector.avaibleSquares.Exists(item => item == from) || vector.avaibleSquares.Exists(item => item == to))
                return true;
            return false;
        }


        //Проверка хода на угрозу мата
        public bool IsMateAfterMove(string move)
        {
            Color inGameColor = desk.notation.InGameColor;
            ForsythEdwardsNotation copyN = (ForsythEdwardsNotation)desk.notation.Clone();
            Desk copyDesk = new Desk(copyN);
            Moves copyMoves = new Moves(copyDesk);
            copyDesk.UpdatePiecesOnDesk(move, inGameColor);
            ChessPlayer op = new Bot(inGameColor.FlipColor(), copyMoves, copyDesk);
            return copyDesk.IsKingInDanger(copyMoves.GetPlayerMoves(op.playerColor), inGameColor);
        }


        //Проверка на шах
        public bool IsCheck()
        {
            Color inGameColor = desk.notation.InGameColor;
            ForsythEdwardsNotation copyN = (ForsythEdwardsNotation)desk.notation.Clone();
            Desk copyDesk = new Desk(copyN);
            Moves copyMoves = new Moves(copyDesk);
            ChessPlayer op = new Bot(inGameColor.FlipColor(), copyMoves, copyDesk);
            return copyDesk.IsKingInDanger(copyMoves.GetPlayerMoves(op.playerColor), inGameColor);
        }


        //Проверка на мат
        public bool IsMate()
        {
            if (GetPlayerMoves(desk.notation.InGameColor).Count == 0 && IsCheck()) return true;
            return false;
        }


        //Проверка на пат
        public bool IsPate()
        {
            if (GetPlayerMoves(desk.notation.InGameColor).Count == 0 && !IsCheck()) return true;
            return false;
        }


    }
}
