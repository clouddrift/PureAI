using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public class Apriori<T>
    {
        private IEnumerable<IEnumerable<T>> samples;
        private IList<HashSet<T>> seeds;
        private double minSupport;

        public int CurrentGeneration
        {
            get;
            private set;
        } = 0;

        public IList<HashSet<T>> Result
        {
            get;
            private set;
        }

        public Apriori(double minSupport)
        {
            if (minSupport < 0)
            {
                throw new ArgumentException("The minimum support should be non-negative.");
            }

            this.minSupport = minSupport;
        }

        public void Initialize(IEnumerable<IEnumerable<T>> samples)
        {
            this.samples = samples;
            this.seeds = CreateInitSeeds(samples);
        }

        public void NextGeneration(int maxGeneration)
        {
            while (CurrentGeneration < maxGeneration)
            {
                Result = seeds.Where(o => IsAboveSupport(samples, o)).ToList();
                seeds = GetNextGenerationCandidates(Result);
                CurrentGeneration++;
            }
        }

        private IList<HashSet<T>> CreateInitSeeds(IEnumerable<IEnumerable<T>> samples)
        {
            return samples.SelectMany(o => o)
                          .Distinct()
                          .Select(o => Enumerable.Repeat(o, 1).ToHashSet())
                          .ToList();
        }

        private bool IsAboveSupport(IEnumerable<IEnumerable<T>> samples, HashSet<T> seed)
        {
            int curSupport = samples.Count(o => !seed.Except(o).Any());
            return curSupport >= minSupport;
        }

        private IList<HashSet<T>> GetNextGenerationCandidates(IList<HashSet<T>> seeds)
        {
            List<HashSet<T>> candidates = new List<HashSet<T>>();

            foreach (var seed1 in seeds)
            {
                foreach (var seed2 in seeds)
                {
                    var exceptedSeed1 = seed1.Except(seed2);
                    var exceptedSeed2 = seed2.Except(seed1);

                    if (exceptedSeed1.IsExactSingle() && exceptedSeed2.IsExactSingle())
                    {
                        var candidate = seed1.Union(exceptedSeed2).ToHashSet();

                        if (candidates.All(o => !o.SetEquals(candidate)))
                        {
                            candidates.Add(candidate);
                        }
                    }
                }
            }

            return candidates;
        }
    }
}
