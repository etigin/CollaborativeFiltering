using System;
using System.Collections.Generic;

namespace CollaborativeFiltering
{
    public class EuclideanDistanceMetric : IScoreMetric
    {
        public double Calculate(Dictionary<string, (double, double)> preferences)
        {
            double sumOfSquares = 0;
            foreach (var prefTuple in preferences.Values)
            {
                sumOfSquares += Math.Pow(prefTuple.Item1 - prefTuple.Item2, 2);
            }

            return 1 / (1 + Math.Sqrt(sumOfSquares));
        }
    }
}
