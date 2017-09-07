using System;
using System.Collections.Generic;

namespace CollaborativeFiltering
{
    public class PearsonMetric : IScoreMetric
    {
        public double Calculate(Dictionary<string, (double, double)> preferences)
        {
            // sum of all the preferences
            double sum1 = 0;
            double sum2 = 0;
            // sum of the squares
            double sumSq1 = 0;
            double sumSq2 = 0;
            // sum of the products
            double sumProducts = 0;

            foreach (var prefTuple in preferences.Values)
            {
                sum1 += prefTuple.Item1;
                sumSq1 += Math.Pow(prefTuple.Item1, 2);

                sum2 += prefTuple.Item2;
                sumSq2 += Math.Pow(prefTuple.Item2, 2);

                sumProducts += prefTuple.Item1 * prefTuple.Item2;
            }

            double numerator = sumProducts - (sum1 * sum2 / preferences.Count);
            double denominator = Math.Sqrt((sumSq1 - Math.Pow(sum1, 2) / preferences.Count) * (sumSq2 - Math.Pow(sum2, 2) / preferences.Count));
            if (denominator == 0)
                return 0;

            return numerator / denominator;
        }
    }
}
