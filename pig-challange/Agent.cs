using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace pig_challenge
{
    class Agent : BasicAgent
    {
        private AgentIdentifier Identifier { get; }
        private float alpha, beta, gamma;

        public Agent(AgentIdentifier identifier, float alpha, float beta, float gamma)
        {
            this.Identifier = identifier;
            this.alpha = alpha;
            this.beta = beta;
            this.gamma = gamma;

            this.randomizer = new Random();
        }

        public override void DetermineStep(Map map, State state)
        {
            Position position = state.GetPosition(this.Identifier);

            //List<float> conditionalProbabilities = this.GetPMCConditionalPropabilities(map, state);

            IList<Position> path = map.GetPathToPig(this.Identifier, state);

            if (path.Count <= 2)
            {
                return;
            }

            Position newPos = new Position (path[1].X, path[1].Y );

            state.MoveAgent(this.Identifier, newPos);
        }


        public void DecideMove(Map map, State state)
        {
            float agentCooperationFactor = CalculateAgentCooperationFactor(map, state);  
            
            //determine whether we will actually cooperate or not

            //calculate what move we need to do considering our (non-/)cooperation
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
               

        public AgentIdentifier GetOtherAgentIdentifier(AgentIdentifier identifier)
        {
            if (identifier == AgentIdentifier.AgentA)
                return AgentIdentifier.AgentB;
            else
                return AgentIdentifier.AgentA;
        }
    }
}
