﻿using System;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class LocationDto : ICowDataEntry
    {
        public long Id { get; set; }
        public long CowId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime DateTime { get; set; }
    }
}
