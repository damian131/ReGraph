using ReGraph.Models.GraphReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.Models.OCR
{
    class ImageDivide
    {
        public static List<bool[,]> DivideOnLines(bool[,] image, int width, int height)
        {

            List<bool[,]> lines = new List<bool[,]>();

            int actuall_y = 0;
            bool white = true;

            while (actuall_y < height)
            {
                int check_line = actuall_y;
                int start = 0, end = 0, new_height = 0;

                if (white)
                {
                    while (white && check_line < height)
                    {
                        for (int i = 0; i < width; ++i)
                        {
                            if (image[i, check_line] == false) white = false;
                        }
                        ++check_line;
                    }

                    start = check_line - 1;

                    while (!white && check_line < height)
                    {
                        white = true;
                        for (int i = 0; i < width; ++i)
                        {
                            if (image[i, check_line] == false) white = false;
                        }
                        ++check_line;
                    }

                    end = check_line - 2;

                    new_height = end - start + 1;

                }

                if (check_line < height)
                {
                    bool[,] new_image = new bool[width, new_height];

                    for (int x = 0; x < width; ++x)
                    {
                        for (int y = start; y <= end; ++y)
                        {
                            new_image[x, y - start] = image[x, y];
                        }
                    }

                    lines.Add(new_image);
                }

                actuall_y = check_line;
            }

            return lines;
        }

        public static List<List<bool[,]>> DivideOnLetters(bool[,] image, int width, int height)
        {
            List<bool[,]> word = new List<bool[,]>();
            List<List<bool[,]>> letters = new List<List<bool[,]>>();

            int actuall_x = 0;
            bool white = true;

            bool[,] new_image;

            int count_white_space = 0;

            //bool first_word = false;

            while (actuall_x < width)
            {
                int check_column = actuall_x;
                int start = 0, end = 0, new_width = 0;

                if (white)
                {
                    count_white_space = 0;
                    
                    while (white && check_column < width)
                    {
                        for (int i = 0; i < height; ++i)
                        {
                            if (image[check_column, i] == false) white = false;
                        }
                        ++check_column;
                        ++count_white_space;
                    }

                    start = check_column - 1;
                    

                    while (!white && check_column < width)
                    {
                        white = true;
                        for (int i = 0; i < height; ++i)
                        {
                            if (image[check_column, i] == false) white = false;
                        }
                        ++check_column;
                    }

                    end = check_column - 2;

                    new_width = end - start + 1;

                }

                if (check_column < width)
                {
                    new_image = new bool[new_width, height];

                    for (int x = start; x <= end; ++x)
                    {
                        for (int y = 0; y < height; ++y)
                        {
                            new_image[x-start, y] = image[x, y];
                        }
                    }
                    
                    if (count_white_space > 4 && word.Count != 0)
                    {
                        letters.Add(word);
                        word = new List<bool[,]>();
                    }
                    word.Add(new_image);

                }

                actuall_x = check_column;
            }

            letters.Add(word);

            return letters;
        }
    }
}
