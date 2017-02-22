using System;
using ImageSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

namespace JsonGSharp
{
    public class JsonGConvert
    {
        private JsonGConvert() { }

        public static JsonGSchema GetSchema(Image image)
        {
            var sPixels = new List<JsonGPixel>(image.Pixels.Length);
            using (var pixels = image.Lock())
            {
                for (int x = 0; x < pixels.Width; x++)
                {
                    for (int y = 0; y < pixels.Height; y++)
                    {
                        var pixel = pixels[x, y];
                        sPixels.Add(new JsonGPixel
                        {
                            Position = new JsonGPosition { X = (uint)x, Y = (uint)y },
                            Color = JsonGColor.FromColor(pixel)
                        });
                    }
                }
            }
            return new JsonGSchema
            {
                Version = 1.0d,
                Transparent = true,
                Size = new JsonGSize { Width = (uint)image.Width, Height = (uint)image.Height },
                Layers = new[]
                {
                    new JsonGLayer { Pixels = sPixels.ToArray() }
                }
            };
        }
        public static string GetString(Image image)
        {
            return JsonConvert.SerializeObject(GetSchema(image));
        }

        public static Image FromRaw(string json)
        {
            var schema = JsonConvert.DeserializeObject<JsonGSchema>(json);
            var width = (int)schema.Size.Width;
            var height = (int)schema.Size.Height;

            if (schema.Layers.Count() > 1) throw new NotSupportedException("Only one layer is supported.");
            var sPixels = schema.Layers.FirstOrDefault()?.Pixels ?? throw new NotSupportedException("At least one layer must be specified");
            var _sColor = schema.Layers.FirstOrDefault().DefaultColor;
            var defaultColor = new Color(_sColor.Red, _sColor.Green, _sColor.Blue, _sColor.Alpha.GetValueOrDefault());

            var image = new Image(width, height);
            using (var pixels = image.Lock())
            {
                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var pixel = sPixels.FirstOrDefault(p => p.Position.X == x && p.Position.Y == y);
                        pixels[x, y] = pixel != null ? new Color(pixel.Color.Red, pixel.Color.Green, pixel.Color.Blue, pixel.Color.Alpha.GetValueOrDefault()) : defaultColor;
                    }
                }
            }
            return image;
        }
        public static Image FromStream(Stream stream)
        {
            var text = new StreamReader(stream).ReadToEnd();
            return FromRaw(text);
        }
    }
}
