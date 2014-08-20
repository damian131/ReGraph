using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace ReGraph.Models.GraphDrawer
{
    public class GraphFileWriter
    {
        private GraphDrawer GraphControl;
        private const string COMMA = ",";
        private const string ENDLINE = "\n";

        public GraphFileWriter(GraphDrawer Drawer)
        {
            GraphControl = Drawer;
        }


        public async void writeToFile(StorageFile file)
        {
            //writing graph titles
            await FileIO.WriteTextAsync(file, GraphControl.Title + COMMA + GraphControl.HorizontalTitle + COMMA + GraphControl.VerticalTitle + ENDLINE);

            //writing ranges
            await FileIO.AppendTextAsync(file, GraphControl.HorizontalAxis.Minimum.ToString()+COMMA+GraphControl.HorizontalAxis.Maximum.ToString()+COMMA);
            await FileIO.AppendTextAsync(file, GraphControl.VerticalAxis.Minimum.ToString() + COMMA + GraphControl.VerticalAxis.Maximum.ToString() + ENDLINE);

            //writing series
            List<Line> solidLines = GraphControl.SolidLines;
            List<Line> dottedLines = GraphControl.DottedLines;
            await FileIO.AppendTextAsync(file, (solidLines.Count + dottedLines.Count).ToString()+ENDLINE);

            foreach (var line in solidLines)
            {
                await FileIO.AppendTextAsync(file, line.Name + COMMA + line.Points.Count + COMMA + line.Color.R.ToString() + COMMA + line.Color.G.ToString() + COMMA + line.Color.B.ToString() + COMMA + "0" + ENDLINE);
                foreach (var point in line.Points)
                {
                    await FileIO.AppendTextAsync(file, point.X.ToString() + COMMA + point.Y + ENDLINE);
                }
            }

            foreach (var line in dottedLines)
            {
                     
                await FileIO.AppendTextAsync(file, line.Name + COMMA + line.Points.Count + COMMA + line.Color.R.ToString()+COMMA+line.Color.G.ToString()+COMMA+line.Color.B.ToString() + COMMA + "1" + ENDLINE);
                foreach (var point in line.Points)
                {
                    await FileIO.AppendTextAsync(file, point.X.ToString() + COMMA + point.Y + ENDLINE);
                }
            }
        }
    }
}
