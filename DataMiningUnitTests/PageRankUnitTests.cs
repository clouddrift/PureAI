using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pure.DataMining;

namespace Pure.DataMining.UnitTests
{
    [TestClass]
    public class PageRankUnitTests
    {
        /// <summary>
        /// A  => B
        /// A <=> C
        /// A  => D
        /// B <=> D
        /// C  => D
        /// </summary>
        [TestMethod]
        public void TestPageRankWithDamping()
        {
            double[] state = new double[4] { 0.25, 0.25, 0.25, 0.25 };

            double[,] transform = new double[4, 4]
            {
                {   0, 1.0/3, 1.0/3, 1.0/3 },
                {   0,     0,     0,     1 },
                { 0.5,     0,     0,   0.5 },
                {   0,     1,     0,     0 }
            };

            var algo = new PageRank();
            algo.IterationThreshold = 1e-8;
            algo.IterationLimit = 50;
            algo.DampingFactor = 0.8;
            PageRankResult result = algo.Perform(state, transform);

            // Check element structure of result matrix.
            Assert.AreEqual(2, result.Matrix.Rank);
            Assert.AreEqual(4, result.Matrix.Length);
            Assert.AreEqual(1, result.Matrix.GetLength(0));
            Assert.AreEqual(4, result.Matrix.GetLength(1));

            // Check element data of result matrix.
            Assert.AreEqual(0.078, result.Matrix[0, 0], 0.001);
            Assert.AreEqual(0.418, result.Matrix[0, 1], 0.001);
            Assert.AreEqual(0.071, result.Matrix[0, 2], 0.001);
            Assert.AreEqual(0.433, result.Matrix[0, 3], 0.001);
        }

        /// <summary>
        /// A <=> B
        /// A <=> C
        /// A  => D
        /// B <=> D
        /// C  => D
        /// </summary>
        [TestMethod]
        public void TestPageRankWithFullDamping()
        {
            double[] state = new double[4] { 0.25, 0.25, 0.25, 0.25 };

            double[,] transform = new double[4, 4]
            {
                {   0, 1.0/3, 1.0/3, 1.0/3 },
                { 0.5,     0,     0,   0.5 },
                {   1,     0,     0,     0 },
                {   0,   0.5,   0.5,     0 }
            };

            var algo = new PageRank();
            algo.DampingFactor = 1;
            PageRankResult result = algo.Perform(state, transform);

            // Check element structure of result matrix.
            Assert.AreEqual(2, result.Matrix.Rank);
            Assert.AreEqual(4, result.Matrix.Length);
            Assert.AreEqual(1, result.Matrix.GetLength(0));
            Assert.AreEqual(4, result.Matrix.GetLength(1));

            // Check element data of result matrix.
            Assert.AreEqual(3.0 / 9, result.Matrix[0, 0], algo.IterationThreshold);
            Assert.AreEqual(2.0 / 9, result.Matrix[0, 1], algo.IterationThreshold);
            Assert.AreEqual(2.0 / 9, result.Matrix[0, 2], algo.IterationThreshold);
            Assert.AreEqual(2.0 / 9, result.Matrix[0, 3], algo.IterationThreshold);
        }

        /// <summary>
        /// A <=> A
        /// A  => B
        /// B <=> B
        /// </summary>
        [TestMethod]
        public void TestPageRankWithSelfRing()
        {
            double[] state = new double[2] { 0.5, 0.5 };

            double[,] transform = new double[2, 2]
            {
                { 0.5, 0.5},
                {   0, 1.0}
            };

            var algo = new PageRank();
            algo.DampingFactor = 1;
            PageRankResult result = algo.Perform(state, transform);

            // Check element structure of result matrix.
            Assert.AreEqual(2, result.Matrix.Rank);
            Assert.AreEqual(2, result.Matrix.Length);
            Assert.AreEqual(1, result.Matrix.GetLength(0));
            Assert.AreEqual(2, result.Matrix.GetLength(1));

            // Check element data of result matrix.
            Assert.AreEqual(0, result.Matrix[0, 0], algo.IterationThreshold);
            Assert.AreEqual(1, result.Matrix[0, 1], algo.IterationThreshold);
        }
    }
}
