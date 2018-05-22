using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Map
    {
        public enum CellType
        {
            Empty = 0,
            Obstacle = 1,
            Exit = 2
        }

        public CellType[,] Grid { get; }

        public BasicAgent AgentA { get; }
        public BasicAgent AgentB { get; }
        public BasicAgent Pig { get; }

        public Map(BasicAgent agentA, BasicAgent agentB, BasicAgent pig)
        {
            this.AgentA = agentA;
            this.AgentB = agentB;
            this.Pig = pig;

            this.Grid = new CellType[9, 9];
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (y == 0 || x == 0 || y == 8 || x == 8 || y == 1 || x == 1 || y == 7 || x == 7)
                    {
                        this.Grid[y, x] = CellType.Obstacle;
                    }
                    else
                    {
                        this.Grid[y, x] = CellType.Empty;
                    }
                }
            }

            // exits
            this.Grid[4, 1] = CellType.Exit;
            this.Grid[4, 7] = CellType.Exit;

            // obstacles
            this.Grid[3, 3] = CellType.Obstacle;
            this.Grid[3, 5] = CellType.Obstacle;
            this.Grid[5, 3] = CellType.Obstacle;
            this.Grid[5, 5] = CellType.Obstacle;
        }

        public bool IsCellExit(int y, int x)
        {
            if (this.Grid[y, x] == CellType.Exit)
            {
                return true;
            }

            return false;
        }

        public bool IsCellEmpty(int y, int x)
        {
            Tuple<int, int> position = new Tuple<int, int>(y, x);

            // check for obstacles
            if (this.Grid[y, x] == CellType.Obstacle)
            {
                return false;
            }

            // check for agents and pig
            if (this.AgentA.Position.Equals(position))
            {
                return false;
            }

            if (this.AgentB.Position.Equals(position))
            {
                return false;
            }

            if (this.Pig.Position.Equals(position))
            {
                return false;
            }

            return true;
        }

        public void Draw()
        {
            for (int y = 0; y < 9; y++)
            {
                Console.Write("|");
                for (int x = 0; x < 9; x++)
                {
                    if (this.AgentA.Position.Equals(new Tuple<int, int>(y, x)))
                    {
                        Console.Write("A");
                    }
                    else if (this.AgentB.Position.Equals(new Tuple<int, int>(y, x)))
                    {
                        Console.Write("B");
                    }
                    else if (this.Pig.Position.Equals(new Tuple<int, int>(y, x)))
                    {
                        Console.Write("o");
                    }
                    else if (this.Grid[y, x] == CellType.Empty || this.Grid[y, x] == CellType.Exit)
                    {
                        Console.Write(" ");
                    }
                    else if (this.Grid[y, x] == CellType.Obstacle)
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
