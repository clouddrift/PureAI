using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public static class NumericCalculation
    {
        public static double Entropy(IEnumerable<double> probalities)
        {
            return probalities.Aggregate(0.0, (total, next) => total - next * Math.Log(next, 2));
        }

        public static double EuclideanDistance(IEnumerable<double> first, IEnumerable<double> second)
        {
            return first.Zip(second, (o1, o2) => Math.Pow(o1 - o2, 2)).Sum();
        }

        public static double ManhattanDistance(IEnumerable<double> first, IEnumerable<double> second)
        {
            return first.Zip(second, (o1, o2) => Math.Abs(o1 - o2)).Sum();
        }

        public static double ChebyshevDistance(IEnumerable<double> first, IEnumerable<double> second)
        {
            return first.Zip(second, (o1, o2) => Math.Abs(o1 - o2)).Max();
        }

        public static double MinkowskiDistance(IEnumerable<double> first, IEnumerable<double> second, double p)
        {
            var sum = first.Zip(second, (o1, o2) => Math.Pow(Math.Abs(o1 - o2), p)).Sum();
            return Math.Pow(sum, 1 / p);
        }
    }
}
