using Newtonsoft.Json;
using System;

namespace LazyBuffalo.Angus.Api.Dtos
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
        public float Snr { get; set; }

        [JsonProperty("rf_chain")]
        public int RfChain { get; set; }

        [JsonProperty("latitude")]
        public float Latitude { get; set; }

        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        [JsonProperty("altitude")]
        public float Altitude { get; set; }
    }
}