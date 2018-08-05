using Newtonsoft.Json;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class GoogleCoordinateDto
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}
