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
        static List<string> list;

        static void Main(string[] args)
        {
            ChessLibrary.GameManager gameManager = new GameManager();
            

            gameManager.StartGame();
            
            while (true){
                Print(ChessToAscii(gameManager));
                Console.WriteLine();
                Console.WriteLine(gameManager.getGameFen());
                Console.WriteLine();
                list = gameManager.GetAllAvaibleMoves();
                if (list.Count == 0) break;
                foreach(string m in list)
                    Console.Write(m + "    ");

                Console.WriteLine(gameManager.IsCheck() ? "CHECK" : "-");

                if (gameManager.GetMyColor() == gameManager.GetInGameColor())
                {
                    string move = Console.ReadLine(); 
                    if (move == "q") break;
                    if (move.Length == 2)
                    {
                        Print(PrintPieceMoves(move, gameManager));
                        Console.ReadKey();
                        continue;
                    }
                    if(list.Contains(move))gameManager.PlayerMove(move);
                }
                else
                {
                    gameManager.BotMove();
                }
            }
            
            if(gameManager.IsPate()) Console.WriteLine("PATE!");

            if (gameManager.IsMate())
                if(gameManager.GetMyColor() == gameManager.GetInGameColor())
                    Console.WriteLine("YOU LOSE!");
                else
                    Console.WriteLine("YOU WIN!");

            Console.ReadKey();
        }

        static string PrintPieceMoves(string from, GameManager gameManager)
        {
            List<string> pieceMoves = new List<string>(); 

            string cell; 
            int pX = from[0]-'a';
            int pY = from[1]-'1';
            char piece = gameManager.GetFigureAt(pX, pY);
            bool longCastling = (piece == 'K' && list.Contains("0-0-0"));
            bool shortCastling = (piece == 'K' && list.Contains(" 0-0 "));

            
            foreach(string moveStr in list)
                if (moveStr.Contains(from)) pieceMoves.Add(moveStr);

            string text = "\t\t\t\t\t\t  +-----------------+\n";
            for (int y = 7; y >= 0; y--)
            {
                text += "\t\t\t\t\t\t" + (y + 1) + " | ";

                for (int x = 0; x < 8; x++)
                {
                    cell = (piece + from + (char)('a' + x) + (y + 1)).ToString();

                    if (pieceMoves.Contains(cell) || pieceMoves.Contains((cell+'Q').ToString()) )
                        if (gameManager.GetFigureAt(x, y)=='.')
                            text += "o ";
                        else
                        {
                            if (pieceMoves.Contains((cell + 'Q').ToString()))
                                text += "? ";
                            else
                                text += "x ";
                        }      
                    else
                        text += gameManager.GetFigureAt(x, y) + " ";
                }
                text += "|\n";
            }
            text += "\t\t\t\t\t\t  +-----------------+\n";
            text += "\t\t\t\t\t\t    a b c d e f g h\n";
            if(longCastling) text += "\nlongCastling\n+-----------+\n| R . . . K |\n+-----------+\n| . . K R . |\n+-----------+\n";
            if(shortCastling) text += "\nshortCastling\n+---------+\n| K . . R |\n+---------+\n| . R K . |\n+---------+\n";
           
            return text;
        }


        static string ChessToAscii(GameManager gameManager)
        {
            string text = "\t\t\t\t\t\t  +-----------------+\n";
            for (int y = 7; y >= 0; y--)
            {
                text += "\t\t\t\t\t\t";
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 8; x++)
                {
                    text += gameManager.GetFigureAt(x, y) + " ";
                }
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
