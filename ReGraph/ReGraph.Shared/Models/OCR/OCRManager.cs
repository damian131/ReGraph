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
        private List<List<bool[,]>> letters = new List<List<bool[,]>>(); // contains lists with words


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

            for (int i = 0; i < lines.Count; ++i)
            {
                MergeLists(letters, ImageDivide.DivideOnLetters(lines[i], lines[i].GetLength(0), lines[i].GetLength(1)));
            }

            List<double> heightToWidth = Base.HeightToWidth(letters);
            List<double[]> blackToAll = Base.BlackToAll(letters);
            List<int> spaces = ImageDivide.CalculateSpaces(letters);

            string result = Base.CompareWithBase(heightToWidth, blackToAll, spaces);

            //Utility.ShowDialog(result);

            //SaveTXT(heightToWidth, blackToAll, Base.names);
            //Save1(heightToWidth, blackToAll, Base.names);
            //Save2(heightToWidth, blackToAll, Base.names);


            //Save(letters);
            //Save(ImageBool, "tekst");
            //Save(lines[0], "linia1");
            //Save(lines[1], "linia2");

            //Save(ImageBool, "przyklad1");



            return result;

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

            //Utility.ShowDialog(file.Path);    --- display dialog with path to file
            

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

        private void Save(bool[,] image, string name)
        {
            RGB[,] ImRGB = ByteArrayUtil.RGB_from_mask(image, image.GetLength(0), image.GetLength(1));

            byte[] src = ByteArrayUtil.toByteArray(ImRGB, ImRGB.GetLength(0), ImRGB.GetLength(1));

            WriteableBitmap cop = new WriteableBitmap(ImRGB.GetLength(0), ImRGB.GetLength(1));
            cop.FromByteArray(src);


            SaveData(cop, name, "jpg");
        }


        private void Save(List<List<bool[,]>> images)
        {
            for (int i = 0; i < images.Count; ++i)
            {
                for (int j = 0; j < images[i].Count; ++j)
                {
                    List<bool[,]> image = images[i];
                    RGB[,] ImRGB = ByteArrayUtil.RGB_from_mask(image[j], image[j].GetLength(0), image[j].GetLength(1));

                    byte[] src = ByteArrayUtil.toByteArray(ImRGB, ImRGB.GetLength(0), ImRGB.GetLength(1));

                    WriteableBitmap cop = new WriteableBitmap(ImRGB.GetLength(0), ImRGB.GetLength(1));
                    cop.FromByteArray(src);


                    SaveData(cop, "" + i + "_" + j, "jpg");
                }
            }
        }


        private void MergeLists(List<List<bool[,]>> first, List<List<bool[,]>> second)
        {
            for (int i = 0; i < second.Count; ++i)
            {
                first.Add(second[i]);
            }
        }


        private static async void SaveTXT(List<double> first, List<double[]> second, string[] names) {

            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.CreateFileAsync("base.txt", CreationCollisionOption.ReplaceExisting);

            List<string> text = new List<string>();

            for(int i=0; i<names.GetLength(0); ++i) {
                string s = "";
                s += names[i];
                s += ":   ";
                s += first[i];
                s += "  ,  (";
                s += (second[i])[0];
                s += ";";
                s += (second[i])[1];
                s += ";";
                s += (second[i])[2];
                s += ";";
                s += (second[i])[3];
                s += ")";
                //await Windows.Storage.FileIO.AppendTextAsync(sampleFile, s);
                text.Add(s);
                
            }
            await Windows.Storage.FileIO.AppendLinesAsync(sampleFile, text);

        }

        private static async void Save1(List<double> first, List<double[]> second, string[] names)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.CreateFileAsync("heighttowidth.txt", CreationCollisionOption.ReplaceExisting);

            string s = "";
            for (int i = 0; i < names.GetLength(0); ++i)
            {
                s += first[i];
                s += ",";

            }
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, s);
        }

        private static async void Save2(List<double> first, List<double[]> second, string[] names)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await folder.CreateFileAsync("blacktoall.txt", CreationCollisionOption.ReplaceExisting);

            string s = "";
            for (int i = 0; i < names.GetLength(0); ++i)
            {
                s += "{";
                s += (second[i])[0];
                s += ",";
                s += (second[i])[1];
                s += ",";
                s += (second[i])[2];
                s += ",";
                s += (second[i])[0];
                s += "}";
                s += ",";

            }
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, s);
        }

    


    }
}
