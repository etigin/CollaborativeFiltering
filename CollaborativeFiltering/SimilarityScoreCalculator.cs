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

        public static IEnumerable<KeyValuePair<string, double>> GetUsersWithSimilarTaste(Dictionary<string, Dictionary<string, double>> preferences,
            string personName, IScoreMetric metric, int usersCount)
        {
            return GetTopMatches(preferences, personName, metric, usersCount);
        }

        public static IEnumerable<KeyValuePair<string, double>> GetSimilarItems(Dictionary<string, Dictionary<string, double>> preferences,
            string itemName, IScoreMetric metric, int itemsCount)
        {
            var transformedPrefs = new Dictionary<string, Dictionary<string, double>>();
            foreach (var prefPair in preferences)
            {
                foreach (var item in prefPair.Value)
                {
                    if (!transformedPrefs.ContainsKey(item.Key))
                    {
                        transformedPrefs[item.Key] = new Dictionary<string, double>();
                    }

                    transformedPrefs[item.Key].Add(prefPair.Key, item.Value);
                }
            }

            return GetTopMatches(transformedPrefs, itemName, metric, itemsCount);
        }

        public static IEnumerable<KeyValuePair<string, double>> GetRecommendations(Dictionary<string, Dictionary<string, double>> preferences,
            string personName, IScoreMetric metric, int usersCount)
        {
            var totals = new Dictionary<string, double>();
            var scoresSums = new Dictionary<string, double>();
            var personScores = preferences[personName];

            foreach (var prefPair in preferences)
            {
                if (prefPair.Key == personName)
                    continue;

                double score = Calculate(personScores, prefPair.Value, metric);
                if (score <= 0)
                    continue;

                foreach (var movie in prefPair.Value)
                {
                    if (!personScores.ContainsKey(movie.Key))
                    {
                        double total = 0.0;
                        totals.TryGetValue(movie.Key, out total);
                        totals[movie.Key] = total + movie.Value * score;

                        double scoreSum;
                        scoresSums.TryGetValue(movie.Key, out scoreSum);
                        scoresSums[movie.Key] = scoreSum + score;
                    }
                }
            }

            var result = new List<KeyValuePair<string, double>>();
            foreach (var item in totals)
            {
                result.Add(new KeyValuePair<string, double>(item.Key, item.Value / scoresSums[item.Key]));
            }

            return result.OrderByDescending(x => x.Value).Take(usersCount);
        }

        private static IEnumerable<KeyValuePair<string, double>> GetTopMatches(Dictionary<string, Dictionary<string, double>> preferences,
            string prefKey, IScoreMetric metric, int count)
        {
            var scores = new Dictionary<string, double>();
            foreach (var prefPair in preferences)
            {
                if (prefPair.Key == prefKey)
                    continue;

                double score = Calculate(preferences[prefKey], prefPair.Value, metric);
                scores.Add(prefPair.Key, score);
            }

            var result = new List<KeyValuePair<string, double>>(count);
            foreach (var t in scores.OrderByDescending(x => x.Value).Take(count))
            {
                result.Add(t);
            }

            return result;
        }
    }
}
