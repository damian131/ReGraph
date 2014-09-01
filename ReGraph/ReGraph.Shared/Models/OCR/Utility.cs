using ReGraph.Models.GraphReader;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.Models.OCR
{
    class Utility
    {

        /// <summary>
        /// This method displays message box
        /// </summary>
        /// <param name="message">Message to display</param>
        async static public void ShowDialog(string message)
        {
            var dialog = new MessageDialog(message, "Information");
            await dialog.ShowAsync();
        }


        ///// <summary>
        ///// Transform image to grayscale
        ///// </summary>
        ///// <param name="image">Image to transform</param>
        //public static void GrayScale(WriteableBitmap image)
        //{
        //    byte[] src = image.ToByteArray();
        //    for (int i = 0; i < src.Length; i += 4)
        //    {
        //        int gray = (src[i] + src[i + 1] + src[i + 2]) / 3;
        //        src[i] = src[i + 1] = src[i + 2] = (byte)gray;
        //    }
        //    image.FromByteArray(src);
        //}



        /// <summary>
        /// Transform image to grayscale
        /// </summary>
        /// <param name="image">Image to transform</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        public static void GrayScale(RGB[,] image, int width, int height)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    byte wyn = (byte)(0.3 * image[x,y].R + 0.59 * image[x,y].G + 0.11 * image[x,y].B);
                    image[x,y].R = image[x,y].G = image[x,y].B = wyn;
                }
            }
        }



        ///// <summary>
        ///// Image binarization with specific threshold
        ///// </summary>
        ///// <param name="image">Image to binarization</param>
        ///// <param name="value">Threshhold</param>
        //public static void SimpleBinarization(WriteableBitmap image, byte value)
        //{
        //    byte[] src = image.ToByteArray();
        //    for (int i = 0; i < src.Length; i += 4)
        //    {
        //        //int gray = (src[i] + src[i + 1] + src[i + 2]) / 3;
        //        int gray = src[i];
        //        if (gray > value) src[i] = src[i + 1] = src[i + 2] = 255;
        //        else src[i] = src[i + 1] = src[i + 2] = 0;
        //    }
        //    image.FromByteArray(src);

        //}


        /// <summary>
        /// Simple image binarization
        /// </summary>
        /// <param name="image">RGB array representing the image</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="value">threshold for the binarization</param>
        public static void SimpleBinarization(RGB[,] image, int width, int height, byte value)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int val = image[x, y].R;
                    if (val > value) image[x, y] = new RGB(Colors.White);
                    else image[x, y] = new RGB(Colors.Black);
                }
            }
        }


        ///// <summary>
        ///// Image binarization with Otsu method
        ///// </summary>
        ///// <param name="image">Image to binarization</param>
        //public static void OtsuBinarization(WriteableBitmap image)
        //{
        //    int[] H = new int[256];
        //    int number_of_pixels = image.PixelWidth * image.PixelHeight;
        //    int val = 0;

        //    byte[] src = image.ToByteArray();
        //    for (int i = 0; i < src.Length; i += 4)
        //    {
        //        val = src[i];
        //        H[val]++;
                
        //    }

        //    int SU = 0;
        //    for (int i = 0; i < 256; ++i) SU += i * H[i];

        //    double R = 0, MAX = 0, SG = 0, SD = 0, W = 0, SUP = 0, WP = 0, T1 = 0, T2 = 0;

        //    for (int i = 0; i < 256; ++i)
        //    {
        //        W += H[i];
        //        if (W == 0) continue;
        //        WP = number_of_pixels - W;
        //        if (WP == 0) break;
        //        SUP += i * H[i];
        //        SG = SUP / W;
        //        SD = (SU - SUP) / WP;
        //        R = W * WP * Math.Pow((SG - SD), 2);
        //        if (R >= MAX)
        //        {
        //            T1 = i;
        //            if (R > MAX) T2 = i;
        //            MAX = R;
        //        }
        //    }

        //    double threshold = (T1 + T2) / 2.0;

        //    for (int i = 0; i < src.Length; i += 4)
        //    {
        //        if (src[i] < threshold) src[i] = src[i + 1] = src[i + 2] = 0;
        //        else src[i] = src[i + 1] = src[i + 2] = 255;

        //    }

        //    image.FromByteArray(src);

        //}



        ///// <summary>
        ///// Operation of erosion on the image
        ///// </summary>
        ///// <param name="image">Image to process</param>
        ///// <param name="mask_size">size of the pixel neighborhood (for example: 3x3 mask, size is 3 etc.)</param>
        //public static void Erosion(WriteableBitmap image, int mask_size)
        //{
        //    byte[] src = image.ToByteArray();
        //    byte[] copy = (byte[])src.Clone();
        //    for (int i = 0; i < src.Length; i += 4)
        //    {
        //        bool check = false;

        //        for (int x = 0; x < mask_size; ++x)
        //        {
        //            for (int y = 0; y < mask_size; ++y)
        //            {
        //                int X = ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[0] + x - 1;
        //                int Y = ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[1] + y - 1;
        //                if (X < 0) X = 0;
        //                if (X > image.PixelWidth - 1) X = image.PixelWidth - 1;
        //                if (Y < 0) Y = 0;
        //                if (Y > image.PixelHeight - 1) Y = image.PixelHeight - 1;

        //                if ((X != ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[0] || Y != ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[1])
        //                    && copy[ByteArrayUtil.GetIndexFromCords(X,Y,image.PixelWidth)] == 255) check = true;
        //            }
        //        }

        //        if (check) src[i] = src[i + 1] = src[i + 2] = 255;

        //    }

        //    image.FromByteArray(src);
        //}


        //public static void Dilation(WriteableBitmap image, int mask_size)
        //{
        //    byte[] src = image.ToByteArray();
        //    byte[] copy = (byte[])src.Clone();
        //    for (int i = 0; i < src.Length; i += 4)
        //    {
        //        bool check = false;



        //        for (int x = 0; x < mask_size; ++x)
        //        {
        //            for (int y = 0; y < mask_size; ++y)
        //            {
        //                int X = ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[0] + x - 1;
        //                int Y = ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[1] + y - 1;
        //                if (X < 0) X = 0;
        //                if (X > image.PixelWidth - 1) X = image.PixelWidth - 1;
        //                if (Y < 0) Y = 0;
        //                if (Y > image.PixelHeight - 1) Y = image.PixelHeight - 1;

        //                if ((X != ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[0] || Y != ByteArrayUtil.GetCordsFromIndex(i, image.PixelWidth)[1])
        //                    && copy[ByteArrayUtil.GetIndexFromCords(X, Y, image.PixelWidth)] == 0) check = true;
        //            }
        //        }

        //        if (check) src[i] = src[i + 1] = src[i + 2] = 0;

        //    }

        //    image.FromByteArray(src);
        //}


        public static void EraseOnePixelNoise(bool[,] image, int width, int height)
        {
            bool[,] copy = (bool[,])image.Clone();

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (copy[x, y] == false)
                    {
                        bool check = true;

                        for (int i = 0; i < 3; ++i)
                        {
                            for (int j = 0; j < 3; ++j)
                            {
                                int X = x + i - 1;
                                int Y = y + j - 1;
                                if (X < 0) X = 0;
                                if (X > width - 1) X = width - 1;
                                if (Y < 0) Y = 0;
                                if (Y > height - 1) Y = height - 1;

                                if ((X != x || Y != y) && copy[X, Y] == false) check = false;
                            }
                        }
                        if (check) image[x, y] = true;
                    }
                }
            }
        }




    }



}
