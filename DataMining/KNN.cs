using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public class KNN<TProperty, TTarget>
    {
        private IList<Func<TProperty, TProperty>> normalizers;
        private IList<Tuple<IEnumerable<TProperty>, TTarget>> trainingData;
        private Func<IEnumerable<TProperty>, IEnumerable<TProperty>, double> distanceCalculator;

        public KNN(Func<IEnumerable<TProperty>, IEnumerable<TProperty>, double> distanceCalculator)
        {
            this.normalizers = new List<Func<TProperty, TProperty>>();
            this.trainingData = new List<Tuple<IEnumerable<TProperty>, TTarget>>();
            this.distanceCalculator = distanceCalculator;
        }

        public void AddNormalizer(Func<TProperty, TProperty> function)
        {
            this.normalizers.Add(function);
        }

        public void AddTrainingData(IEnumerable<TProperty> properties, TTarget target)
        {
            var normalizedTrainingData = Normalize(properties);
            var data = new Tuple<IEnumerable<TProperty>, TTarget>(normalizedTrainingData, target);
            this.trainingData.Add(data);
        }

        public TTarget Perform(IEnumerable<TProperty> properties, int k)
        {
            var normalizedData = Normalize(properties);

            var query = from data in this.trainingData
                        orderby distanceCalculator(data.Item1, normalizedData) ascending
                        select data;

            var target = from data in query.Take(k)
                         group data by data.Item2 into g
                         orderby g.Count() descending
                         select g.Key;

            return target.First();
        }

        private IEnumerable<TProperty> Normalize(IEnumerable<TProperty> properties)
        {
            return properties.Select((property, index) => normalizers[index](property));
        }
    }
}
