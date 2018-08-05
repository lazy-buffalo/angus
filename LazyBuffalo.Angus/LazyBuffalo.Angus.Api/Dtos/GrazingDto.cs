using System.Collections.Generic;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public class GrazingDto
    {
        public long Id { get; set; }
        public List<GoogleCoordinateDto> Coordinates { get; set; }
    }
}
