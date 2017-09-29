using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using CollaborativeFiltering;

namespace CollaborativeFilteringTests
{
    public class SimilarityScoreCalculatorTests
    {
        [Fact]
        public void EuclideanDistanceMetricTest()
        {
            var data = DataReader.ReadFileContent("SampleData//critics.json");
            var dict = DataReader.DeserializeData<Dictionary<string, Dictionary<string, double>>>(data);
            var lizaPref = dict["Lisa Rose"];
            var genePref = dict["Gene Seymour"];

            var score = SimilarityScoreCalculator.Calculate(lizaPref, genePref, new EuclideanDistanceMetric());
            score.Should().BeApproximately(0.294, 0.001);
        }

        [Fact]
        public void PearsonDistanceMetricTest()
        {
            var data = DataReader.ReadFileContent("SampleData//critics.json");
            var dict = DataReader.DeserializeData<Dictionary<string, Dictionary<string, double>>>(data);
            var lizaPref = dict["Lisa Rose"];
            var genePref = dict["Gene Seymour"];

            var score = SimilarityScoreCalculator.Calculate(lizaPref, genePref, new PearsonMetric());
            score.Should().BeApproximately(0.396, 0.001);
        }

        [Fact]
        public void UsersWithSimilarTasteTest()
        {
            var data = DataReader.ReadFileContent("SampleData//critics.json");
            var dict = DataReader.DeserializeData<Dictionary<string, Dictionary<string, double>>>(data);

            var top3 = SimilarityScoreCalculator.GetUsersWithSimilarTaste(dict, "Toby", new PearsonMetric(), 3).ToList();

            top3[0].Key.Should().Be("Lisa Rose");
            top3[1].Key.Should().Be("Mick LaSalle");
            top3[2].Key.Should().Be("Claudia Puig");

            top3[0].Value.Should().BeApproximately(0.991, 0.001);
            top3[1].Value.Should().BeApproximately(0.924, 0.001);
            top3[2].Value.Should().BeApproximately(0.893, 0.001);
        }

        [Fact]
        public void RecommendationsTest()
        {
            var data = DataReader.ReadFileContent("SampleData//critics.json");
            var dict = DataReader.DeserializeData<Dictionary<string, Dictionary<string, double>>>(data);

            var top3 = SimilarityScoreCalculator.GetRecommendations(dict, "Toby", new PearsonMetric(), 3).ToList();

            top3[0].Key.Should().Be("The Night Listener");
            top3[1].Key.Should().Be("Lady in the Water");
            top3[2].Key.Should().Be("Just My Luck");

            top3[0].Value.Should().BeApproximately(3.347, 0.001);
            top3[1].Value.Should().BeApproximately(2.832, 0.001);
            top3[2].Value.Should().BeApproximately(2.530, 0.001);
        }
    }
}
