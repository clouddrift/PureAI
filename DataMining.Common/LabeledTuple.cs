using System.Collections.Generic;

namespace Pure.DataMining
{
    public class LabeledTuple<TProperty, TTarget>
    {
        public TTarget Target
        {
            get;
        }

        public IList<TProperty> Properties
        {
            get;
        }

        public LabeledTuple(TTarget target, params TProperty[] properties)
        {
            this.Target = target;
            this.Properties = new List<TProperty>(properties);
        }
    }
}
