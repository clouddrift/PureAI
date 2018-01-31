using System;
using System.Collections.Generic;
using System.Linq;

namespace Pure.DataMining
{
    public class NativeBayes<TProperty, TTarget>
    {
        private List<TTarget> targets;
        private Dictionary<string, NativeBayesProperty<TProperty, TTarget>> properties;

        internal int TargetCount
        {
            get { return this.targets.Count; }
        }

        public NativeBayes(params TTarget[] targets)
        {
            this.targets = targets.ToList();
            this.properties = new Dictionary<string, NativeBayesProperty<TProperty, TTarget>>();
        }

        public NativeBayesProperty<TProperty, TTarget> AddProperty(string name)
        {
            var property = new NativeBayesProperty<TProperty, TTarget>(this);
            properties.Add(name, property);
            return property;
        }

        public NativeBayesResult<TTarget> Perform(IDictionary<string, TProperty> inputProperties)
        {
            if (inputProperties.Count > properties.Count)
            {
                throw new ArgumentException();
            }

            TTarget target = default(TTarget);
            double maxJoint = 0.0;

            for (int targetIndex = 0; targetIndex < targets.Count; targetIndex++)
            {
                double likelihood = 1.0;

                foreach (var inputPropertyPair in inputProperties)
                {
                    NativeBayesProperty<TProperty, TTarget> property;

                    if (properties.TryGetValue(inputPropertyPair.Key, out property))
                    {
                        double subLikelihood = property.GetCount(inputPropertyPair.Value, targetIndex) / property.GetCount(targetIndex);
                        likelihood *= subLikelihood;
                    }
                    else
                    {
                        string message = string.Format("Input property '{0}' is invalid.", inputPropertyPair.Key);
                        throw new ArgumentException(message);
                    }
                }

                double prior = properties.Sum(o => o.Value.GetCount(targetIndex)) / properties.Sum(o => o.Value.GetCount());
                double curJoint = likelihood * prior;

                if (maxJoint < curJoint)
                {
                    maxJoint = curJoint;
                    target = targets[targetIndex];
                }
            }

            return new NativeBayesResult<TTarget>(target, maxJoint);
        }
    }
}
