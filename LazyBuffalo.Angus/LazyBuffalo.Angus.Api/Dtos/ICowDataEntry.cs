using System;

namespace LazyBuffalo.Angus.Api.Dtos
{
    public interface ICowDataEntry
    {
        long CowId { get; }
        DateTime DateTime { get; }
    }
}