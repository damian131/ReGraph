using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ReGraph.Models.GraphDrawer
{
    public class Line
    {
        public List<Point> Points;
        public Color Color;
        public String Name;

        public Line(List<Point> points, Color color, String name)
        {
            this.Points = points;
            this.Color = color;
            this.Name = name;
        }

        public Line()
        {
            Points = new List<Point>();
        }
    }
}
