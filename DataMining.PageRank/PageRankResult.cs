using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace Pure.DataMining
{
    public class PageRankResult
    {
        public double[,] Matrix
        {
            get;
        }

        public double IterationCount
        {
            get;
        }

        public PageRankResult(double [,] matrix, double iterationCount)
        {
            Matrix = matrix;
            IterationCount = iterationCount;
        }
    }
}
