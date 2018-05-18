using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Board
    {
        Random rand = new Random();

        private Agent agentA, agentB;
        private Pig pig;
        private int[,] map;
        
        public Board()
        {
            this.agentA = new Agent(this.GetRandomStartPosition()); 
            this.agentB = new Agent(this.GetRandomStartPosition());
            this.pig = new Pig(this.GetRandomStartPosition());

            this.InitMap();
        }

        public void RunGame()
        {
            for (int i = 0; i < 15; i++)
            {
                Tuple<int, int> newPositionAgentA = this.agentA.DetermineStep();
                Tuple<int, int> newPositionAgentB = this.agentB.DetermineStep();
                Tuple<int, int> newPositionPig = this.pig.DetermineStep();

                this.Draw();
            }
        }

        private void Draw()
        {
            for (int i = 0; i < 9; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 9; j++)
                {
                    if (this.agentA.Position.Equals(new Tuple<int, int>(i, j)))
                    {
                        Console.Write("A");
                    }
                    else if (this.agentB.Position.Equals(new Tuple<int, int>(i, j)))
                    {
                        Console.Write("B");
                    }
                    else if (this.pig.Position.Equals(new Tuple<int, int>(i, j)))
                    {
                        Console.Write("o");
                    }
                    else if (this.map[i, j] == 0)
                    {
                        Console.Write(" ");
                    }
                    else if (this.map[i, j] == 1)
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

        // TODO: check if positions of agents and pig don't collide
        private Tuple<int,int> GetRandomStartPosition()
        {
            int x = 0, y = 0;
            while (true)
            {
                x = rand.Next(2, 7);
                y = rand.Next(2, 7);

                // check for obstacles
                if (x == 3 && y == 3 || x == 3 && y == 5 || x == 5 && y == 3 || x == 5 && y == 5)
                {
                    continue;
                }

                break;
            }

            return new Tuple<int, int>(y, x);
        }

        private void InitMap()
        {
            this.map = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (i == 0 || j == 0 || i == 8 || j == 8 || i == 1 || j == 1 || i == 7 || j == 7)
                    {
                        this.map[i, j] = 1;
                    }
                    else
                    {
                        this.map[i, j] = 0;
                    }
                }
            }

            // exits
            this.map[4, 1] = 0;
            this.map[4, 7] = 0;

            // obstacles
            this.map[3, 3] = 1;
            this.map[3, 5] = 1;
            this.map[5, 3] = 1;
            this.map[5, 5] = 1;
        }
    }
}
