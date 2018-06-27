using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using RoyT.AStar;

namespace pig_challenge
{
    class Map
    {
        const int MAX_COST = 999;

        public enum CellType
        {
            Empty = 0,
            Obstacle = 1,
            Exit = 2
        }

        RNGCryptoServiceProvider randomizer;

        public float maxDistanceToPig = 9.0f;
        // Create a new grid and let each cell have a default traversal cost of 1.0
        private Grid grid;

        public Map()
        {
            this.grid = new Grid(9, 9, 1.0f);

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (y == 0 || x == 0 || y == 8 || x == 8 || y == 1 || x == 1 || y == 7 || x == 7)
                    {
                        // Block some cells (for example walls)
                        this.grid.BlockCell(new Position(x, y));
                    }
                }
            }

            // exits
            this.grid.UnblockCell(this.GetExit1Position());
            this.grid.UnblockCell(this.GetExit2Position());

            // obstacles
            this.grid.BlockCell(new Position(3, 3));
            this.grid.BlockCell(new Position(3, 5));
            this.grid.BlockCell(new Position(5, 3));
            this.grid.BlockCell(new Position(5, 5));

            this.randomizer = new RNGCryptoServiceProvider();
        }

        public Position GetExit1Position()
        {
            return new Position(1, 4);
        }

        public Position GetExit2Position()
        {
            return new Position(7, 4);
        }

        public bool IsCellExit(int x, int y)
        {
            Position exit1 = this.GetExit1Position();
            Position exit2 = this.GetExit2Position();

            if ((x == exit1.X && y == exit1.Y) || (x == exit2.X && y == exit2.Y))
            {
                return true;
            }

            return false;
        }

        public bool IsCellEmpty(int x, int y, State state)
        {
            Position position = new Position(x, y);

            // check for obstacles
            if (float.IsInfinity(this.grid.GetCellCost(position))) // ==  this.Grid[position[0], position[1]] == CellType.Obstacle)
            {
                return false;
            }

            // check for agents and pig
            if (state.PositionAgentA.Equals(position))
            {
                return false;
            }

            if (state.PositionAgentB.Equals(position))
            {
                return false;
            }

            if (state.PositionPig.Equals(position))
            {
                return false;
            }

            return true;
        }

        // unpredictable: find all suitable positions and randomly select one
        public Position GetRandomStartPosition(State state)
        {
            byte[] byteArray = new byte[4];
            int x = 0, y = 0;
            while (true)
            {
                x = this.GetRandomInt(2, 6, byteArray);
                y = this.GetRandomInt(2, 6, byteArray);

                if (!this.IsCellEmpty(x, y, state))
                {
                    continue;
                }

                break;
            }

            return new Position( x, y );
        }

        public void Draw(int iteration, State state)
        {
            if (!Program.PRINT_DEBUG_INFO)
                return;

            Console.WriteLine($"Turn {iteration}");
            for (int y = 0; y < 9; y++)
            {
                //Console.Write("|");
                for (int x = 0; x < 9; x++)
                {
                    Position position = new Position(x, y);
                    if (state.PositionAgentA.Equals(position))
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" A ");
                        Console.ResetColor();
                    }
                    else if (state.PositionAgentB.Equals(position))
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(" B ");
                        Console.ResetColor();
                    }
                    else if (state.PositionPig.Equals(position))
                    {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" P ");
                        Console.ResetColor();
                    }
                    else if (this.GetExit1Position().Equals(position) || this.GetExit2Position().Equals(position))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.Write("   ");
                        Console.ResetColor();
                    }
                    else if (!float.IsInfinity(this.grid.GetCellCost(position)))
                    {
                        Console.Write("   ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.Write("   ");
                        Console.ResetColor();
                    }
                    //Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            //state.Print();
        }

        /// <summary>
        /// Get a list of all available positions in the agent's vicinity
        /// </summary>
        /// <param name="position"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<Position> GetAvailablePositions(Position currentAgentPosition, State state)
        {
            List<Position> availablePositions = new List<Position>
            {
                new Position(currentAgentPosition.X, currentAgentPosition.Y - 1), // up
                new Position(currentAgentPosition.X + 1, currentAgentPosition.Y), // right
                new Position(currentAgentPosition.X, currentAgentPosition.Y + 1), // pown
                new Position(currentAgentPosition.X - 1, currentAgentPosition.Y)  // left
            };

            // filter on actual possibility
            List<Position> possiblePositions = availablePositions.Where(
                pos =>
                {
                    int x = pos.X;
                    int y = pos.Y;

                    return x >= 0 && y >= 0 && x < 9 && y < 9 && this.IsCellEmpty(x, y, state);
                }
            ).ToList();

            possiblePositions.Add(new Position(currentAgentPosition.X, currentAgentPosition.Y));

            return possiblePositions;
        }

        public IList<Position> GetPathToPig(BasicAgent.AgentIdentifier agentId, State state)
        {
            Position currentAgentPosition = state.GetAgentPosition(agentId);

            return GetPathToPigFromPosition(state, currentAgentPosition);
        }

        public IList<Position> GetPathToPigFromPosition(State state, Position position)
        { 
            Position pigPosition = state.GetAgentPosition(BasicAgent.AgentIdentifier.Pig);

            return GetPathToGoalPositionFromStartPosition(state, position, pigPosition);
        }

        public IList<Position> GetPathToGoalPositionFromStartPosition(State state, Position startPosition, Position goalPosition)
        {
            Position agentAPosition = state.GetAgentPosition(BasicAgent.AgentIdentifier.AgentA);
            Position agentBPosition = state.GetAgentPosition(BasicAgent.AgentIdentifier.AgentB);

            this.grid.BlockCell(agentAPosition);
            this.grid.BlockCell(agentBPosition);

            Position[] path = this.grid.GetPath(startPosition, goalPosition, MovementPatterns.LateralOnly);

            this.grid.UnblockCell(agentAPosition);
            this.grid.UnblockCell(agentBPosition);

            return path;
        }

        /// <summary>
        /// Get the actual cost of a path, taking into account the fact that a path with cost 0 is actually really expensive, 
        ///     because there is no possible path
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int GetActualPathCost(int count)
        {
            if (count == 0)
            {
                return MAX_COST;
            }
            else
            {
                return (int)Math.Pow(count, 2);
            }
        }

        protected int GetRandomInt(int inclBegin, int exclEnd, byte[] byteArray)
        {
            this.randomizer.GetBytes(byteArray);
            uint randomInt = BitConverter.ToUInt32(byteArray, 0);

            return (int)(inclBegin + (randomInt % (exclEnd - inclBegin + 1)));
        }
    }
}
