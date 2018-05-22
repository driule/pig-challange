using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("welcome to pig challange!");

            for (int i = 0; i < 1; i++)
            {
                GameBoard gameBoard = new GameBoard();
                gameBoard.RunGame();
            }

            Console.ReadLine();
        }
    }
}
