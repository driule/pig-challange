using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Map
    {
        public int[,] Grid { get; }

        public Map()
        {
            this.Grid = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i == 0 || j == 0 || i == 8 || j == 8 || i == 1 || j == 1 || i == 7 || j == 7)
                    {
                        this.Grid[i, j] = 1;
                    }
                    else
                    {
                        this.Grid[i, j] = 0;
                    }
                }
            }

            // exits
            this.Grid[4, 1] = 0;
            this.Grid[4, 7] = 0;

            // obstacles
            this.Grid[3, 3] = 1;
            this.Grid[3, 5] = 1;
            this.Grid[5, 3] = 1;
            this.Grid[5, 5] = 1;
        }

        public bool IsCellEmpty(int x, int y, Agent agentA, Agent agentB, Pig pig)
        {
            Tuple<int, int> position = new Tuple<int, int>(x, y);

            // check for obstacles
            if (x == 3 && y == 3 || x == 3 && y == 5 || x == 5 && y == 3 || x == 5 && y == 5)
            {
                return false;
            }

            // check for agents and pig
            if (agentA.Position.Equals(position))
            {
                return false;
            }

            if (agentB.Position.Equals(position))
            {
                return false;
            }

            if (pig.Position.Equals(position))
            {
                return false;
            }

            return true;
        }

        public void Draw(Agent agentA, Agent agentB, Pig pig)
        {
            for (int i = 0; i < 9; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 9; j++)
                {
                    if (agentA.Position.Equals(new Tuple<int, int>(i, j)))
                    {
                        Console.Write("A");
                    }
                    else if (agentB.Position.Equals(new Tuple<int, int>(i, j)))
                    {
                        Console.Write("B");
                    }
                    else if (pig.Position.Equals(new Tuple<int, int>(i, j)))
                    {
                        Console.Write("o");
                    }
                    else if (this.Grid[i, j] == 0)
                    {
                        Console.Write(" ");
                    }
                    else if (this.Grid[i, j] == 1)
                    {
                        Console.Write("X");
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
