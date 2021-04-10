using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    class Desk
    {
        public ForsythEdwardsNotation notation;
        public Piece[,] pieces;
        public List<Piece> deadPieces;
        public Square[,] deskSquares;

        //Создать доску
        public Desk(ForsythEdwardsNotation notation)
        {
            this.notation = notation;
            pieces = new Piece[8, 8];
            deskSquares = new Square[8, 8];
            deadPieces = new List<Piece>();
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
                    pieces[x, y] = lines[7 - y][x] == '.' ? null : ParseToPiece(lines[7 - y][x]);
            FillDeskSquares();
        }

        //Обновить инф. о положении фигур на доске (массивы фигур,клеток и взятых фигур)
        internal void UpdatePiecesOnDesk(string move, Color inGameColor)
        {
            Square kingSq = FindKing(inGameColor);
            King k = (King)kingSq.ownedPiece;

            if (String.Compare(move," 0-0 ")!=0 && String.Compare(move, "0-0-0") != 0)
            {
                int fromX = move[1] - 'a';  int fromY = move[2] - '1';
                int toX = move[3] - 'a';    int toY = move[4] - '1';

                //cохр.съеденные фигуры
                if (pieces[toX, toY] != null)
                    deadPieces.Add(pieces[toX, toY]);

                pieces[toX, toY] = null;
                pieces[toX, toY] = pieces[fromX, fromY];
                pieces[fromX, fromY] = null;

                //взятие на проходе
                if (String.Compare(notation.EnPassant, deskSquares[toX, toY].Name) == 0) {
                    pieces[toX, fromY] = null;
                }
                //определяем был ли совершен ход пешкой на две клетки
                if (Math.Abs(toY - fromY) == 2 && (move[0] == 'p' || move[0] == 'P'))
                    notation.EnPassant = deskSquares[(toX+fromX)/2, (toY + fromY)/2].Name;
                else
                    notation.EnPassant = "-";
                //превращение пешки (Pe7e8N,Pa7b8Q и т.д.). Превращение фиксируется на 6-ом знаке(если оно есть)
                if (move.Length == 6) pieces[toX, toY] = ParseToPiece(move[5]);
                //обнуляем рокировки при ходе короля
                if ((k.shortCastling || k.longCastling) && move[0]== k.key) { k.shortCastling = false; k.longCastling = false; }

            }
            else //рокировка (всегда происходит на конкретных клетках, поэтому там присутствуют magic numbers)
            {
                if (String.Compare(move, " 0-0 ") == 0 && inGameColor == Color.white) {
                    pieces[6, 0] = pieces[4, 0]; pieces[4, 0] = null;
                    pieces[5, 0] = pieces[7, 0]; pieces[7, 0] = null;
                }
                if (String.Compare(move, "0-0-0") == 0 && inGameColor == Color.white) {
                    pieces[2, 0] = pieces[4, 0]; pieces[4, 0] = null;
                    pieces[3, 0] = pieces[0, 0]; pieces[0, 0] = null;
                }
                if (String.Compare(move, " 0-0 ") == 0 && inGameColor == Color.black) {
                    pieces[6, 7] = pieces[4, 7]; pieces[4, 7] = null;
                    pieces[5, 7] = pieces[7, 7]; pieces[7, 7] = null;
                }
                if (String.Compare(move, "0-0-0") == 0 && inGameColor == Color.black) {
                    pieces[2, 7] = pieces[4, 7]; pieces[4, 7] = null;
                    pieces[3, 7] = pieces[0, 7]; pieces[0, 7] = null;
                }
                k.shortCastling = false; k.longCastling = false;
            }
            //право рокировки с ладьей навсегда теряется при ходе этой ладьи
            if(k.shortCastling && inGameColor==Color.white && (pieces[7, 0] == null || pieces[7,0].key!='R')) { k.shortCastling = false;} 
            if(k.longCastling && inGameColor==Color.white && (pieces[0, 0] == null || pieces[0,0].key!='R')) { k.longCastling = false; } 
            if(k.shortCastling && inGameColor==Color.black && (pieces[7, 7] == null || pieces[7,7].key!='r')) { k.shortCastling = false; } 
            if(k.longCastling && inGameColor==Color.black && (pieces[0, 7] == null || pieces[0,7].key!='r')) { k.longCastling = false; } 

            UpdateDeskSquares();
        }

        //Возвращает объект фигуры в зависимости от поступающего ключа
        private Piece ParseToPiece(char key)
        {
            switch (key)
            {
                case 'K':
                    return new King(key, Color.white, notation.Castling);
                case 'k':
                    return new King(key, Color.black, notation.Castling);
                case 'P':
                    return new Pawn(key, Color.white);
                case 'p':
                    return new Pawn(key, Color.black);
                case 'N':
                    return new Night(key, Color.white);
                case 'n':
                    return new Night(key, Color.black);
                case 'B':
                    return new Bishop(key, Color.white);
                case 'b':
                    return new Bishop(key, Color.black);
                case 'R':
                    return new Rook(key, Color.white);
                case 'r':
                    return new Rook(key, Color.black);
                case 'Q':
                    return new Queen(key, Color.white);
                case 'q':
                    return new Queen(key, Color.black);

                default: return null;

            }
        }

        //Получить фигуру по координатам доски
        public char GetFigureAt(int x, int y)
        {
            if (pieces[x, y]!=null)
                return pieces[x, y].key;
            else
                return '.';
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
            foreach(string opTurn in opTurnList)
            {
                if (String.Compare(kingSquare.Name, opTurn.Substring(3, 2)) == 0)
                    return true;
            }
            return false;
        }

        //Получить клетку на которой стоит король
        public Square FindKing(Color inGameColor)
        {
            char kingKey = inGameColor == Color.black ? 'k' : 'K';
            foreach (Square square in deskSquares)
                if (square.ownedPiece!=null && square.ownedPiece.key == kingKey)
                    return square;
            return Square.none;
        }
    }
}
