using System;
using System.Collections.Generic;
using System.Text;
using ReGraph.Models.GraphDrawer;
using Windows.Storage;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI;
namespace ReGraph.Models.GraphDrawer
{
    public class GraphFileReader
    {
        private GraphDrawer GraphControl;
        private const char COMMA = ',';
        private const string ENDLINE = "\n";

        public GraphFileReader(GraphDrawer Drawer)
        {
            GraphControl = Drawer;
        }

        public async void readFromFile(StorageFile file)
        {
            IList<string> lines = await FileIO.ReadLinesAsync(file);

            //reading titles
            string[] titles = lines[0].Split(',');
            GraphControl.Title = titles[0];
            GraphControl.HorizontalTitle = titles[1];
            GraphControl.VerticalTitle = titles[1];
            lines.RemoveAt(0);

            //reading ranges
            string[] ranges = lines[0].Split(COMMA);
            GraphControl.HorizontalAxis.Minimum = Double.Parse(ranges[0]);
            GraphControl.HorizontalAxis.Maximum = Double.Parse(ranges[1]);
            GraphControl.VerticalAxis.Minimum = Double.Parse(ranges[2]);
            GraphControl.VerticalAxis.Maximum = Double.Parse(ranges[3]);
            lines.RemoveAt(0);

            //reading series size
            int series_count = int.Parse(lines[0]);
            lines.RemoveAt(0);

            //parsing lines
            for (int i = 0; i < series_count; ++i)
            {
                Line line = new Line();

                //reading series header
                string[] header = lines[0].Split(COMMA);
                line.Name = header[0];
                int length = int.Parse(header[1]);
                byte r = byte.Parse(header[2]);
                byte g = byte.Parse(header[3]);
                byte b = byte.Parse(header[4]);
                line.Color = Color.FromArgb(255, r, g, b);
                int type = int.Parse(header[5]);
                lines.RemoveAt(0);

                //parsing points
                for (int j = 0; j < length; ++j)
                {
                    string[] p = lines[0].Split(COMMA);
                    double x = Double.Parse(p[0]);
                    double y = Double.Parse(p[1]);
                    line.Points.Add(new Point() { X = x, Y = y });
                    lines.RemoveAt(0);
                }

                //adding line
                if (type == 0)
                    GraphControl.addSolidLine(line);
                else
                    GraphControl.addDottedLine(line);
            }
        }
    }
}
