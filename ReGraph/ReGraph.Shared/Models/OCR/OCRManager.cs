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

            //Utility.SimpleBinarization(ImageRGB, width, height, 100);
            Utility.OtsuBinarization(ImageRGB, width, height);

            ImageBool = ByteArrayUtil.mask_from_RGB(ImageRGB, width, height);

            Utility.EraseOnePixelNoise(ImageBool, width, height);

            lines = ImageDivide.DivideOnLines(ImageBool, width, height);

            letters = ImageDivide.DivideOnLetters(lines[0], lines[0].GetLength(0), lines[0].GetLength(1));

            


            //Utility.ShowDialog("" + letters.Count + "," + letters[0].GetLength(0) + "," + letters[0].GetLength(1));

            Save(letters);




            /////////////// TO SAVE IMAGE //////////////////////////////////

            //ImageRGB = ByteArrayUtil.RGB_from_mask(ImageBool, width, height);

            //byte[] src = ByteArrayUtil.toByteArray(ImageRGB, width, height);

            //copy.FromByteArray(src);
            

            //SaveData(copy,"obrazek","jpg");

            return "cos";

        }



        async private void SaveWithFP(WriteableBitmap bmp, String file_name)
        {
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("JPG File", new List<string>() { ".jpg" });
            StorageFile file = await picker.PickSaveFileAsync();

            //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///images/"+file_name+".jpg"));

            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
            Stream pixelStream = bmp.PixelBuffer.AsStream();
            byte[] pixels = new byte[pixelStream.Length];
            await pixelStream.ReadAsync(pixels, 0, pixels.Length);

            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bmp.PixelWidth, (uint)bmp.PixelHeight, 96.0, 96.0, pixels);
            await encoder.FlushAsync();
        }


        public static async void SaveData(WriteableBitmap bmp, string file_name, string extension)
        {

            string full_name = file_name + "." + extension;

            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Optionally overwrite any existing file with CreationCollisionOption
            StorageFile file = await localFolder.CreateFileAsync(full_name, CreationCollisionOption.ReplaceExisting);
            

            try
            {
                if (file != null)
                {
                    IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                    Stream pixelStream = bmp.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bmp.PixelWidth, (uint)bmp.PixelHeight, 96.0, 96.0, pixels);
                    await encoder.FlushAsync();
                }
            }

            catch (FileNotFoundException)
            {
                // Error saving data
            }

        }

        private void Save(List<bool[,]> images) {
            for(int i=0; i<images.Count; ++i) {
            RGB[,] ImRGB = ByteArrayUtil.RGB_from_mask(images[i], images[i].GetLength(0), images[i].GetLength(1));

            byte[] src = ByteArrayUtil.toByteArray(ImRGB, ImRGB.GetLength(0), ImRGB.GetLength(1));

            WriteableBitmap cop = new WriteableBitmap(ImRGB.GetLength(0), ImRGB.GetLength(1));
            cop.FromByteArray(src);
            

            SaveData(cop,""+i,"jpg");
            }
        }


    }
}
