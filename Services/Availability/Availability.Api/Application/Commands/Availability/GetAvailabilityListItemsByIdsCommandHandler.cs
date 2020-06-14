using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Availability.Api.Application.Dtos;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class GetAvailabilityListItemsByIdsCommandHandler : IRequestHandler<GetAvailabilityListItemsByIdsCommand, List<AvailabilityListItemDto>>
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        private readonly IMapper _mapper;

        public GetAvailabilityListItemsByIdsCommandHandler(IAvailabilityRecordRepository availabilityRecordRepository,
            IMapper mapper)
        {
            _availabilityRecordRepository = availabilityRecordRepository;
            _mapper = mapper;
        }
        
        public async Task<List<AvailabilityListItemDto>> Handle(GetAvailabilityListItemsByIdsCommand request, CancellationToken cancellationToken)
        {
            var result = new List<AvailabilityRecord>();
            foreach (var id in request.Ids)
            {
                result.Add(await _availabilityRecordRepository.GetById(id, cancellationToken));
            }

            return result.Select(_mapper.Map<AvailabilityListItemDto>).ToList();
        }
    }
}