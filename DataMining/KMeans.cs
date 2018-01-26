using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public class KMeans<T>
    {
        private Func<T, T, double> distanceCalculator;
        private Func<IEnumerable<T>, T> centerCalculator;
        private double iterationThreshold;

        public KMeans(Func<T, T, double> distanceCalculator, Func<IEnumerable<T>, T> centerCalculator, double iterationThreshold = 1e-8)
        {
            this.distanceCalculator = distanceCalculator;
            this.centerCalculator = centerCalculator;
            this.iterationThreshold = iterationThreshold;
        }

        public IList<HashSet<int>> Process(IList<T> samples, IList<T> clusterCenters)
        {
            IList<T> currentCenters = clusterCenters;
            IList<HashSet<int>> clusters = CreateClusters(clusterCenters.Count);
            bool needIteration = true;

            while (needIteration)
            {
                for (int sampleIndex = 0; sampleIndex < samples.Count; sampleIndex++)
                {
                    int clusterIndex = IndexOfNearestClusterCenter(samples[sampleIndex], currentCenters);
                    clusters[clusterIndex].Add(sampleIndex);
                }

                var nextCenters = clusters.Select(o => centerCalculator(o.Select(x => samples[x]))).ToList();
                var offsetOfCenters = currentCenters.Zip(nextCenters, (first, second) => distanceCalculator(first, second)).ToList();
                currentCenters = nextCenters;
                needIteration = offsetOfCenters.Any(o => o > iterationThreshold);
            }

            return clusters;
        }

        private IList<HashSet<int>> CreateClusters(int count)
        {
            IList<HashSet<int>> clusters = new List<HashSet<int>>(count);

            for (int i = 0; i < count; i++)
            {
                var emptyCluster = new HashSet<int>();
                clusters.Add(emptyCluster);
            }

            return clusters;
        }

        private int IndexOfNearestClusterCenter(T sample, IList<T> clusterCenters)
        {
            int clusterIndex = -1;
            double minDistance = double.MaxValue;

            for (int j = 0; j < clusterCenters.Count; j++)
            {
                double curDistance = distanceCalculator(sample, clusterCenters[j]);

                if (clusterIndex == -1 || curDistance < minDistance)
                {
                    clusterIndex = j;
                    minDistance = curDistance;
                }
            }

            return clusterIndex;
        }
    }
}
