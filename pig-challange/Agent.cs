using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using RoyT.AStar;

namespace pig_challenge
{
    class Agent : BasicAgent
    {
        private AgentIdentifier Identifier { get; }
        private float alpha, beta, gamma;

        public Agent(AgentIdentifier identifier, float alpha, float beta, float gamma) : base()
        {
            this.Identifier = identifier;
            this.alpha = alpha;
            this.beta = beta;
            this.gamma = gamma;
        }

        public override void DetermineStep(Map map, State state)
        {
            Position position = state.GetPosition(this.Identifier);


            //IList<Position> path = map.GetPathToPig(this.Identifier, state);

            //if (path.Count <= 2)
            //{
            //    return;
            //}

            //Position newPos = new Position(path[1].X, path[1].Y);

            Position newPos = this.DecideMove(map, state);

            //Position newPos = this.DecideMove(map, state);
            state.MoveAgent(this.Identifier, newPos);
        }


        public Position DecideMove(Map map, State state)
        {
            float agentCooperationFactor = CalculateAgentCooperationFactor(map, state);

            //determine whether we will actually cooperate or not and 
            //  calculate what move we need to do considering our (non-/)cooperation

            Position newPos;
            if (agentCooperationFactor > 0.75f)
            {
                newPos = GetCooperationMove(map, state);
            }
            else if (agentCooperationFactor < 0.25f)
            {
                newPos = GetDefectMove(map, state);
            }
            else if (agentCooperationFactor > this.GetRandomDouble())
            {
                newPos = GetCooperationMove(map, state);
            }
            else
            {
                newPos = GetDefectMove(map, state);
            }


            //Position newPos;
            //if (agentCooperationFactor > this.GetRandomDouble())
            //    newPos = GetCooperationMove(map, state);
            //else
            //    newPos = GetDefectMove(map, state);

            return newPos;
        }

        private float CalculateAgentCooperationFactor(Map map, State state)
        {
            AgentIdentifier otherAgentID = this.GetOtherAgentIdentifier(this.Identifier);

            int distanceAToPig = map.GetPathToPig(AgentIdentifier.AgentA, state).Count();
            int distanceBToPig = map.GetPathToPig(AgentIdentifier.AgentB, state).Count();


            float agentCooperationFactor = 0.0f;

            float distanceRatio = (1.0f - ((float)(distanceAToPig + distanceBToPig) / (2.0f * map.maxDistanceToPig)));
            if (distanceRatio < 0.0f)
                throw new Exception("distance ratio too big, probably maxDistance isn't the actual max");
            agentCooperationFactor += this.alpha * distanceRatio;

            agentCooperationFactor += this.beta * state.GetCooperationProbability(otherAgentID);

            agentCooperationFactor += this.gamma * (float)state.GetScore(otherAgentID) / 25.0f;

            return agentCooperationFactor;
        }

        public Tuple<Position, int> GetBestMoveToGoalPosition(Map map, State state, Position goalPosition)
        {
            Position position = state.GetPosition(this.Identifier);

            List<Position> availablePositions = map.GetAvailablePositions(position, state);

            //This calculates the amount of steps until the position of the pig is reached, so we could subtract 1 to get to the position just before the pig
            //Basically, I actually wanted to calculate from the availablePosition to next to the pig, and then add 1 tot the total,
            //  but the GetPath method actually includes the startPosition in the path, so the +1 is not needed.
            List<int> costs = availablePositions
                                    .Select(availablePosition => 
                                                map.GetActualPathCost(
                                                    map.GetPathToGoalPositionFromStartPosition(state, availablePosition, goalPosition)
                                                            .Count))
                                    .ToList();

            //Now sort the available positions by the cost of the path from this position and then choose the smallest cost: orderedZip[0]
            List<Tuple<Position, int>> orderedZip = availablePositions.Zip(costs, 
                                                                            (x, y) => new Tuple<Position, int>( x, y ))
                                    .OrderBy(pair => pair.Item2)
                                    .ToList();

            //If you're already next to the goal, don't move. 
            //  A cost of 1 means the path contains 1 element, which is the case if you're already next to the goal
            return orderedZip[0];
        }

        private Position GetCooperationMove(Map map, State state)
        {
            Tuple<Position, int> posCostTuple = this.GetBestMoveToGoalPosition(map, state, state.GetPosition(AgentIdentifier.Pig));
            if (posCostTuple.Item2 <= 1)
                return state.GetPosition(this.Identifier);
            else
                return posCostTuple.Item1;
        }

        private Position GetDefectMove(Map map, State state)
        {
            //Exits at (x == 1 && y == 4) || (x == 7 && y == 4)
            Position exitPos1 = new Position(1, 4);
            Position exitPos2 = new Position(7, 4);

            Tuple<Position, int> move1 = GetBestMoveToGoalPosition(map, state, exitPos1);
            Tuple<Position, int> move2 = GetBestMoveToGoalPosition(map, state, exitPos2);

            if (move1.Item2 < move2.Item2)
                return move1.Item1;
            else
                return move2.Item1;
        }


        public AgentIdentifier GetOtherAgentIdentifier(AgentIdentifier identifier)
        {
            if (identifier == AgentIdentifier.AgentA)
                return AgentIdentifier.AgentB;
            else
                return AgentIdentifier.AgentA;
        }
    }
}
