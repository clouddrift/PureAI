using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pure.DataMining;

namespace Pure.DataMining.UnitTests
{
    class RegionalDivision : IAdaBootModel<double, bool>
    {
        private Func<IEnumerable<double>, bool> formula;

        public bool Classify(IEnumerable<double> properties)
        {
            return formula.Invoke(properties);
        }

        public RegionalDivision(Func<IEnumerable<double>, bool> formula)
        {
            this.formula = formula;
        }
    }

    [TestClass]
    public class AdaBoostUnitTests
    {
        [TestMethod]
        public void TestAdaBoost()
        {
            var models = new List<IAdaBootModel<double, bool>>()
            {
                new RegionalDivision(o => o.ToList()[0] >  -0.5),
                new RegionalDivision(o => o.ToList()[0] <= -0.5),
                new RegionalDivision(o => o.ToList()[0] <=  0.5),
                new RegionalDivision(o => o.ToList()[0] >   0.5),
                new RegionalDivision(o => o.ToList()[1] >  -0.5),
                new RegionalDivision(o => o.ToList()[1] <= -0.5),
                new RegionalDivision(o => o.ToList()[1] <=  0.5),
                new RegionalDivision(o => o.ToList()[1] >   0.5)
            };

            var tuples = new List<LabeledTuple<double, bool>>()
            {
                new LabeledTuple<double, bool>( true, -1,  0),
                new LabeledTuple<double, bool>( true,  1,  0),
                new LabeledTuple<double, bool>(false,  0, -1),
                new LabeledTuple<double, bool>(false,  0,  1)
            };

            var adaBoost = new AdaBoost<double, bool>(models);
            adaBoost.Train(tuples, tuples.Count);

            foreach (var tuple in tuples)
            {
                var expected = tuple.Target;
                var actual = adaBoost.Predict(tuple.Properties);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
