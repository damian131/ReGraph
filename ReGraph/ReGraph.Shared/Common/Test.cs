using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using ReGraph.Models.GraphDrawer;
namespace ReGraph.Common
{
    public class Test
    {
        public static Line getTestLine()
        {
            Random rnd = new Random();
            string name = "TEST" + rnd.NextDouble();
            Color color = Color.FromArgb(255, (byte)(rnd.NextDouble() * 255), (byte)(rnd.NextDouble() * 255), (byte)(rnd.NextDouble() * 255));

            List<Point> list = new List<Point>();
            int size = (int)(rnd.NextDouble() * 50);
            for (int i = 0; i < size; ++i)
                list.Add(new Point() { X = rnd.NextDouble()*100, Y = rnd.NextDouble()*100 });
            
            return new Line(list, color, name);
        }
    }
}
