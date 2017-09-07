using System;
using System.Collections.Generic;
using System.Linq;

namespace CollaborativeFiltering
{
    public class SimilarityScoreCalculator
    {
        public static double Calculate(Dictionary<string, double> firstPref, Dictionary<string, double> secondPref, IScoreMetric metric)
        {
            var common = firstPref.Keys.Intersect(secondPref.Keys).ToList();
            if (common.Count == 0)
                return 0;

            var prefs = new Dictionary<string, (double, double)>();
            foreach (var key in common)
            {
                prefs.Add(key, (firstPref[key], secondPref[key]));
            }

            return metric.Calculate(prefs);
        }
    }
}
