using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pure.DataMining;

namespace Pure.DataMining.UnitTests
{
    [TestClass]
    public class NativeBayesUnitTests
    {
        [TestMethod]
        public void TestNativeBayes()
        {
            bool[] buyComputer = { true, false };

            int[][] ageArray = new int[][]
            {
                new int[] { 2, 3 }, // youth
                new int[] { 4, 0 }, // middle-aged
                new int[] { 3, 2 }  // senior
            };

            int[][] incomeArray = new int[][]
            {
                new int[] { 3, 1 }, // low
                new int[] { 4, 2 }, // medium
                new int[] { 2, 2 }  // high
            };

            int[][] studentArray = new int[][]
            {
                new int[] { 6, 1 }, // yes
                new int[] { 3, 4 }  // no
            };

            int[][] creditArray = new int[][]
            {
                new int[] { 6, 2 }, // fair
                new int[] { 3, 3 }  // excellent
            };

            string[] names = { "age", "income", "student", "credit" };
            string[] ageItems = { "youth", "middle-aged", "senior" };
            string[] incomeItems = { "low", "medium", "high" };
            string[] studentItems = { "yes", "no" };
            string[] creditItems = { "fair", "excellent" };

            var bayes = new NativeBayes<string, bool>(buyComputer);

            var age = bayes.AddProperty(names[0]);
            age.AddPropertyItem(ageItems[0], ageArray[0]);
            age.AddPropertyItem(ageItems[1], ageArray[1]);
            age.AddPropertyItem(ageItems[2], ageArray[2]);

            var income = bayes.AddProperty(names[1]);
            income.AddPropertyItem(incomeItems[0], incomeArray[0]);
            income.AddPropertyItem(incomeItems[1], incomeArray[1]);
            income.AddPropertyItem(incomeItems[2], incomeArray[2]);

            var student = bayes.AddProperty(names[2]);
            student.AddPropertyItem(studentItems[0], studentArray[0]);
            student.AddPropertyItem(studentItems[1], studentArray[1]);

            var credit = bayes.AddProperty(names[3]);
            credit.AddPropertyItem(creditItems[0], creditArray[0]);
            credit.AddPropertyItem(creditItems[1], creditArray[1]);

            Dictionary<string, string> testProperties = new Dictionary<string, string>()
            {
                { names[0], ageItems[0] },
                { names[1], incomeItems[1] },
                { names[2], studentItems[0] },
                { names[3], creditItems[0] }
            };

            var result = bayes.Perform(testProperties);
            Assert.AreEqual(true, result.Target);
            Assert.AreEqual(0.028, result.JointProbability, 0.001);
        }
    }
}
