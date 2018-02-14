using System;
using System.Collections.Generic;
using System.Linq;

namespace Pure.DataMining
{
    public class AdaBoost<TProperty, TTarget>
    {
        private const double equalityDelta = 1e-8;
        private const int trainingCount = 10;

        private Dictionary<IAdaBootModel<TProperty, TTarget>, double> weightedModels;

        public AdaBoost(IEnumerable<IAdaBootModel<TProperty, TTarget>> models)
        {
            this.weightedModels = models.ToDictionary(o => o, o => 0.0);
        }

        public void Train(IEnumerable<LabeledTuple<TProperty, TTarget>> tuples, int tupleCount)
        {
            Dictionary<LabeledTuple<TProperty, TTarget>, double> weightedTuples = tuples.ToDictionary(o => o, weight => 1.0 / tupleCount);
            IEnumerable<IAdaBootModel<TProperty, TTarget>> models = new List<IAdaBootModel<TProperty, TTarget>>(weightedModels.Keys);

            foreach (var model in models)
            {
                int consistentCount = 0;
                int totalCount = 0;
                List<LabeledTuple<TProperty, TTarget>> consistentTuples = new List<LabeledTuple<TProperty, TTarget>>();

                foreach (var trainTuple in GetTrainingSet(weightedTuples))
                {
                    if (model.Classify(trainTuple.Properties).Equals(trainTuple.Target))
                    {
                        consistentCount++;
                    }

                    totalCount++;
                }

                double errorRate = 1 - consistentCount * 1.0 / totalCount;

                if (errorRate > 0.5)
                {
                    continue;
                }

                UpdateModelWeight(model, errorRate);
                DecreaseTupleWeight(weightedTuples, consistentTuples, errorRate);
                NormalizeTupleWeight(weightedTuples);
            }
        }

        public TTarget Predict(IEnumerable<TProperty> properties)
        {
            var ladder = new Dictionary<TTarget, double>();

            foreach (var weightedModel in weightedModels)
            {
                IAdaBootModel<TProperty, TTarget> model = weightedModel.Key;
                double weight = weightedModel.Value;

                TTarget classification = model.Classify(properties);
                ladder.AddOrIncrease(classification, weight, o => o + weight);
            }

            TTarget target = ladder.OrderByDescending(o => o.Value).First().Key;
            return target;
        }

        private IEnumerable<LabeledTuple<TProperty, TTarget>> GetTrainingSet(IDictionary<LabeledTuple<TProperty, TTarget>, double> weightedTuples)
        {
            return from tuple in weightedTuples
                   orderby tuple.Value descending
                   select tuple.Key;
        }

        private void UpdateModelWeight(IAdaBootModel<TProperty, TTarget> model, double errorRate)
        {
            this.weightedModels[model] = Math.Log(1 - errorRate, errorRate);
        }

        private void DecreaseTupleWeight(IDictionary<LabeledTuple<TProperty, TTarget>, double> weightedTuples, IEnumerable<LabeledTuple<TProperty, TTarget>> consistentTuples, double errorRate)
        {
            double factor = errorRate / (1 - errorRate);

            foreach (var correctTuple in consistentTuples)
            {
                weightedTuples[correctTuple] *= factor;
            }
        }

        private void NormalizeTupleWeight(IDictionary<LabeledTuple<TProperty, TTarget>, double> weightedTuples)
        {
            List<LabeledTuple<TProperty, TTarget>> keys = new List<LabeledTuple<TProperty, TTarget>>(weightedTuples.Keys);
            double weightSum = weightedTuples.Values.Sum();

            foreach (var tuple in keys)
            {
                weightedTuples[tuple] /= weightSum;
            }
        }
    }
}
