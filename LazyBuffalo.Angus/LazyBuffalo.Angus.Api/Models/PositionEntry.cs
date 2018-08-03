using System;

namespace LazyBuffalo.Angus.Api.Models
{
    public class PositionEntry : IEntry
    {
        public long Id { get; set; }

        public bool IsUp { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public DateTime DateTime { get; set; }
        public long CowId { get; set; }
        public Cow Cow { get; set; }
    }
}
