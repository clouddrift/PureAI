using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pure.DataMining;

namespace Pure.DataMining.UnitTests
{
    [TestClass]
    public class C45UnitTests
    {
        [TestMethod]
        public void TestC45()
        {
            var samples = new List<IList<object>>
            {
                new List<object> { 1, "Sunny", 85, 85, false, false },
                new List<object> { 2, "Sunny", 80, 90, true, false },
                new List<object> { 3, "Overcast", 83, 78, false, true },
                new List<object> { 4, "Rainy", 70, 96, false, true },
                new List<object> { 5, "Rainy", 68, 80, false, true },
                new List<object> { 6, "Rainy", 65, 70, true, false },
                new List<object> { 7, "Overcast", 64, 65, true, true },
                new List<object> { 8, "Sunny", 72, 95, false, false },
                new List<object> { 9, "Sunny", 69, 70, false, true },
                new List<object> { 10, "Rainy", 75, 80, false, true },
                new List<object> { 11, "Sunny", 75, 70, true, true },
                new List<object> { 12, "Overcast", 73, 90, true, true },
                new List<object> { 13, "Overcast", 81, 75, false, true },
                new List<object> { 14, "Rainy", 71, 80, true, false },
            };

            var optionIndexes = new List<int> { 1, 2, 3, 4 };
            var targetIndex = 5;

            var algo = new C45<object>();
            algo.Process(samples, optionIndexes, targetIndex);
        }
    }
}
