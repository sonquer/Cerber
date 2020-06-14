using AutoMapper;
using Availability.Api.Application.Dtos;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;

namespace Availability.Api.Application.Mappings
{
    public class DefaultDomainMapping : Profile
    {
        public DefaultDomainMapping()
        {
            CreateMap<AvailabilityRecord, AvailabilityRecordDto>();
            CreateMap<AvailabilityLog, AvailabilityLogDto>();
            CreateMap<AvailabilityRecord, AvailabilityListItemDto>();
        }
    }
}