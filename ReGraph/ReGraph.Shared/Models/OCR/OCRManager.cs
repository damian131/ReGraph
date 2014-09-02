using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using ReGraph.Models.GraphReader;

namespace ReGraph.Models.OCR
{
    public class OCRManager
    {

        private RGB[,] ImageRGB;
        private int width;
        private int height;
        private bool[,] ImageBool;
        private List<bool[,]> lines;
        private List<bool[,]> letters;


        public string Recognize(WriteableBitmap image)
        {

            WriteableBitmap copy = image.Clone();

            width = image.PixelWidth;
            height = image.PixelHeight;

            ImageRGB = ByteArrayUtil.fromWriteableBitmap(image);

            Utility.HistogramStrech(ImageRGB, width, height); 

            Utility.GrayScale(ImageRGB, width, height);

            Utility.SimpleBinarization(ImageRGB, width, height, 100);

            ImageBool = ByteArrayUtil.mask_from_RGB(ImageRGB, width, height);

            Utility.EraseOnePixelNoise(ImageBool, width, height);

            lines = ImageDivide.DivideOnLines(ImageBool, width, height);

            letters = ImageDivide.DivideOnLetters(lines[0], lines[0].GetLength(0), lines[0].GetLength(1));


            //Utility.ShowDialog("" + letters.Count + "," + letters[0].GetLength(0) + "," + letters[0].GetLength(1));






            /////////////// TO SAVE IMAGE //////////////////////////////////

            ImageRGB = ByteArrayUtil.RGB_from_mask(ImageBool, width, height);

            byte[] src = ByteArrayUtil.toByteArray(ImageRGB, width, height);

            copy.FromByteArray(src);
            

            //Save(copy);

            return "cos";

        }



        async private void Save(WriteableBitmap bmp)
        {
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("JPG File", new List<string>() { ".jpg" });
            StorageFile file = await picker.PickSaveFileAsync();

            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
            Stream pixelStream = bmp.PixelBuffer.AsStream();
            byte[] pixels = new byte[pixelStream.Length];
            await pixelStream.ReadAsync(pixels, 0, pixels.Length);

            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bmp.PixelWidth, (uint)bmp.PixelHeight, 96.0, 96.0, pixels);
            await encoder.FlushAsync();
        }


    }
}
