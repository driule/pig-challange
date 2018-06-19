using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using RoyT.AStar;

namespace pig_challenge
{
    struct AgentConfiguration
    {
        public float alpha, beta, gamma, delta;

        // minimum cooperation probability limit when agent cooperates for sure
        public float minCooperationLimit;

        // maximum cooperation probability limit when agent defects for sure
        public float maxDefectLimit;
    }

    class Agent : BasicAgent
    {
        private AgentIdentifier Identifier { get; }
        private AgentConfiguration configuration;

        public Agent(AgentIdentifier identifier, AgentConfiguration configuration) : base()
        {
            this.Identifier = identifier;
            this.configuration = configuration;
        }

        // determine whether we will actually cooperate or not and 
        // calculate what move we need to do considering our (non-/)cooperation
        public override void DetermineStep(Map map, State state)
        {
            float agentCooperationFactor = this.CalculateAgentCooperationFactor(map, state);
            
            Position newAgentPosition;
            if (agentCooperationFactor > this.configuration.minCooperationLimit)
            {
                newAgentPosition = GetCooperationMove(map, state);
            }
            else if (agentCooperationFactor < this.configuration.maxDefectLimit)
            {
                newAgentPosition = GetDefectMove(map, state);
            }
            else if (agentCooperationFactor > this.GetRandomDouble())
            {
                newAgentPosition = GetCooperationMove(map, state);
            }
            else
            {
                newAgentPosition = GetDefectMove(map, state);
            }

            state.MoveAgent(this.Identifier, newAgentPosition);
        }

        private float CalculateAgentCooperationFactor(Map map, State state)
        {
            AgentIdentifier otherAgentId = this.GetOtherAgentIdentifier(this.Identifier);

            int distanceAToPig = map.GetPathToPig(AgentIdentifier.AgentA, state).Count();
            int distanceBToPig = map.GetPathToPig(AgentIdentifier.AgentB, state).Count();

            float agentCooperationFactor = 0.0f;

            float distanceRatio = (1.0f - ((float)(distanceAToPig + distanceBToPig) / (2.0f * map.maxDistanceToPig)));
            if (distanceRatio < 0.0f)
            {
                throw new Exception("Distance ratio too big, probably maxDistance isn't the actual max");
            }

            agentCooperationFactor += this.configuration.alpha * distanceRatio;
            agentCooperationFactor += this.configuration.beta * state.GetAgentCooperationProbability(otherAgentId);
            agentCooperationFactor += this.configuration.gamma * (float)state.GetAgentScore(otherAgentId) / 25.0f;
            agentCooperationFactor += this.configuration.delta;

            return agentCooperationFactor;
        }

        public Tuple<Position, int> GetBestMoveToGoalPosition(Map map, State state, Position goalPosition)
        {
            Position currentAgentPosition = state.GetAgentPosition(this.Identifier);

            List<Position> availablePositions = map.GetAvailablePositions(currentAgentPosition, state);

            // this calculates the amount of steps until the position of the pig is reached, so we could subtract 1 to get to the position just before the pig
            // basically, I actually wanted to calculate from the availablePosition to next to the pig, and then add 1 tot the total,
            // but the GetPath method actually includes the startPosition in the path, so the +1 is not needed.
            List<int> costs = availablePositions.Select(
                availablePosition => map.GetActualPathCost(
                    map.GetPathToGoalPositionFromStartPosition(state, availablePosition, goalPosition).Count
                )
            ).ToList();

            // now sort the available positions by the cost of the path from this position and then choose the smallest cost: orderedZip[0]
            List<Tuple<Position, int>> positions = availablePositions.Zip(
                costs,
                (x, y) => new Tuple<Position, int>(x, y)
            ).OrderBy(
                pair => pair.Item2
            ).ToList();

            // if agent is already next to the goal, don't move. 
            // a cost of 1 means the path contains 1 element, which is the case if you're already next to the goal
            return positions[0];
        }

        private Position GetCooperationMove(Map map, State state)
        {
            Tuple<Position, int> positionWithCost = this.GetBestMoveToGoalPosition(map, state, state.GetAgentPosition(AgentIdentifier.Pig));

            if (positionWithCost.Item2 <= 1)
            {
                return state.GetAgentPosition(this.Identifier);
            }
            else
            {
                return positionWithCost.Item1;
            }
        }

        private Position GetDefectMove(Map map, State state)
        {
            // exits at (x == 1 && y == 4) || (x == 7 && y == 4)
            Position exitPosition1 = new Position(1, 4);
            Position exitPosition2 = new Position(7, 4);

            Tuple<Position, int> move1 = GetBestMoveToGoalPosition(map, state, exitPosition1);
            Tuple<Position, int> move2 = GetBestMoveToGoalPosition(map, state, exitPosition2);

            if (move1.Item2 < move2.Item2)
            {
                return move1.Item1;
            }
            else
            {
                return move2.Item1;
            }
        }

        public AgentIdentifier GetOtherAgentIdentifier(AgentIdentifier identifier)
        {
            if (identifier == AgentIdentifier.AgentA)
            {
                return AgentIdentifier.AgentB;
            }
            else
            {
                return AgentIdentifier.AgentA;
            }
        }
    }
}
