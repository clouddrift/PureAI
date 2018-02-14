using System;

namespace Pure.DataMining
{
    public static class DoubleExtension
    {
        public static bool AreEqual(this double source, double other, double delta)
        {
            return Math.Abs(source - other) <= delta;
        }
    }
}
