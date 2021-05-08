using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    using static PiecesKeys;
    class Desk
    {
        internal ForsythEdwardsNotation notation;
        internal Piece[,] pieces = new Piece[8, 8];
        internal Square[,] deskSquares = new Square[8, 8];
        internal string curKilledPieceSquare = "none";


        //Создать доску
        internal Desk(ForsythEdwardsNotation notation)
        {
            this.notation = notation;
            SetPiecesOnDesk(this.notation.PiecePosition);
        }


        //Заполнить массив фигур
        private void SetPiecesOnDesk(string data)
        {
            for (int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
            data = data.Replace("1", ".");
            string[] lines = data.Split('/');
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    pieces[x, y] = lines[7 - y][x] == '.' ? Piece.nullPiece : ParseToPiece(lines[7 - y][x]);
            FillDeskSquares();
        }


        //Обновить инф. о положении фигур на доске (массивы фигур,клеток и взятых фигур)
        internal void UpdatePiecesOnDesk(PieceMove pieceMove, Color inGameColor)
        {
            curKilledPieceSquare = "none";
            Square kingSq = FindKing(inGameColor);
            King k = (King)kingSq.ownedPiece;

            if (pieceMove.name != " 0-0 " && pieceMove.name != "0-0-0")
            {
                int fromX = pieceMove.fromX; int fromY = pieceMove.fromY;
                int toX = pieceMove.toX; int toY = pieceMove.toY;

                //перемещение фигуры
                if (pieces[toX, toY] != Piece.nullPiece) { pieces[toX, toY].movesVector.status = "delete"; curKilledPieceSquare = pieceMove.to; }
                pieces[toX, toY] = pieces[fromX, fromY];
                pieces[toX, toY].movesVector.status = "recalculate";
                pieces[toX, toY].movesVector.startPosition = pieceMove.to;
                pieces[fromX, fromY] = Piece.nullPiece;

                //взятие на проходе
                if ((pieceMove.pieceKey == blackPawn || pieceMove.pieceKey == whitePawn) && notation.EnPassant == deskSquares[toX, toY].Name)
                {
                    pieces[toX, fromY].movesVector.status = "delete";
                    curKilledPieceSquare = deskSquares[toX, fromY].Name;
                    pieces[toX, fromY] = Piece.nullPiece;
                }
                

                //определяем был ли совершен ход пешкой на две клетки
                if (Math.Abs(toY - fromY) == 2 && (pieceMove.pieceKey == blackPawn || pieceMove.pieceKey == whitePawn))
                    notation.EnPassant = deskSquares[(toX + fromX) / 2, (toY + fromY) / 2].Name;
                else
                    notation.EnPassant = "-";

                //превращение пешки (Pe7e8N,Pa7b8Q и т.д.). Превращение фиксируется на 6-ом знаке(если оно есть)
                if (pieceMove.promotion) {
                    Piece piece = ParseToPiece(pieceMove.promotionCharKey);
                    pieces[toX, toY] = piece; 
                }

                //право рокировки навсегда теряется при ходе короля
                if ((k.shortCastling || k.longCastling) && pieceMove.pieceKey == k.pieceKey) { k.shortCastling = false; k.longCastling = false; }

                //право рокировки с ладьей навсегда теряется при ходе этой ладьи или ее взятии
                if (k.shortCastling && k.pieceKey == whiteKing && pieces[7, 0].pieceKey != whiteRook) { k.shortCastling = false; }
                if (k.longCastling && k.pieceKey == whiteKing && pieces[0, 0].pieceKey != whiteRook) { k.longCastling = false; }
                if (k.shortCastling && k.pieceKey == blackKing && pieces[7, 7].pieceKey != blackRook) { k.shortCastling = false; }
                if (k.longCastling && k.pieceKey == blackKing && pieces[0, 7].pieceKey != blackRook) { k.longCastling = false; }

            }
            else
            {
                MakeCastling(k, pieceMove);
            }

            UpdateDeskSquares();
        }

        //рокировка (всегда происходит на конкретных клетках, поэтому там присутствуют magic numbers)
        private void MakeCastling(King k, PieceMove pMove)
        {
            pieces[pMove.toKX, pMove.toKY] = pieces[pMove.fromKX, pMove.fromKY];
            pieces[pMove.toKX, pMove.toKY].movesVector.status = "recalculate";
            pieces[pMove.toKX, pMove.toKY].movesVector.startPosition = pMove.toK;
            pieces[pMove.fromKX, pMove.fromKY] = Piece.nullPiece;

            pieces[pMove.toRX, pMove.toRY] = pieces[pMove.fromRX, pMove.fromRY];
            pieces[pMove.toRX, pMove.toRY].movesVector.status = "recalculate";
            pieces[pMove.toRX, pMove.toRY].movesVector.startPosition = pMove.toR;
            pieces[pMove.fromRX, pMove.fromRY] = Piece.nullPiece;

            k.shortCastling = false; k.longCastling = false;
        }


        //Возвращает объект фигуры в зависимости от поступающего ключа
        private Piece ParseToPiece(char key)
        {
            switch (key)
            {
                case (char)whiteKing:
                    return new King(whiteKing, Color.white, notation.Castling);
                case (char)blackKing:
                    return new King(blackKing, Color.black, notation.Castling);
                case (char)whitePawn:
                    return new Pawn(whitePawn, Color.white);
                case (char)blackPawn:
                    return new Pawn(blackPawn, Color.black);
                case (char)whiteKnight:
                    return new Night(whiteKnight, Color.white);
                case (char)blackKnight:
                    return new Night(blackKnight, Color.black);
                case (char)whiteBishop:
                    return new Bishop(whiteBishop, Color.white);
                case (char)blackBishop:
                    return new Bishop(blackBishop, Color.black);
                case (char)whiteRook:
                    return new Rook(whiteRook, Color.white);
                case (char)blackRook:
                    return new Rook(blackRook, Color.black);
                case (char)whiteQueen:
                    return new Queen(whiteQueen, Color.white);
                case (char)blackQueen:
                    return new Queen(blackQueen, Color.black);

                default: return Piece.nullPiece;

            }
        }


        //Получить фигуру по координатам доски
        internal char GetFigureAt(int x, int y)
        {
            return (char)pieces[x, y].pieceKey;
        }


        //Инициализация массива клеток
        private void FillDeskSquares()
        {
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    deskSquares[x, y] = new Square(x, y);
            UpdateDeskSquares();
        }


        //Поставить фигуры на клетки
        private void UpdateDeskSquares()
        {
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    deskSquares[x, y].SetPieceOnSquare(pieces[x, y]);
        }


        //Проверяет нахождение короля под ударом врага
        internal bool IsKingInDanger(List<string> opTurnList, Color inGameColor)
        {
            Square kingSquare = FindKing(inGameColor);
            foreach (string opTurn in opTurnList)
            {
                if (kingSquare.Name == opTurn.Substring(3, 2))
                    return true;
            }
            return false;
        }


        //Получить клетку на которой стоит король
        internal Square FindKing(Color inGameColor)
        {
            PiecesKeys kingKey = inGameColor == Color.black ? blackKing : whiteKing;
            foreach (Square square in deskSquares)
                if (square.ownedPiece.pieceKey == kingKey) return square;
            return Square.none;
        }


    }
}
