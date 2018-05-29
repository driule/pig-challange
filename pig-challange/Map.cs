using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoyT.AStar;

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

        // Create a new grid and let each cell have a default traversal cost of 1.0
        Grid grid = new Grid(9, 9, 1.0f);

        public Map()
        {
            this.Grid = new CellType[9, 9];
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (y == 0 || x == 0 || y == 8 || x == 8 || y == 1 || x == 1 || y == 7 || x == 7)
                    {
                        this.Grid[x, y] = CellType.Obstacle;

                        // Block some cells (for example walls)
                        grid.BlockCell(new Position(x, y));
                    }
                    else
                    {
                        this.Grid[x, y] = CellType.Empty;
                    }
                }
            }

            // exits
            this.Grid[1, 4] = CellType.Exit;
            this.Grid[7, 4] = CellType.Exit;

            // obstacles
            this.Grid[3, 3] = CellType.Obstacle;
            this.Grid[3, 5] = CellType.Obstacle;
            this.Grid[5, 3] = CellType.Obstacle;
            this.Grid[5, 5] = CellType.Obstacle;

            grid.BlockCell(new Position(3, 3));
            grid.BlockCell(new Position(3, 5));
            grid.BlockCell(new Position(5, 3));
            grid.BlockCell(new Position(5, 5));
        }

        public bool IsCellExit(int x, int y)
        {
            if (this.Grid[x, y] == CellType.Exit)
            {
                return true;
            }

            return false;
        }

        public bool IsCellEmpty(int x, int y, State state)
        {
            int[] position = new int[] { x, y };

            // check for obstacles
            if (this.Grid[position[0], position[1]] == CellType.Obstacle)
            {
                return false;
            }

            // check for agents and pig
            if (state.PositionAgentA.SequenceEqual(position))
            {
                return false;
            }

            if (state.PositionAgentB.SequenceEqual(position))
            {
                return false;
            }

            if (state.PositionPig.SequenceEqual(position))
            {
                return false;
            }

            return true;
        }

        public void Draw(int iteration, State state)
        {
            Console.WriteLine($"Iteration {iteration}");
            for (int y = 0; y < 9; y++)
            {
                Console.Write("|");
                for (int x = 0; x < 9; x++)
                {
                    int[] position = new int[] { x, y };
                    if (state.PositionAgentA.SequenceEqual(position))
                    {
                        Console.Write("A");
                    }
                    else if (state.PositionAgentB.SequenceEqual(position))
                    {
                        Console.Write("B");
                    }
                    else if (state.PositionPig.SequenceEqual(position))
                    {
                        Console.Write("o");
                    }
                    else if (this.Grid[x, y] == CellType.Empty || this.Grid[x, y] == CellType.Exit)
                    {
                        Console.Write(" ");
                    }
                    else if (this.Grid[x, y] == CellType.Obstacle)
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
        public List<int[]> GetAvailablePositions(int[] position, State state)
        {
            List<int[]> availablePositions = new List<int[]>
            {
                new int[]{ position[0] - 1, position[1]},
                new int[]{ position[0], position[1] - 1},
                new int[]{ position[0] + 1, position[1]},
                new int[]{ position[0], position[1] + 1}
            };

            return availablePositions.Where(index =>
            {
                int x = index[0];
                int y = index[1];
                return x >= 0 && y >= 0 && x < 9 && y < 9 && this.IsCellEmpty(x, y, state);
            }).ToList();
        }

        public IList<Position> GetPathToPig(BasicAgent.AgentIdentifier agentId, State state)
        {
            Position[] positions;
            int[] agentPos, otherAgentPos, pigAgentPos;
            agentPos = state.GetPosition(agentId);
            otherAgentPos = state.GetPosition((BasicAgent.AgentIdentifier)(1 - (int)agentId));
            pigAgentPos = state.GetPosition(BasicAgent.AgentIdentifier.Pig);

            Position pos, otherPos, pigPos;
            pos = new Position(agentPos[0], agentPos[1]);
            otherPos = new Position(otherAgentPos[0], otherAgentPos[1]);
            pigPos = new Position(pigAgentPos[0], pigAgentPos[1]);

            grid.BlockCell(otherPos);
            positions = grid.GetPath(pos, pigPos, MovementPatterns.LateralOnly);
            grid.UnblockCell(otherPos);
            return positions;
        }
    }
}
