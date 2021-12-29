using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SendFileClientToServerWithTCP.Additional
{
   public  class PdfHelper
    {
        public static Byte[]getBytes(string path)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return bytes;

        }

        public static string getPdfPath(byte[] buffer) {

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, buffer);
            buffer = ms.ToArray();
            string path = @"C:\Users\mehsu\source\repos\SendFileClientToServerWithTCP\Server\bin\Debug\test2.pdf";
            File.WriteAllBytes(path,buffer);


            return path;

        }

        public static string GetTextFromPDF(string path)
        {
            string text;
            PdfReader reader = new PdfReader(path);

            using (StringWriter output = new StringWriter())
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                    output.WriteLine(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));

                reader.Close();
                text = output.ToString();
            }
            return text;
        }

    }
}
