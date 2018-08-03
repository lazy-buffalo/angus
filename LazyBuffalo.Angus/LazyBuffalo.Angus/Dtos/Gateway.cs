using Newtonsoft.Json;
using System;

namespace LazyBuffalo.Angus.Dtos
{
    public class Gateway
    {
        [JsonProperty("gtw_id")]
        public string GtwId { get; set; }

        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("channel")]
        public int Channel { get; set; }

        [JsonProperty("rssi")]
        public int Rssi { get; set; }

        [JsonProperty("snr")]
        public int Snr { get; set; }

        [JsonProperty("rf_chain")]
        public int RfChain { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("altitude")]
        public int Altitude { get; set; }
    }
}