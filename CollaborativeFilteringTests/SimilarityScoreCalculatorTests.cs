using System;
using System.Collections.Generic;
using System.IO;
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

            var score = SimilarityScoreCalculator.CalculateEuclideanDistance(lizaPref, genePref);
            score.Should().BeApproximately(0.294, 0.001);
        }
    }
}
