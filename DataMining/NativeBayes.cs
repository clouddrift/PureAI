using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public sealed class NativeBayesResult<TTarget>
    {
        public TTarget Target
        {
            get;
        }

        public double Probability
        {
            get;
        }

        internal NativeBayesResult(TTarget target, double probability)
        {
            this.Target = target;
            this.Probability = probability;
        }
    }

    public class NativeBayes<TProperty, TTarget>
    {
        private List<TTarget> targetItems;
        private Dictionary<string, NativeBayesProperty<TProperty, TTarget>> propertyItems;

        internal int TargetCount
        {
            get { return this.targetItems.Count; }
        }

        public NativeBayes(params TTarget[] targets)
        {
            this.targetItems = targets.ToList();
            this.propertyItems = new Dictionary<string, NativeBayesProperty<TProperty, TTarget>>();
        }

        public NativeBayesProperty<TProperty, TTarget> AddProperty(string name)
        {
            var property = new NativeBayesProperty<TProperty, TTarget>(this);
            propertyItems.Add(name, property);
            return property;
        }

        public NativeBayesResult<TTarget> Perform(IDictionary<string, TProperty> input)
        {
            if (input.Count > propertyItems.Count)
            {
                throw new ArgumentException();
            }

            TTarget target = default(TTarget);
            double probability = 0.0;

            for (int targetIndex = 0; targetIndex < targetItems.Count; targetIndex++)
            {
                double targetProbability = 1.0;

                foreach (var pair in input)
                {
                    NativeBayesProperty<TProperty, TTarget> propertyItem;
                    bool result = propertyItems.TryGetValue(pair.Key, out propertyItem);

                    if (result)
                    {
                        double percentage = propertyItem.GetPropertyPercentage(pair.Value, targetIndex);
                        targetProbability *= percentage;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }

                double aa = propertyItems.Sum(o => o.Value.GetTotalCount(targetIndex)) / propertyItems.Sum(o => o.Value.GetTotalCount());
                targetProbability *= aa;

                if (probability < targetProbability)
                {
                    probability = targetProbability;
                    target = targetItems[targetIndex];
                }
            }

            return new NativeBayesResult<TTarget>(target, probability);
        }
    }

    public class NativeBayesProperty<TProperty, TTarget>
    {
        private NativeBayes<TProperty, TTarget> parent;
        private Dictionary<TProperty, IList<int>> propertyItems;
        private IList<int> totalCounters;

        public ReadOnlyDictionary<TProperty, IList<int>> PropertyItems
        {
            get { return new ReadOnlyDictionary<TProperty, IList<int>>(propertyItems); }
        }

        internal NativeBayesProperty(NativeBayes<TProperty, TTarget> parent)
        {
            this.propertyItems = new Dictionary<TProperty, IList<int>>();
            this.totalCounters = Enumerable.Repeat(0, parent.TargetCount).ToList();
            this.parent = parent;
        }

        public void AddPropertyItem(TProperty item, params int[] counters)
        {
            if (this.parent.TargetCount != counters.Length)
            {
                throw new ArgumentException();
            }

            this.propertyItems.Add(item, counters);

            for (int i = 0; i < this.parent.TargetCount; i++)
            {
                this.totalCounters[i] += counters[i];
            }
        }

        public double GetPropertyPercentage(TProperty property, int targetIndex)
        {
            return PropertyItems[property][targetIndex] * 1.0 / totalCounters[targetIndex];
        }

        public double GetTotalCount(int targetIndex)
        {
            return totalCounters[targetIndex];
        }

        public double GetTotalCount()
        {
            return totalCounters.Sum();
        }
    }
}
