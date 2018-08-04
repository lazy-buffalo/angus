using System.Collections.Generic;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class CowLocationDto
    {
        public long CowId { get; set; }
        public string CowName { get; set; }

        public List<LocationDto> Locations { get; set; }
    }
}

