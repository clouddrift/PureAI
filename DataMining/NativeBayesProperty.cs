using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pure.DataMining
{
    public class NativeBayesProperty<TProperty, TTarget>
    {
        private NativeBayes<TProperty, TTarget> parent;
        private Dictionary<TProperty, IList<int>> propertyItems;
        private List<int> targetCounters;
        private int totalCounter;

        public ReadOnlyDictionary<TProperty, IList<int>> PropertyItems
        {
            get { return new ReadOnlyDictionary<TProperty, IList<int>>(propertyItems); }
        }

        internal NativeBayesProperty(NativeBayes<TProperty, TTarget> parent)
        {
            this.propertyItems = new Dictionary<TProperty, IList<int>>();
            this.targetCounters = Enumerable.Repeat(0, parent.TargetCount).ToList();
            this.parent = parent;
        }

        public void AddPropertyItem(TProperty item, params int[] countArray)
        {
            if (this.parent.TargetCount != countArray.Length)
            {
                throw new ArgumentException("The count of target and array should be same.");
            }

            this.propertyItems.Add(item, countArray);

            for (int i = 0; i < this.parent.TargetCount; i++)
            {
                this.targetCounters[i] += countArray[i];
                this.totalCounter += countArray[i];
            }
        }

        public double GetCount(TProperty property, int targetIndex)
        {
            return PropertyItems[property][targetIndex];
        }

        public double GetCount(int targetIndex)
        {
            return targetCounters[targetIndex];
        }

        public double GetCount()
        {
            return totalCounter;
        }
    }
}
