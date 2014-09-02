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


        public static void OtsuBinarization(RGB[,] image, int width, int height)
        {
            int[] H = new int[256];
            int number_of_pixels = width * height;
            int val = 0;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    val = image[x, y].R;
                    H[val]++;
                }
            }

            int SU = 0;
            for (int i = 0; i < 256; ++i) SU += i * H[i];

            double R = 0, MAX = 0, SG = 0, SD = 0, W = 0, SUP = 0, WP = 0, T1 = 0, T2 = 0;

            for (int i = 0; i < 256; ++i)
            {
                W += H[i];
                if (W == 0) continue;
                WP = number_of_pixels - W;
                if (WP == 0) break;
                SUP += i * H[i];
                SG = SUP / W;
                SD = (SU - SUP) / WP;
                R = W * WP * Math.Pow((SG - SD), 2);
                if (R >= MAX)
                {
                    T1 = i;
                    if (R > MAX) T2 = i;
                    MAX = R;
                }
            }

            double threshold = (T1+T2) / 2.0;

            for(int x=0; x<width; ++x) {
                for(int y=0; y<height; ++y) {
                    if(image[x,y].R < threshold) image[x,y] = new RGB(Colors.Black);
                    else image[x,y] = new RGB(Colors.White);
                }
            }

        }



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


        /// <summary>
        /// delete single black pixels on the image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
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


        public static void Dilation(bool[,] image, int width, int height)
        {


            bool[,] copy = (bool[,])image.Clone();

            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {

                    image[i, j] = true;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            image[i, j] &= copy[i + k, j + l];
                        }
                    }


                }
            }
        }


        public static void Erosion(bool[,] image, int width, int height)
        {
            bool[,] copy = (bool[,])image.Clone();


            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {

                    image[i, j] = false;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            image[i, j] |= copy[i + k, j + l];
                        }
                    }


                }
            }
        }


        public static void Blur(RGB[,] image, int width, int height, int id) {

            RGB[,] copy = ByteArrayUtil.RGB_ArrayClone(image, width, height);

            int[, ,] maski = {
                             {{1,1,1},{1,1,1},{1,1,1} },
                             {{1,1,1},{1,2,1},{1,1,1} },
                             {{1,2,1},{2,4,2},{1,2,1} }
                             };

            int sum;
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    int[] wynik = new int[3];
                    wynik[0] = wynik[1] = wynik[2] = 0;
                    sum = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            sum += maski[id, k + 1, l + 1];
                            wynik[0] += copy[i + k, j + l].R * maski[id, k + 1, l + 1];
                            wynik[1] += copy[i + k, j + l].G * maski[id, k + 1, l + 1];
                            wynik[2] += copy[i + k, j + l].B * maski[id, k + 1, l + 1];

                        }
                    }

                    byte r = (byte)(wynik[0] / sum);
                    byte g = (byte)(wynik[1] / sum);
                    byte b = (byte)(wynik[2] / sum);
                    image[i, j] = new RGB(r, g, b, 255);
                }
            }
        }


        public static void MakeSharper(RGB[,] image, int width, int height, int id)
        {
            RGB[,] copy = ByteArrayUtil.RGB_ArrayClone(image, width, height);

            int[, ,] maski = {
                             {{0,-1,0},{-1,5,-1},{0,-1,0} },
                             {{-1,-1,-1},{-1,9,-1},{-1,-1,-1} },
                             {{1,-2,1},{-2,5,-2},{1,-2,1} }
                             };

            int sum = 0;
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    int[] wynik = new int[3];
                    wynik[0] = wynik[1] = wynik[2] = 0;
                    sum = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            sum += maski[id, k + 1, l + 1];
                            wynik[0] += copy[i + k, j + l].R * maski[id, k + 1, l + 1];
                            wynik[1] += copy[i + k, j + l].G * maski[id, k + 1, l + 1];
                            wynik[2] += copy[i + k, j + l].B * maski[id, k + 1, l + 1];

                        }
                    }
                    for (int k = 0; k < 3; k++) if (wynik[k] > 255) wynik[k] = 255;
                    for (int k = 0; k < 3; k++) if (wynik[k] < 0) wynik[k] = 0;
                    byte r = (byte)(wynik[0] / sum);
                    byte g = (byte)(wynik[1] / sum);
                    byte b = (byte)(wynik[2] / sum);
                    image[i, j] = new RGB(r, g, b, 255);
                }
            }
        }


        public static void AverageFilter(RGB[,] image, int width, int height, int mask_size)
        {

            int size = (2 * mask_size + 1) * (2 * mask_size + 1);
            RGB[,] copy = ByteArrayUtil.RGB_ArrayClone(image, width, height);
            for (int i = mask_size; i < width - mask_size; i++)
            {
                for (int j = mask_size; j < height - mask_size; j++)
                {
                    int[] sum = new int[3];
                    for (int k = -mask_size; k <= mask_size; k++)
                    {
                        for (int l = -mask_size; l <= mask_size; l++)
                        {
                            sum[0] += copy[i + k, j + l].R;
                            sum[1] += copy[i + k, j + l].G;
                            sum[2] += copy[i + k, j + l].B;
                        }
                    }
                    byte r = (byte)(sum[0] / size);
                    byte g = (byte)(sum[1] / size);
                    byte b = (byte)(sum[2] / size);
                    image[i, j] = new RGB(r, g, b, 255);
                }
            }
        }


        public static void MedianFilter(RGB[,] image, int width, int height, int mask_size)
        {

            int size = (2 * mask_size + 1) * (2 * mask_size + 1);
            int middle = (int)(size / 2);
            RGB[,] copy = ByteArrayUtil.RGB_ArrayClone(image, width, height);
            for (int i = mask_size; i < width - mask_size; i++)
            {
                for (int j = mask_size; j < height - mask_size; j++)
                {
                    List<byte> listR = new List<byte>();
                    List<byte> listG = new List<byte>();
                    List<byte> listB = new List<byte>();
                    for (int k = -mask_size; k <= mask_size; k++)
                    {
                        for (int l = -mask_size; l <= mask_size; l++)
                        {
                            listR.Add(copy[i + k, j + l].R);
                            listG.Add(copy[i + k, j + l].G);
                            listB.Add(copy[i + k, j + l].B);
                        }
                    }
                    listR.Sort();
                    listB.Sort();
                    listG.Sort();
                    byte r = listR[middle];
                    byte g = listG[middle];
                    byte b = listB[middle];

                    image[i, j] = new RGB(r, g, b, 255);


                }
            }
        }


        public static void HistogramStrech(RGB[,] image, int width, int height)
        {
            int min_r = 255, max_r = 0, min_g = 255, max_g = 0, min_b = 255, max_b = 0;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (image[x, y].R < min_r) min_r = image[x, y].R;
                    if (image[x, y].R > max_r) max_r = image[x, y].R;

                    if (image[x, y].G < min_g) min_g = image[x, y].G;
                    if (image[x, y].G > max_g) max_g = image[x, y].G;

                    if (image[x, y].B < min_b) min_b = image[x, y].B;
                    if (image[x, y].B > max_b) max_b = image[x, y].B;
                }
            }

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    image[x, y].R = (byte)((image[x, y].R - min_r) * 255 / (max_r - min_r));
                    image[x, y].G = (byte)((image[x, y].G - min_g) * 255 / (max_g - min_g));
                    image[x, y].B = (byte)((image[x, y].B - min_b) * 255 / (max_b - min_b));
                }
            }
        }



        public static void kuhawaraGrayFilter(RGB[,] image, int width, int height, int mask_size)
        {

            int quarter_size = (mask_size + 1) * (mask_size + 1);
            int size = (2 * mask_size + 1) * (2 * mask_size + 1);

            RGB[,] copy = ByteArrayUtil.RGB_ArrayClone(image, width, height);
            for (int i = mask_size; i < width - mask_size; i++)
            {
                for (int j = mask_size; j < height - mask_size; j++)
                {
                    int[] average = new int[4];
                    double[] W = new double[4];
                    int[] sum = new int[4];
                    //pierwsza cwiartka
                    for (int k = -mask_size; k <= 0; k++)
                    {
                        for (int l = -mask_size; l <= 0; l++)
                        {
                            sum[0] += copy[i + k, j + l].R;
                        }
                    }
                    //druga cwiartka
                    for (int k = 0; k <= mask_size; k++)
                    {
                        for (int l = -mask_size; l <= 0; l++)
                        {
                            sum[1] += copy[i + k, j + l].R;
                        }
                    }
                    //trzecia cwiartka
                    for (int k = -mask_size; k <= 0; k++)
                    {
                        for (int l = 0; l <= mask_size; l++)
                        {
                            sum[2] += copy[i + k, j + l].R;
                        }
                    }
                    //czwarta cwiartka
                    for (int k = 0; k <= mask_size; k++)
                    {
                        for (int l = 0; l <= mask_size; l++)
                        {
                            sum[3] += copy[i + k, j + l].R;
                        }
                    }

                    //liczenie srednich

                    for (int l = 0; l < 4; l++)
                    {
                        average[l] = sum[l] / quarter_size;
                    }



                    ///////liczenie wariancji
                    //pierwsza cwiartka
                    for (int k = -mask_size; k <= 0; k++)
                    {
                        for (int l = -mask_size; l <= 0; l++)
                        {
                            W[0] += Math.Pow(average[0] - copy[i + k, j + l].R, 2);
                        }
                    }
                    //druga cwiartka
                    for (int k = 0; k <= mask_size; k++)
                    {
                        for (int l = -mask_size; l <= 0; l++)
                        {
                            W[1] += Math.Pow(average[1] - copy[i + k, j + l].R, 2);
                        }
                    }
                    //trzecia cwiartka
                    for (int k = -mask_size; k <= 0; k++)
                    {
                        for (int l = 0; l <= mask_size; l++)
                        {
                            W[2] += Math.Pow(average[2] - copy[i + k, j + l].R, 2);
                        }
                    }
                    //czwarta cwiartka
                    for (int k = 0; k <= mask_size; k++)
                    {
                        for (int l = 0; l <= mask_size; l++)
                        {
                            W[3] += Math.Pow(average[3] - copy[i + k, j + l].R, 2);
                        }
                    }
                    //dzielenie wariancji
                    for (int k = 0; k < 4; k++) W[k] = W[k] / quarter_size;

                    double min = W[0];
                    int id = 0;
                    for (int k = 1; k < 4; k++)
                        if (W[k] < min)
                        {
                            min = W[k];
                            id = k;
                        }
                    image[i, j] = new RGB((byte)average[id]);


                }
            }

        }





    }



}
