using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Pure.DataMining;

namespace Pure.DataMining
{
    public class C45<T>
    {
        public TreeNode<T> Process(IEnumerable<IList<T>> samples, IEnumerable<int> optionIndexes, int targetIndex)
        {
            var root = new TreeNode<T>();
            var totalCount = samples.Count();
            var propertyMapping = new Dictionary<T, int>();

            foreach (var sample in samples)
            {
                propertyMapping.AddOrIncrease(sample[targetIndex]);
            }

            double entropy = NumericCalculation.Entropy(propertyMapping.Select(o => o.Value * 1.0 / totalCount));
            Console.WriteLine(entropy);

            foreach (var index in optionIndexes)
            {
                if (index != targetIndex)
                {
                    Console.WriteLine(index);

                    var mapping = new Dictionary<T, IDictionary<T, int>>();
                    var list = samples.Select(o => o[index]).Distinct();

                    foreach (var key in list)
                    {
                        foreach (var sample in samples.Where(o => o[index].Equals(key)).GroupBy(o => o[targetIndex]))
                        {
                            Console.WriteLine(key + " " + sample.Key + " " + sample.Count());
                            //var splitValue = sample[index];
                            //var targetValue = sample[targetIndex];
                            //mapping.AddOrIncrease(sample[index]);
                        }
                    }
                }
            }

            return null;
        }
    }
}
