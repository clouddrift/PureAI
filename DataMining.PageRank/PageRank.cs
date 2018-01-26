using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Pure.DataMining
{
    public class PageRank
    {
        public double DampingFactor
        {
            get;
            set;
        } = 0.85;

        public double IterationThreshold
        {
            get;
            set;
        } = 1e-5;

        public int IterationLimit
        {
            get;
            set;
        } = 20;

        public PageRankResult Perform(double[] state, double[,] transform)
        {
            Matrix<double> current = CreateMatrix.DenseOfRows(Enumerable.Repeat(state, 1));
            Matrix<double> linkMatrix = CreateMatrix.DenseOfArray(transform);
            Matrix<double> randomMatrix = CreateMatrix.Dense(state.Length, state.Length, 1.0 / state.Length);

            int iterationCount = 0;
            double norm = double.MaxValue;

            while (iterationCount < IterationLimit && IterationThreshold < norm)
            {
                Matrix<double> PossibilityMatrix = (1 - DampingFactor) * randomMatrix + DampingFactor * linkMatrix;
                Matrix<double> next = current.Multiply(PossibilityMatrix);
                Matrix<double> difference = next - current;
                norm = difference.L1Norm();

                current = next;
                iterationCount++;
            }

            return new PageRankResult(current.ToArray(), iterationCount);
        }
    }
}
