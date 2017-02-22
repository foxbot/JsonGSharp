using System;
using JsonGSharp;
using ImageSharp;
using System.IO;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                Console.WriteLine("Converting 'sample.png' to 'sample.jsng'");
                var image = new Image("sample.png");
                var json = JsonGConvert.GetString(image);
                File.WriteAllText("sample.jsng", json);
                Console.WriteLine("OK");
            }
            {
                Console.WriteLine("Converting 'sample2.jsng' to 'sample2.png'");
                var image = JsonGConvert.FromStream(File.Open("sample2.jsng", FileMode.Open));
                image.Save("sample2.png");
                Console.WriteLine("Done!");
            }
        }
    }
}