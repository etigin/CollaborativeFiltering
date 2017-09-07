using System.Collections.Generic;

namespace CollaborativeFiltering
{
    public interface IScoreMetric
    {
         double Calculate(Dictionary<string, (double, double)> preferences);
    }
}
