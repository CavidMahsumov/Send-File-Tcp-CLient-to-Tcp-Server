using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendFileClientToServerWithTCP.Additional
{
    public class ImageHelper
    {
        public static void CreateIfMissing(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string GetImagePath(byte[] buffer, int counter)
        {

            //Image x = (Bitmap)((new ImageConverter()).ConvertFrom(buffer));
            Image x = (Bitmap)((new ImageConverter()).ConvertFrom(buffer));
            Image bitmap1 = new Bitmap(x);
            bitmap1.Save($@"C:\Users\mehsu\source\repos\SendFileClientToServerWithTCP\Server\bin\Debug\image{counter}.png");
            var imagepath = $@"C:\Users\mehsu\source\repos\SendFileClientToServerWithTCP\Server\bin\Debug\image{counter}.png";
            return imagepath;
        }
        public static byte[] GetBytesOfImage(string path)
        {
            byte[] b = File.ReadAllBytes(path);
            return b;
        }

    }
}
