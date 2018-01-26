using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pure.DataMining;

namespace Pure.DataMining.UnitTests
{
    [TestClass]
    public class KMeansUnitTests
    {
        /// <summary>
        /// + + + o + + +
        /// + 1 1 o + 3 +
        /// + 1 + o + 3 3
        /// o o o o o o o
        /// + + + o + + +
        /// + + 2 2 2 + +
        /// + + + 2 + + +
        /// </summary>
        [TestMethod]
        public void TestKMeans()
        {
            var samples = new List<Tuple<double, double>>
            {
                new Tuple<double, double>(-2,  2),
                new Tuple<double, double>(-1,  2),
                new Tuple<double, double>(-1,  1),
                new Tuple<double, double>(-1, -2),
                new Tuple<double, double>( 0, -2),
                new Tuple<double, double>( 1, -2),
                new Tuple<double, double>( 0, -3),
                new Tuple<double, double>( 2,  2),
                new Tuple<double, double>( 2,  1),
                new Tuple<double, double>( 3,  1)
            };

            var clusters = new List<Tuple<double, double>>
            {
                samples[1],
                samples[2],
                samples[7]
            };

            var kmeansAlgo = new KMeans<Tuple<double, double>>(distanceCalculator, centerCalculator);
            var result = kmeansAlgo.Process(samples, clusters);

            for (int clusterIndex = 0; clusterIndex < result.Count; clusterIndex++)
            {
                Console.WriteLine("[Cluster {0}]", clusterIndex);

                foreach (var pointIndex in result[clusterIndex])
                {
                    Console.WriteLine("Index {0}: ({1}, {2})", pointIndex, samples[pointIndex].Item1, samples[pointIndex].Item2);
                }
            }

            // Check cluster count.
            Assert.AreEqual(3, result.Count);

            // Check first cluster.
            Assert.IsTrue(result[0].Contains(0));
            Assert.IsTrue(result[0].Contains(1));
            Assert.IsTrue(result[0].Contains(2));

            // Check second cluster.
            Assert.IsTrue(result[1].Contains(3));
            Assert.IsTrue(result[1].Contains(4));
            Assert.IsTrue(result[1].Contains(5));
            Assert.IsTrue(result[1].Contains(6));

            // Check third cluster.
            Assert.IsTrue(result[2].Contains(7));
            Assert.IsTrue(result[2].Contains(8));
            Assert.IsTrue(result[2].Contains(9));
        }

        private double distanceCalculator(Tuple<double, double> point1, Tuple<double, double> point2)
        {
            double part1 = Math.Pow(point1.Item1 - point2.Item1, 2);
            double part2 = Math.Pow(point1.Item2 - point2.Item2, 2);
            return Math.Sqrt(part1 + part2);
        }

        private Tuple<double, double> centerCalculator(IEnumerable<Tuple<double, double>> points)
        {
            double avg1 = points.Average(o => o.Item1);
            double avg2 = points.Average(o => o.Item2);
            return new Tuple<double, double>(avg1, avg2);
        }
    }
}
