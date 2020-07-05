using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Availability.Api.Application.Claims;
using Availability.Api.Application.Dtos;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class GetAvailabilityListItemsCommandHandler : IRequestHandler<GetAvailabilityListItemsCommand, List<AvailabilityListItemDto>>
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        private readonly IClaimConverter _claimConverter;

        private readonly IMapper _mapper;

        public GetAvailabilityListItemsCommandHandler(IAvailabilityRecordRepository availabilityRecordRepository,
            IClaimConverter claimConverter,
            IMapper mapper)
        {
            _availabilityRecordRepository = availabilityRecordRepository;
            _claimConverter = claimConverter;
            _mapper = mapper;
        }
        
        public async Task<List<AvailabilityListItemDto>> Handle(GetAvailabilityListItemsCommand request, CancellationToken cancellationToken)
        {
            var accountId = _claimConverter.GetAccountId(request.ClaimsPrincipal);
            
            var availabilityRecords = await _availabilityRecordRepository.GetByAccountId(accountId, cancellationToken);

            return availabilityRecords.Select(_mapper.Map<AvailabilityListItemDto>).ToList();
        }
    }
}