using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.Models.GraphDrawer
{
    public class Point : IComparable
    {
        public double X { get; set; }
        public double Y { get; set; }



        public int CompareTo(object obj)
        {
            Point p = obj as Point;
            if (p.X > X)
            {
                return -1;
            }
            if (p.X < X)
            {
                return 1;
            }
            return 0;
        }

    }
}
