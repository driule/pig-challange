using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace pig_challenge
{
    class Map
    {
        public enum CellType
        {
            Empty = 0,
            Obstacle = 1,
            Exit = 2
        }

        public float maxDistanceToPig = 9.0f;
        // Create a new grid and let each cell have a default traversal cost of 1.0
        private Grid grid;

        private Random randomizer;

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
            this.grid.UnblockCell(new Position(1, 4));
            this.grid.UnblockCell(new Position(7, 4));

            // obstacles
            this.grid.BlockCell(new Position(3, 3));
            this.grid.BlockCell(new Position(3, 5));
            this.grid.BlockCell(new Position(5, 3));
            this.grid.BlockCell(new Position(5, 5));

            this.randomizer = new Random();
        }

        public bool IsCellExit(int x, int y)
        {
            if((x == 1 && y == 4) || (x == 7 && y == 4))
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
            int x = 0, y = 0;
            while (true)
            {
                x = this.randomizer.Next(2, 6);
                y = this.randomizer.Next(2, 6);

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
            Console.WriteLine($"Iteration {iteration}");
            for (int y = 0; y < 9; y++)
            {
                Console.Write("|");
                for (int x = 0; x < 9; x++)
                {
                    Position position = new Position(x, y);
                    if (state.PositionAgentA.Equals(position))
                    {
                        Console.Write("A");
                    }
                    else if (state.PositionAgentB.Equals(position))
                    {
                        Console.Write("B");
                    }
                    else if (state.PositionPig.Equals(position))
                    {
                        Console.Write("o");
                    }
                    else if (!float.IsInfinity(this.grid.GetCellCost(position)))
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("X");
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            //state.Print();
        }

        // returns a list of all available positions in the agent's vacinity
        // TODO: possible just use a deterministic list of available positions (there are few in total)
        public List<Position> GetAvailablePositions(Position position, State state)
        {
            List<Position> availablePositions = new List<Position>
            {
                new Position( position.X - 1, position.Y),
                new Position( position.X, position.Y - 1),
                new Position( position.X + 1, position.Y),
                new Position( position.X, position.Y + 1)
            };

            return availablePositions.Where(pos =>
            {
                int x = pos.X;
                int y = pos.Y;
                return x >= 0 && y >= 0 && x < 9 && y < 9 && this.IsCellEmpty(x, y, state);
            }).ToList();
        }

        public IList<Position> GetPathToPig(BasicAgent.AgentIdentifier agentId, State state)
        {
            Position currentAgentPosition = state.GetPosition(agentId); 
            Position agentAPosition = state.GetPosition(BasicAgent.AgentIdentifier.AgentA);
            Position agentBPosition = state.GetPosition(BasicAgent.AgentIdentifier.AgentB);
            Position pigPosition = state.GetPosition(BasicAgent.AgentIdentifier.Pig);

            this.grid.BlockCell(agentAPosition);
            this.grid.BlockCell(agentBPosition);

            Position[] path = this.grid.GetPath(currentAgentPosition, pigPosition, MovementPatterns.LateralOnly);

            this.grid.UnblockCell(agentAPosition);
            this.grid.UnblockCell(agentBPosition);

            return path;
        }

        public IList<Position> GetPathToPigFromPosition(State state, Position position)
        {
            Position agentAPosition = state.GetPosition(BasicAgent.AgentIdentifier.AgentA);
            Position agentBPosition = state.GetPosition(BasicAgent.AgentIdentifier.AgentB);
            Position pigPosition = state.GetPosition(BasicAgent.AgentIdentifier.Pig);

            this.grid.BlockCell(agentAPosition);
            this.grid.BlockCell(agentBPosition);

            Position[] path = this.grid.GetPath(position, pigPosition, MovementPatterns.LateralOnly);

            this.grid.UnblockCell(agentAPosition);
            this.grid.UnblockCell(agentBPosition);

            return path;
        }
    }
}
