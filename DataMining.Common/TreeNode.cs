using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pure.DataMining
{
    public class TreeNode<T>
    {
        public T Value
        {
            get;
        }

        public IList<TreeNode<T>> Children
        {
            get;
        }

        public void Output()
        {
            foreach (var child in Children)
            {
                Console.WriteLine("{0} => {1}", Value, child.Value);
            }
        }
    }
}
