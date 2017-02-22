using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JsonGSharp
{
    public class JsonGObject
    {
        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }
    }

    public class JsonGSchema : JsonGObject
    {
        [JsonProperty("version"), JsonRequired]
        public double Version
        {
            get
            {
                return 1.0d;
            }
            set
            {
                if (value != 1.0d)
                {
                    throw new NotSupportedException($"JsonG {value} is not supported; only 1.0 is supported.");
                }
            }
        }
        [JsonProperty("transparency"), JsonRequired]
        public bool Transparent { get; set; }
        [JsonProperty("size"), JsonRequired]
        public JsonGSize Size { get; set; }
        [JsonProperty("layers"), JsonRequired]
        public IEnumerable<JsonGLayer> Layers { get; set; }

    }

    public class JsonGSize : JsonGObject
    {
        [JsonProperty("width"), JsonRequired]
        public uint Width { get; set; }
        [JsonProperty("height"), JsonRequired]
        public uint Height { get; set; }
    }

    public class JsonGLayer : JsonGObject
    {
        [JsonProperty("default_color")]
        public JsonGColor DefaultColor { get; set; } = JsonGColor.FromColor(new ImageSharp.Color(0, 0, 0));
        [JsonProperty("pixels")]
        public IEnumerable<JsonGPixel> Pixels { get; set; }
    }

    public class JsonGColor : JsonGObject
    {
        [JsonProperty("red"), JsonRequired]
        public byte Red { get; set; } = 0;
        [JsonProperty("blue"), JsonRequired]
        public byte Blue { get; set; } = 0;
        [JsonProperty("green"), JsonRequired]
        public byte Green { get; set; } = 0;
        /// <summary> The Alpha value may be null if the image is transparent </summary>
        [JsonProperty("alpha"), JsonRequired]
        public byte? Alpha { get; set; } = 255;

        public static JsonGColor FromColor(ImageSharp.Color color)
        {
            return new JsonGColor
            {
                Red = color.R,
                Green = color.G,
                Blue = color.B,
                Alpha = color.A,
            };
        }
    }

    public class JsonGPixel : JsonGObject
    {
        [JsonProperty("position"), JsonRequired]
        public JsonGPosition Position { get; set; }
        [JsonProperty("color"), JsonRequired]
        public JsonGColor Color { get; set; }
    }

    public class JsonGPosition : JsonGObject
    {
        [JsonProperty("x"), JsonRequired]
        public uint X { get; set; }
        [JsonProperty("y"), JsonRequired]
        public uint Y { get; set; }
    }
}
