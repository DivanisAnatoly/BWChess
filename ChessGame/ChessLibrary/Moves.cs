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
        private readonly Desk desk;
        private Color inGameColor;


        public Moves(Desk desk)
        {
            this.desk = desk;
        }


        public List<string> GetPlayerMoves(Color playerColor)
        {
            List<string> result = new List<string>();
            List<Vectors> moves = (playerColor == Color.white) ? whiteMoves : blackMoves;

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
                if (k.CanMakeShortCastling(desk.deskSquares, ksq))
                {
                    if (k.pieceColor == Color.black && !IsMateAfterMove("ke8f8", kingVector) && !IsMateAfterMove("ke8g8", kingVector)) kingVector.avaibleSquares.Add(" 0-0 ");
                    if (k.pieceColor == Color.white && !IsMateAfterMove("ke1f1", kingVector) && !IsMateAfterMove("ke1g1", kingVector)) kingVector.avaibleSquares.Add(" 0-0 ");
                }

                if (k.CanMakeLongCastling(desk.deskSquares, ksq))
                {
                    if (k.pieceColor == Color.black && !IsMateAfterMove("ke8d8", kingVector) && !IsMateAfterMove("ke8c8", kingVector)) kingVector.avaibleSquares.Add("0-0-0");
                    if (k.pieceColor == Color.white && !IsMateAfterMove("ke1d1", kingVector) && !IsMateAfterMove("ke1c1", kingVector)) kingVector.avaibleSquares.Add("0-0-0");
                }
            }
        }


        internal void InitMoves()
        {
            inGameColor = desk.notation.InGameColor;

            List<Vectors> activePlayerMoves = (inGameColor == Color.white) ? whiteMoves : blackMoves; ;

            for (int i = 0; i < activePlayerMoves.Count; i++)
            {
                Vectors vector = activePlayerMoves[i];
                vector.avaibleSquares.RemoveAll(item => IsMateAfterMove((char)vector.vectorPieceKey + vector.startPosition + item, vector));
            }

            CheckCastling(inGameColor);
            CheckCastling(inGameColor.FlipColor());
        }


        internal void UpdateMoves(PieceMove pieceMove = null)
        {
            RecalculatedPiecesPosition.Clear();

            inGameColor = desk.notation.InGameColor;

            List<Vectors> activePlayerMoves = (inGameColor == Color.white) ? whiteMoves : blackMoves; ;
            List<Vectors> waitingPlayerMoves = (inGameColor == Color.white) ? blackMoves : whiteMoves; ;

            Vectors wKingVector = whiteMoves.Find(item => item.vectorPieceKey == whiteKing);
            Vectors bKingVector = blackMoves.Find(item => item.vectorPieceKey == blackKing);

            wKingVector.avaibleSquares.Remove("0-0-0");
            wKingVector.avaibleSquares.Remove(" 0-0 ");
            bKingVector.avaibleSquares.Remove("0-0-0");
            bKingVector.avaibleSquares.Remove(" 0-0 ");


            if (pieceMove.castling != "none")
                CastlingMoveUpdate(pieceMove, ref activePlayerMoves, ref waitingPlayerMoves);
            else
                UsualMoveUpdate(pieceMove, ref activePlayerMoves, ref waitingPlayerMoves);


            for (int i = 0; i < activePlayerMoves.Count; i++)
            {
                Vectors vector = activePlayerMoves[i];
                vector.avaibleSquares.RemoveAll(item => IsMateAfterMove((char)vector.vectorPieceKey + vector.startPosition + item, vector));
            }
            CheckCastling(inGameColor);
            CheckCastling(inGameColor.FlipColor());

            movesStory.Add(pieceMove.name);
        }


        private void UsualMoveUpdate(PieceMove pieceMove, ref List<Vectors> activePlayerMoves, ref List<Vectors> waitingPlayerMoves)
        {
            string from = pieceMove.from;
            string to = pieceMove.to;

            activePlayerMoves.RemoveAll(item => item.status == "delete");

            UpdateMovesLists(from, to, ref activePlayerMoves, ref waitingPlayerMoves);
        }


        private void CastlingMoveUpdate(PieceMove pieceMove, ref List<Vectors> activePlayerMoves, ref List<Vectors> waitingPlayerMoves)
        {
            string fromK, toK, fromR, toR;

            fromK = pieceMove.fromK;
            toK = pieceMove.toK;
            fromR = pieceMove.fromR;
            toR = pieceMove.toR;

            UpdateMovesLists(fromK, toK, ref activePlayerMoves, ref waitingPlayerMoves);
            UpdateMovesLists(fromR, toR, ref activePlayerMoves, ref waitingPlayerMoves);
        }


        //Обновление списков векторов               
        private void UpdateMovesLists(string from, string to, ref List<Vectors> activePlayerMoves, ref List<Vectors> waitingPlayerMoves)
        {
            for (int i = 0; i < activePlayerMoves.Count; i++)
            {
                Vectors vector = activePlayerMoves[i];
                if (vector.status == "normal" && InvalidVector(vector, from, to))
                    activePlayerMoves[i].status = "recalculate";
            }


            for (int i = 0; i < waitingPlayerMoves.Count; i++)
            {
                Vectors vector = waitingPlayerMoves[i];
                if (vector.status == "normal" && InvalidVector(vector, from, to))
                    waitingPlayerMoves[i].status = "recalculate";
            }


            for (int i = 0; i < activePlayerMoves.Count; i++)
            {
                Vectors vector = activePlayerMoves[i];
                if (vector.status == "recalculate")
                    activePlayerMoves[i] = RecalculateVector(vector);
            }


            for (int i = 0; i < waitingPlayerMoves.Count; i++)
            {
                Vectors vector = waitingPlayerMoves[i];
                if (vector.status == "recalculate")
                    waitingPlayerMoves[i] = RecalculateVector(vector);
            }

        }


        //Пересчет вектора
        private Vectors RecalculateVector(Vectors vector)
        {
            RecalculatedPiecesPosition.Add(vector.startPosition);
            Square pieceSqure = desk.deskSquares[vector.StartPositionX, vector.StartPositionY];
            return pieceSqure.ownedPiece.GetPieceMoves(desk, pieceSqure);
        }


        //Проверка на невалидность вектора
        internal bool InvalidVector(Vectors vector, string from, string to)
        {
            if (vector.occupiedSquares.Exists(item => item.Contains(from)) || vector.occupiedSquares.Exists(item => item.Contains(to))
                || vector.avaibleSquares.Exists(item => item.Contains(from)) || vector.avaibleSquares.Exists(item => item.Contains(to)))
                return true;

            string kSquareName = desk.curKilledPieceSquare;
            if (kSquareName != "none" && kSquareName != to && (vector.occupiedSquares.Exists(item => item.Contains(kSquareName)) 
                || vector.avaibleSquares.Exists(item => item.Contains(kSquareName)))) 
                return true;

            return false;
        }


        public bool IsMateAfterMove(string move, Vectors vector)
        {
            bool result;
            PieceMove pieceMove = new PieceMove(inGameColor, move);
            ForsythEdwardsNotation copyN = (ForsythEdwardsNotation)desk.notation.Clone();
            Desk copyDesk = new Desk(copyN);
            Moves copyMoves = new Moves(copyDesk);
            copyDesk.UpdatePiecesOnDesk(pieceMove, inGameColor);
            ChessPlayer op = new Bot(inGameColor.FlipColor(), copyMoves, copyDesk);

            result = copyDesk.IsKingInDanger(copyMoves.GetPlayerMoves(op.playerColor), inGameColor);

            if (result && vector.status != "recalculate")
            {
                vector.status = "recalculate";
                RecalculatedPiecesPosition.Add(pieceMove.from);
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
            return (GetPlayerMoves(desk.notation.InGameColor).Count == 0 && IsCheck());
        }


        //Проверка на пат
        public bool IsPate()
        {
            return (GetPlayerMoves(desk.notation.InGameColor).Count == 0 && !IsCheck());
        }


    }
}
