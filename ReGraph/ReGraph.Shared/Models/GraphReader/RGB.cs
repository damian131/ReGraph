using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace ReGraph.Models.GraphReader
{
    public class RGB
    {
        public byte R;
        public byte G;
        public byte B;
        public enum RecognitionStasus { NONE, SELECTED, DELETED };
        public RecognitionStasus PixelRecognitionStatus;
        public RGB(byte red, byte green, byte blue)
        {
            R = red;
            G = green;
            B = blue;
            PixelRecognitionStatus = RecognitionStasus.NONE;
        }
        public RGB()
        {
            R = 0;
            G = 0;
            B = 0;
            PixelRecognitionStatus = RecognitionStasus.NONE;
        }
        public RGB(Color col)
        {
            R = col.R;
            G = col.G;
            B = col.B;
            PixelRecognitionStatus = RecognitionStasus.NONE;
        }
        public RGB(RGB col)
        {
            R = col.R;
            G = col.G;
            B = col.B;
            PixelRecognitionStatus = RecognitionStasus.NONE;
        }
        public RGB(byte col)
        {
            R = col;
            G = col;
            B = col;
            PixelRecognitionStatus = RecognitionStasus.NONE;
        }
        public byte[] toByteArray()
        {
            byte[] tab = new byte[4];
            tab[0] = B;
            tab[1] = G;
            tab[2] = R;
            tab[3] = 255;
            return tab;
        }
        public Color toColor()
        {
            Color temp = new Color();
            temp.R = R;
            temp.G = G;
            temp.B = B;
            temp.A = 255;
            return temp;
        }
        public bool Equals(RGB color)
        {
            if (color.R != R) return false;
            if (color.G != G) return false;
            if (color.B != B) return false;
            return true;
        }
        public bool Equals(Color color)
        {
            if (color.R != R) return false;
            if (color.G != G) return false;
            if (color.B != B) return false;
            return true;
        }
        public RGB Clone()
        {
            return new RGB(R, G, B);
        }
        public byte max()
        {
            return Math.Max(R, Math.Max(G, B));
        }
        public int getDifference(RGB color)
        {
            int result = 0;
            result += Math.Abs(color.R - R);
            result += Math.Abs(color.G - G);
            result += Math.Abs(color.B - B);
            return result;
        }

        public String ToString()
        {
            return "R = " + R + ", G =" + G + ", B = " + B;
        }
    }
}
