using System.Collections.Generic;

namespace Pure.DataMining
{
    public interface IAdaBootModel<TProperty, TTarget>
    {
        TTarget Classify(IEnumerable<TProperty> properties);
    }
}
