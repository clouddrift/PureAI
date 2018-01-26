using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pure.DataMining;

namespace Pure.DataMining.UnitTests
{
    [TestClass]
    public class KNNUnitTests
    {
        [TestMethod]
        public void TestKNN()
        {
            var samples = new List<IList<object>>
            {
                new List<object> { 1.5, 40.0, "Thin" },
                new List<object> { 1.5, 50.0, "Fat" },
                new List<object> { 1.5, 60.0, "Fat" },
                new List<object> { 1.6, 40.0, "Thin" },
                new List<object> { 1.6, 50.0, "Thin" },
                new List<object> { 1.6, 60.0, "Fat" },
                new List<object> { 1.6, 70.0, "Fat" },
                new List<object> { 1.7, 50.0, "Thin" },
                new List<object> { 1.7, 60.0, "Thin" },
                new List<object> { 1.7, 70.0, "Fat" },
                new List<object> { 1.7, 80.0, "Fat" },
                new List<object> { 1.8, 60.0, "Thin" },
                new List<object> { 1.8, 70.0, "Thin" },
                new List<object> { 1.8, 80.0, "Fat" },
                new List<object> { 1.8, 90.0, "Fat" },
                new List<object> { 1.9, 80.0, "Thin" },
                new List<object> { 1.9, 90.0, "Fat" }
            };

            KNN<double, string> algo = new KNN<double, string>(NumericCalculation.EuclideanDistance);

            // Height range: [1.5 - 2.0]
            algo.AddNormalizer(o => (o - 1.5) * 2.0);

            // Weight range: [20.0 - 100.0]
            algo.AddNormalizer(o => (o - 20.0) * 0.0125);

            // Train Data
            foreach (var sample in samples)
            {
                algo.AddTrainingData(sample.Take(2).Cast<double>(), sample[2] as string);
            }

            // Test Data
            var sameCount = samples.Count(o =>
            {
                var properties = o.Take(2).Cast<double>();
                var result = algo.Perform(properties, 5);
                return o[2].Equals(result);
            });

            Assert.IsTrue(sameCount * 1.0 / samples.Count > 0.88);
        }
    }
}
