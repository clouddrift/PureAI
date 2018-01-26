using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pure.DataMining;

namespace Pure.DataMining.UnitTests
{
    [TestClass]
    public class AprioriUnitTests
    {
        [TestMethod]
        public void TestApriori()
        {
            List<List<int>> samples = new List<List<int>>
            {
                new List<int> { 1, 2, 5 },
                new List<int> { 2, 4 },
                new List<int> { 2, 3 },
                new List<int> { 1, 2, 4 },
                new List<int> { 1, 3 },
                new List<int> { 2, 3 },
                new List<int> { 1, 3 },
                new List<int> { 1, 2, 3, 5 },
                new List<int> { 1, 2, 3 }
            };

            Apriori<int> apriori = new Apriori<int>(2);
            apriori.Initialize(samples);

            apriori.NextGeneration(1);
            Assert.AreEqual(5, apriori.Result.Count);
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 1 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 2 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 3 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 4 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 5 })));

            apriori.NextGeneration(2);
            Assert.AreEqual(6, apriori.Result.Count);
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 1, 2 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 1, 3 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 1, 5 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 2, 3 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 2, 4 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 2, 5 })));

            apriori.NextGeneration(3);
            Assert.AreEqual(2, apriori.Result.Count);
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 1, 2, 3 })));
            Assert.IsTrue(apriori.Result.Any(o => o.SetEquals(new HashSet<int>() { 1, 2, 5 })));

            apriori.NextGeneration(4);
            Assert.AreEqual(0, apriori.Result.Count);
        }

        private void Entropy()
        {
            double[] values = { 9.0 / 14, 5.0 / 14 };
            double result = NumericCalculation.Entropy(values);
            Console.WriteLine(result);
        }
    }
}
