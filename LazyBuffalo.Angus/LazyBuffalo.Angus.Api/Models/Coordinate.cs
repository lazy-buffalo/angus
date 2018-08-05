namespace LazyBuffalo.Angus.Api.Models
{
    public class Coordinate
    {
        public long Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public long GrazingId { get; set; }
        public Grazing Grazing { get; set; }
    }
}
