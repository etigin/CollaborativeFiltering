using System;
using System.Collections.Generic;
using System.Linq;

namespace CollaborativeFiltering
{
    public class SimilarityScoreCalculator
    {
        public static double CalculateEuclideanDistance(Dictionary<string, double> firstPref, Dictionary<string, double> secondPref)
        {
            var common = firstPref.Keys.Intersect(secondPref.Keys).ToList();
            if (common.Count == 0)
                return 0;

            double sumOfSquares = 0;
            foreach (var key in common)
            {
                sumOfSquares += Math.Pow(firstPref[key] - secondPref[key], 2);
            }

            return 1 / (1 + Math.Sqrt(sumOfSquares));
        }
    }
}
