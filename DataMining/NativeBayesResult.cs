namespace Pure.DataMining
{
    public sealed class NativeBayesResult<TTarget>
    {
        public TTarget Target
        {
            get;
        }

        public double JointProbability
        {
            get;
        }

        internal NativeBayesResult(TTarget target, double jointProbability)
        {
            this.Target = target;
            this.JointProbability = jointProbability;
        }
    }
}
