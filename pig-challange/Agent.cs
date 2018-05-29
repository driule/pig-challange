﻿using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace pig_challenge
{
    class Agent : BasicAgent
    {
        private AgentIdentifier Identifier { get; }

        public Agent(AgentIdentifier identifier)
        {
            this.Identifier = identifier;
            this.randomizer = new Random();
        }

        public override void DetermineStep(Map map, State state)
        {
            int[] position = state.GetPosition(this.Identifier);

            IList<Position> path = map.GetPathToPig(this.Identifier, state);

            if (path.Count <= 2)
            {
                return;
            }

            int[] newPos = { path[1].X, path[1].Y };

            state.MoveAgent(this.Identifier, newPos);
        }
    }
}
