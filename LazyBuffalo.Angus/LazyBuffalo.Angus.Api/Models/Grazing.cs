using System.Collections.Generic;

namespace LazyBuffalo.Angus.Api.Models
{
    public class Grazing
    {
        public long Id { get; set; }
        public List<Coordinate> Coordinates { get; set; }

        public Grazing()
        {
            Coordinates = new List<Coordinate>();
        }
    }
}
