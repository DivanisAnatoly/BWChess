using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    using static PiecesKeys;
    class Moves
    {
        internal List<Vectors> whiteMoves = new List<Vectors>();
        internal List<Vectors> blackMoves = new List<Vectors>();
        internal List<string> movesStory = new List<string>();
        internal List<string> RecalculatedPiecesPosition = new List<string>();
        internal List<string> KingGuardsPiecesPosition = new List<string>();
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
            Vectors kingVector = (kingColor == Color.white)
                ? whiteMoves.Find(item => item.vectorPieceKey == whiteKing)
                : blackMoves.Find(item => item.vectorPieceKey == blackKing);


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
            RecalculatedPiecesPosition.Clear();

            List<Vectors> activePlayerMoves;
            List<Vectors> waitingPlayerMoves;

            Color inGameColor = desk.notation.InGameColor;
            activePlayerMoves = (inGameColor == Color.white) ? whiteMoves : blackMoves;
            waitingPlayerMoves = (inGameColor == Color.white) ? blackMoves : whiteMoves;

            Vectors wKingVector = whiteMoves.Find(item => item.vectorPieceKey == whiteKing);
            Vectors bKingVector = blackMoves.Find(item => item.vectorPieceKey == blackKing);

            if (wKingVector != null)
            {
                wKingVector.avaibleSquares.Remove("0-0-0");
                wKingVector.avaibleSquares.Remove(" 0-0 ");
            }

            if (bKingVector != null)
            {
                bKingVector.avaibleSquares.Remove("0-0-0");
                bKingVector.avaibleSquares.Remove(" 0-0 ");
            }


            if (move == "none")
            {
                foreach (Vectors vector in activePlayerMoves)
                    vector.avaibleSquares.RemoveAll(item => IsMateAfterMove((char)vector.vectorPieceKey + vector.startPosition + item));
                CheckCastling(inGameColor);
                CheckCastling(inGameColor.FlipColor());
                return;
            }


            movesStory.Add(move);


            if (move == "0-0-0" || move == " 0-0 ")
                CastlingMoveUpdate(move, inGameColor, ref activePlayerMoves, ref waitingPlayerMoves);
            else
                UsualMoveUpdate(move, inGameColor, ref activePlayerMoves, ref waitingPlayerMoves);

            KingGuardsPiecesPosition.Clear();

            foreach (Vectors vector in activePlayerMoves)
                vector.avaibleSquares.RemoveAll(item => IsMateAfterMove((char)vector.vectorPieceKey + vector.startPosition + item));
            CheckCastling(inGameColor);
            CheckCastling(inGameColor.FlipColor());
        }


        private void UsualMoveUpdate(string move, Color inGameColor, ref List<Vectors> activePlayerMoves, ref List<Vectors> waitingPlayerMoves)
        {
            string from = move.Substring(1, 2);
            string to = move.Substring(3, 2);

            activePlayerMoves.RemoveAll(item => item.startPosition == to);

            int index = waitingPlayerMoves.FindIndex(item => item.startPosition == from);
            waitingPlayerMoves[index] = RecalculateVector(waitingPlayerMoves[index], to);

            UpdateMovesLists(from, to, ref activePlayerMoves, ref waitingPlayerMoves);
        }


        private void CastlingMoveUpdate(string move, Color inGameColor, ref List<Vectors> activePlayerMoves, ref List<Vectors> waitingPlayerMoves)
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


        //Обновление списков векторов               
        private void UpdateMovesLists(string from, string to, ref List<Vectors> activePlayerMoves, ref List<Vectors> waitingPlayerMoves)
        {
            for (int i = 0; i < activePlayerMoves.Count; i++)
            {
                string vectorStartPosition = activePlayerMoves[i].startPosition;
                if (InvalidVector(activePlayerMoves[i], from, to) || KingGuardsPiecesPosition.Exists(item => item == vectorStartPosition))
                    activePlayerMoves[i] = RecalculateVector(activePlayerMoves[i]);
            }


            for (int i = 0; i < waitingPlayerMoves.Count; i++)
            {
                string vectorStartPosition = waitingPlayerMoves[i].startPosition;
                if (InvalidVector(waitingPlayerMoves[i], from, to) || KingGuardsPiecesPosition.Exists(item => item == vectorStartPosition))
                    waitingPlayerMoves[i] = RecalculateVector(waitingPlayerMoves[i]);
            }

        }


        //Пересчет вектора
        private Vectors RecalculateVector(Vectors vector, string to = "none")
        {
            RecalculatedPiecesPosition.Add(vector.startPosition);
            if (to != "none") vector.startPosition = to;
            Square pieceSqure = desk.deskSquares[vector.StartPositionX, vector.StartPositionY];
            return pieceSqure.ownedPiece.GetPieceMoves(desk, pieceSqure);
        }


        //Проверка на невалидность вектора
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
            bool result;
            Color inGameColor = desk.notation.InGameColor;
            ForsythEdwardsNotation copyN = (ForsythEdwardsNotation)desk.notation.Clone();
            Desk copyDesk = new Desk(copyN);
            Moves copyMoves = new Moves(copyDesk);
            copyDesk.UpdatePiecesOnDesk(move, inGameColor);
            ChessPlayer op = new Bot(inGameColor.FlipColor(), copyMoves, copyDesk);

            result = copyDesk.IsKingInDanger(copyMoves.GetPlayerMoves(op.playerColor), inGameColor);
            if (result)
            {
                string movedFrom = move.Substring(1, 2);
                KingGuardsPiecesPosition.Add(movedFrom);
            }
            return result;
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
