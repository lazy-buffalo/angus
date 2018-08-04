namespace LazyBuffalo.Angus.Api.Dtos
{
    public class CowLastLocationDto
    {
        public long CowId { get; set; }
        public string CowName { get; set; }

        public LocationDto Location { get; set; }
    }
}

