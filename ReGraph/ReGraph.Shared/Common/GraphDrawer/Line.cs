using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ReGraph.Common.GraphDrawer
{
    class Line
    {
        public List<Point> points;
        public Color color;
        public String name;

        public Line(List<Point> points, Color color, String name)
        {
            this.points = points;
            this.color = color;
            this.name = name;
        }
    }
}
