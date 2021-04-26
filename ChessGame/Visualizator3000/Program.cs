using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLibrary;

namespace Visualizator3000
{
    class Program
    {
        static ChessLibrary.GameManager gameManager = new GameManager();
        static List<char> whitePieces = new List<char>() { 'K', 'R', 'Q', 'B', 'P', 'N' };
        static List<char> blackPieces = new List<char>() { 'k', 'r', 'q', 'b', 'p', 'n' };
        static List<char> myColorPieces;
        static List<string> pieceMovesList;

        static void Main(string[] args)
        {
            gameManager.StartGame(playerColor:"white");

            myColorPieces = (gameManager.GetMyColor() == "white") ? whitePieces : blackPieces;

            while (true)
            {
                Print(ChessToAscii());

                Console.WriteLine("\n-FEN:" + gameManager.GetGameFen() + "\n");
                
                
                string gameState = gameManager.GameState();
                if (gameState != "CHECK" && gameState != "GAME IN PROGRESS") { Console.WriteLine("-Game state: " + gameState); break; }
                Console.WriteLine("-Game state: " + gameState);


                Console.WriteLine("\n-Player color:" + gameManager.GetMyColor() + "\n");
                Console.WriteLine("-Last move(" + gameManager.GetOpponentColor() + "):" + gameManager.GetLastMove() + "\n");

                List<string> recalculatedPieces = gameManager.RecalculatedPieces();
                Console.WriteLine("-Number of recalculated vectors : " + recalculatedPieces.Count());

                if (gameManager.GetMyColor() == gameManager.GetInGameColor())
                {
                    string move;
                    string pieceSquare = "";
                    string moveToSquare;

                    if (ChoosePiece(ref pieceSquare))
                    {
                        while (true)
                        {
                            moveToSquare = Console.ReadLine();
                            move = (gameManager.GetFigureAt(pieceSquare) + pieceSquare + moveToSquare).ToString();

                            if (moveToSquare == "0-0-0" || moveToSquare == " 0-0 ") move = moveToSquare;
                            if (moveToSquare == "q") break;

                            if (pieceMovesList.Exists(item => item == move))
                            {
                                gameManager.PlayerMove(move);
                                break;
                            }
                        }
                    }
                }
                else
                    gameManager.BotMove();
            }

            Console.ReadKey();
        }


        private static bool ChoosePiece(ref string pieceSquare)
        {
            pieceSquare = Console.ReadLine();
            if (pieceSquare.Length != 2)
                return false;
            else
            {
                char piece = gameManager.GetFigureAt(pieceSquare);

                if (!myColorPieces.Exists(item => item == piece)) return false;

                pieceMovesList = gameManager.GetAllAvaibleMoves(pieceSquare);
                if (pieceMovesList.Count == 0) return false;

                Print(PrintPieceMoves(pieceSquare, pieceMovesList));
            }
            return true;
        }


        static string PrintPieceMoves(string pieceSquare, List<string> pieceMovesList)
        {
            string move, moveToSquare;
            char piece = gameManager.GetFigureAt(pieceSquare);
            bool longCastling = (piece == 'K' || piece == 'k') && pieceMovesList.Contains("0-0-0");
            bool shortCastling = (piece == 'K' || piece == 'k') && pieceMovesList.Contains(" 0-0 ");


            string text = "\t\t\t\t\t\t  +-----------------+\n";

            for (int y = 7; y >= 0; y--)
            {
                text += "\t\t\t\t\t\t" + (y + 1) + " | ";

                for (int x = 0; x < 8; x++)
                {
                    moveToSquare = ("" + (char)('a' + x) + (y + 1)).ToString();
                    move = (piece + pieceSquare + moveToSquare);

                    if (pieceMovesList.Exists(item => item.Contains(move)))
                        if (gameManager.GetFigureAt(x, y) == '.') text += "o ";
                        else
                        {
                            if (pieceMovesList.Exists(item => item == (move + "Q")) ||
                                pieceMovesList.Exists(item => item == (move + "q"))) text += "? ";
                            else text += "x ";
                        }
                    else
                        text += gameManager.GetFigureAt(x, y) + " ";
                }
                text += "|\n";
            }

            text += "\t\t\t\t\t\t  +-----------------+\n";
            text += "\t\t\t\t\t\t    a b c d e f g h\n";

            if (longCastling) text += "\nlongCastling\n+-----------+\n| R . . . K |\n+-----------+\n| . . K R . |\n+-----------+\n";
            if (shortCastling) text += "\nshortCastling\n+---------+\n| K . . R |\n+---------+\n| . R K . |\n+---------+\n";

            return text;
        }


        static string ChessToAscii()
        {
            string text = "\t\t\t\t\t\t  +-----------------+\n";
            for (int y = 7; y >= 0; y--)
            {
                text += "\t\t\t\t\t\t";
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 8; x++)
                    text += gameManager.GetFigureAt(x, y) + " ";
                text += "|\n";
            }
            text += "\t\t\t\t\t\t  +-----------------+\n";
            text += "\t\t\t\t\t\t    a b c d e f g h\n";
            return text;
        }


        static void Print(string text)
        {
            Console.Clear();
            ConsoleColor oldForeColor = Console.ForegroundColor;
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z' || x >= '1' && x <= '8')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (x >= 'A' && x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                if (x == 'o')
                    Console.ForegroundColor = ConsoleColor.Green;
                if (x == 'x')
                    Console.ForegroundColor = ConsoleColor.Yellow;
                if (x == '?')
                    Console.ForegroundColor = ConsoleColor.DarkCyan;

                Console.Write(x);
            }
            Console.ForegroundColor = oldForeColor;
        }


    }
}
