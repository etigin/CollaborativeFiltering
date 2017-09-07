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

            var score = SimilarityScoreCalculator.Calculate(lizaPref, genePref, new EuclideanDistanceMetric());
            score.Should().BeApproximately(0.294, 0.001);
        }
    }
}
