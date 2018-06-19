using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace pig_challenge
{
    class Program
    {
        const int NUM_GAMES = 10;
        const int MAX_ITERATIONS = 25;

        static void Main(string[] args)
        {
            float[,] floatArray = new float[81796, 8];
            FillConfigurations(floatArray);

            Console.WriteLine("Welcome to the pig challenge!");
            Console.WriteLine("Press Enter to start a new tournament.");

            RunForRandomConfigurations(floatArray);

            Console.ReadLine();
            
        }

        static void FillConfigurations(float[,] floatArray)
        {
            int step = 10;
            int count = 0;
            for (int alphaA = 0; alphaA <= 100; alphaA += step)
                for (int betaA = 0; betaA <= 100; betaA += step)
                    for (int gammaA = 0; gammaA <= 100; gammaA += step)
                        for (int deltaA = 0; deltaA <= 100; deltaA += step)
                        {
                            if (alphaA + betaA + gammaA + deltaA != 100)
                                continue;
                            for (int alphaB = 0; alphaB <= 100; alphaB += step)
                                for (int betaB = 0; betaB <= 100; betaB += step)
                                    for (int gammaB = 0; gammaB <= 100; gammaB += step)
                                        for (int deltaB = 0; deltaB <= 100; deltaB += step)
                                        {
                                            if (alphaB + betaB + gammaB + deltaB != 100)
                                                continue;

                                            floatArray[count, 0] = alphaA / 100.0f;
                                            floatArray[count, 1] = betaA / 100.0f;
                                            floatArray[count, 2] = gammaA / 100.0f;
                                            floatArray[count, 3] = deltaA / 100.0f;

                                            floatArray[count, 4] = alphaB / 100.0f;
                                            floatArray[count, 5] = betaB / 100.0f;
                                            floatArray[count, 6] = gammaB / 100.0f;
                                            floatArray[count, 7] = deltaB / 100.0f;

                                            count++;
                                        }
                        }
        }

        static void RunForRandomConfigurations(float[,] floatArray)
        {
            Result[] resultArray = new Result[81796];
            var csv = new StringBuilder();


            //Stopwatch stopwatch = new Stopwatch();

            int count = 0;
            Parallel.For(0, 4, id =>
            {

                for (int i = id * 20449; i < (id + 1) * 20449; i++)
                {
                    count++;
                    if (count % 100 == 0)
                        Console.WriteLine($"Iteration {count}");
                    //stopwatch.Reset();
                    //stopwatch.Start();


                    AgentConfiguration agentConfigurationA = new AgentConfiguration { alpha = floatArray[i, 0], beta = floatArray[i, 1], gamma = floatArray[i, 2], delta = floatArray[i, 3], minCooperationLimit = 0.75f, maxDefectLimit = 0.25f };
                    AgentConfiguration agentConfigurationB = new AgentConfiguration { alpha = floatArray[i, 4], beta = floatArray[i, 5], gamma = floatArray[i, 6], delta = floatArray[i, 7], minCooperationLimit = 0.75f, maxDefectLimit = 0.25f };

                    Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);
                    csv.AppendLine($"Configuration A: {floatArray[i, 0]}, {floatArray[i, 1]}, {floatArray[i, 2]}, {floatArray[i, 3]} \n " +
                        $"Configuration B: {floatArray[i, 4]}, {floatArray[i, 5]}, {floatArray[i, 6]}, {floatArray[i, 7]} ");
                    Result res = tournament.Run(agentConfigurationA, agentConfigurationB);
                    csv.AppendLine($"Total scores: \n Agent A: {res.scoreA}, Agent B: {res.scoreB}");

                    resultArray[i] = res; ;

                    //stopwatch.Stop();
                    //Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks +
                    //            " mS: " + stopwatch.ElapsedMilliseconds);
                    //Console.ReadLine();
                }

            });
            File.WriteAllText("./results.csv", csv.ToString());


        }
    }
}
