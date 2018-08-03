using System;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class CowLocationDto
    {
        public long CowId { get; set; }
        public string CowName { get; set; }
        public double Longitude { get; set; }
        public double Latidude { get; set; }
        public DateTime LocationDateTime { get; set; }
    }
}

