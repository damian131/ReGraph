using ReGraph.Models.GraphReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.Models.OCR
{
    class ByteArrayUtil
    {

        /// <summary>
        /// Get pixel (x,y) index of the byte array
        /// </summary>
        /// <param name="x">pixel X-cord</param>
        /// <param name="y">pixel Y-cord</param>
        /// <param name="width">image width</param>
        /// <returns></returns>
        public static int GetIndexFromCords(int x, int y, int width)
        {
            int index = (x + y * width) * 4;
            return index;
        }


        /// <summary>
        /// Get pixel cords from the byte array
        /// </summary>
        /// <param name="index">index in the byte array</param>
        /// <param name="width">image width</param>
        /// <returns></returns>
        public static int[] GetCordsFromIndex(int index, int width)
        {
            int[] cords = new int[2];

            int y = index / (width * 4);

            int x = (index % (width * 4)) / 4;

            cords[0] = x;
            cords[1] = y;

            return cords;
        }


        public static RGB[,] fromByteArray(byte[] array, int current_width, int current_height)
        {
            RGB[,] tab = new RGB[current_width, current_height];
            for (int i = 0; i < current_width; i++)
            {
                for (int j = 0; j < current_height; j++)
                {
                    int poz = j * (current_width) * 4 + i * 4;
                    tab[i, j] = new RGB(array[poz + 2], array[poz + 1], array[poz], array[poz + 3]);
                }
            }

            return tab;
        }

        public static RGB[,] fromWriteableBitmap(WriteableBitmap InputImage)
        {
            RGB[,] ImageData = new RGB[InputImage.PixelWidth, InputImage.PixelHeight];
            byte[] byteArray = InputImage.ToByteArray();
            int width = InputImage.PixelWidth;
            int height = InputImage.PixelHeight;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    int index = j * (width) * 4 + i * 4;
                    ImageData[i, j] = new RGB(byteArray[index + 2], byteArray[index + 1], byteArray[index]);
                }
            }
            return ImageData;
        }

        public static byte[] toByteArray(RGB[,] array, int current_width, int current_height)
        {
            int length = current_width * current_height * 4;
            byte[] tab = new byte[length];

            for (int i = 0; i < current_width; i++)
            {
                for (int j = 0; j < current_height; j++)
                {
                    int poz = j * (current_width) * 4 + i * 4;
                    for (int k = 0; k < 4; k++)
                    {
                        tab[poz + k] = array[i, j].toByteArray()[k];
                    }
                }

            }
            return tab;
        }


        public static bool[,] mask_from_RGB(RGB[,] in_array, int width, int height)
        {
            bool[,] out_mask = new bool[width, height];

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (in_array[i, j].isWhite()) out_mask[i, j] = true;
                    else out_mask[i, j] = false;
                }
            }

            return out_mask;
        }



        public static RGB[,] RGB_from_mask(bool[,] bool_array, int width, int height)
        {
            RGB[,] result = new RGB[width, height];

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (bool_array[i, j] == true) result[i, j] = new RGB(Colors.White);
                    else result[i, j] = new RGB(Colors.Black);
                }
            }

            return result;
        }



        public static RGB[,] RGB_ArrayClone(RGB[,] image, int width, int height)
        {
            RGB[,] result = new RGB[width, height];

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    result[i, j] = image[i, j].Clone();
                }
            }
            return result;
        }


    }
}
