using Newtonsoft.Json;
using System;

namespace LazyBuffalo.Angus.Dtos
{
    public class Metadata
    {

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("frequency")]
        public float Frequency { get; set; }

        [JsonProperty("modulation")]
        public string Modulation { get; set; }

        [JsonProperty("data_rate")]
        public string DataRate { get; set; }

        [JsonProperty("bit_rate")]
        public int BitRate { get; set; }

        [JsonProperty("coding_rate")]
        public string CodingRate { get; set; }

        [JsonProperty("gateways")]
        public Gateway[] Gateways { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("altitude")]
        public int Altitude { get; set; }
    }
}
