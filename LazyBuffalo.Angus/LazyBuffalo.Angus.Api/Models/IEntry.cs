using System;

namespace LazyBuffalo.Angus.Api.Models
{
    public interface IEntry
    {
        DateTime DateTime { get; set; }

        long CowId { get; set; }
        Cow Cow { get; set; }
    }
}