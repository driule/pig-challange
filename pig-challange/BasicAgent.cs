using System;
using System.Security.Cryptography;

namespace pig_challenge
{
    abstract class BasicAgent
    {
        public enum AgentIdentifier
        {
            AgentA = 0,
            AgentB = 1,
            Pig = 2
        }

        private RNGCryptoServiceProvider randomizer;

        public BasicAgent()
        {
            this.randomizer = new RNGCryptoServiceProvider();
        }

        abstract public void DetermineStep(Map map, State state);

        protected int GetRandomInt(int inclBegin, int exclEnd)
        {
            byte[] byteArray = new byte[4];
            this.randomizer.GetBytes(byteArray);
            uint randomInt = BitConverter.ToUInt32(byteArray, 0);

            return (int)(inclBegin + (randomInt % (exclEnd - inclBegin + 1)));
        }

        protected double GetRandomDouble()
        {
            byte[] byteArray = new byte[8];
            this.randomizer.GetBytes(byteArray);
            UInt64 randomInt = BitConverter.ToUInt64(byteArray, 0) / (1 << 11);

            return randomInt / (double)(1UL << 53);
        }
    }
}
