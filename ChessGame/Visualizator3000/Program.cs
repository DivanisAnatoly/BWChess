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
        static Dictionary<string, List<string>> movesCash = new Dictionary<string, List<string>>();
        static ChessLibrary.GameManager gameManager = new GameManager();

        static void Main(string[] args)
        {
            gameManager.StartGame();

            while (true)
            {
                Print(ChessToAscii());
                Console.WriteLine("\n" + gameManager.getGameFen() + "\n");

                string gameState = gameManager.GameState();
                Console.WriteLine(gameState);
                if (gameState != "CHECK" && gameState != "GAME IN PROGRESS") break;

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

                            if (movesCash[pieceSquare].Exists(item => item.Contains(move)))
                            {
                                gameManager.PlayerMove(move);
                                movesCash.Clear();
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
                List<string> pieceMovesList;
                char piece = gameManager.GetFigureAt(pieceSquare);

                if (piece == 'E' || piece == '.') return false;
                if (gameManager.GetMyColor() == 'w' && !(piece >= 'A' && piece <= 'Z')) return false;
                //if(gameManager.GetMyColor() == 'b' &&  !(piece>='a' && piece <= 'z')) return false;

                if (movesCash.ContainsKey(pieceSquare))
                    pieceMovesList = movesCash[pieceSquare];
                else
                {
                    pieceMovesList = gameManager.GetAllAvaibleMoves(pieceSquare);
                    if (pieceMovesList.Count == 0) return false;
                    movesCash.Add(pieceSquare, pieceMovesList);
                }

                Print(PrintPieceMoves(pieceSquare, pieceMovesList));
            }
            return true;
        }


        static string PrintPieceMoves(string pieceSquare, List<string> pieceMovesList)
        {
            string move;
            char piece = gameManager.GetFigureAt(pieceSquare);
            bool longCastling = (piece == 'K' && pieceMovesList.Contains("0-0-0"));
            bool shortCastling = (piece == 'K' && pieceMovesList.Contains(" 0-0 "));


            string text = "\t\t\t\t\t\t  +-----------------+\n";

            for (int y = 7; y >= 0; y--)
            {
                text += "\t\t\t\t\t\t" + (y + 1) + " | ";

                for (int x = 0; x < 8; x++)
                {
                    move = (piece + pieceSquare + (char)('a' + x) + (y + 1)).ToString();
                    if (pieceMovesList.Exists(item => item.Contains(move)))
                        if (gameManager.GetFigureAt(x, y) == '.') text += "o ";
                        else
                        {
                            if (pieceMovesList.Exists(item => item.Contains(move + "Q"))) text += "? ";
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
