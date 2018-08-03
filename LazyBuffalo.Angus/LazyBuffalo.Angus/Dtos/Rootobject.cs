using Newtonsoft.Json;

namespace LazyBuffalo.Angus.Dtos
{
    public class Rootobject
    {

        [JsonProperty("app_id")]
        public string AppId { get; set; }

        [JsonProperty("dev_id")]
        public string DevId { get; set; }

        [JsonProperty("hardware_serial")]
        public string HardwareSerial { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("counter")]
        public int Counter { get; set; }

        [JsonProperty("is_retry")]
        public bool IsRetry { get; set; }

        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        [JsonProperty("payload_raw")]
        public string PayloadRaw { get; set; }

        [JsonProperty("payload_fields")]
        public PayloadFields PayloadFields { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("downlink_url")]
        public string DownlinkUrl { get; set; }
    }
}