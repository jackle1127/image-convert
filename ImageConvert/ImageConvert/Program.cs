using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageConvert
{
    class Program
    {
        const string CONVERT_ARG = "-c";

        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide arguments.");
                return -1;
            }

            switch (args[0])
            {
                case CONVERT_ARG:
                    return ConvertImage(args);
                    break;
            }

            return 0;
        }

        static string UniqueFileName(string filePath)
        {
            int i = 1;
            var currentFileName = filePath;
            var baseName = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
            var extension = Path.GetExtension(filePath);
            while (File.Exists(currentFileName))
            {
                currentFileName = $"{baseName} ({i}){extension}";
                i++;
            }
            return currentFileName;
        }

        private static Dictionary<string, (string extension, ImageFormat format)> IMAGE_FORMAT = new Dictionary<string, (string, ImageFormat)>()
        {
            { "bmp", ("bmp", ImageFormat.Bmp) },
            { "gif", ("gif", ImageFormat.Gif) },
            { "icon", ("ico", ImageFormat.Icon) },
            { "jpg", ("jpg", ImageFormat.Jpeg) },
            { "png", ("png", ImageFormat.Png) },
            { "tiff", ("tiff", ImageFormat.Tiff) },
        };

        static int ConvertImage(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Please provide 2 more arguments: Image format, Image file name.");
                return -1;
            }

            var destinationFormat = args[1].ToLower();
            if (!IMAGE_FORMAT.Keys.Contains(destinationFormat))
            {
                var message = "Image format not supported. Supported output image formats: ";
                message += string.Join(", ", IMAGE_FORMAT.Keys);
                Console.WriteLine(message);
            }

            var fileInfo = new FileInfo(args[2]);
            var image = Image.FromFile(fileInfo.FullName);

            var outputInfo = IMAGE_FORMAT[destinationFormat];
            var outputFileName = Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.Name));
            outputFileName += "." + outputInfo.extension;
            outputFileName = UniqueFileName(outputFileName);

            image.Save(outputFileName, outputInfo.format);
            image.Dispose();

            return 0;
        }
    }
}
