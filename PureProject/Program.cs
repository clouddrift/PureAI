using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pure.DataMining;

namespace PureProject
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] values = { 9.0 / 14, 5.0 / 14 };
            double result = Utilities.GetEntropy(values);
            Console.WriteLine(result);
        }

        private void AprioriSample()
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

            Apriori<int> algo = new Apriori<int>(2, 4);

            foreach (var result in algo.Process(samples))
            {
                var list = result.ToList();
                list.Sort();

                Console.WriteLine(string.Join(" ", list));
            }
        }
    }
}
