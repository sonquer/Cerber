using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Availability.Api.Application.Dtos;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using MediatR;

namespace Availability.Api.Application.Commands.Availability
{
    public class GetAvailabilityRecordByIdCommandHandler : IRequestHandler<GetAvailabilityRecordByIdCommand, AvailabilityRecordDto>
    {
        private readonly IAvailabilityRecordRepository _availabilityRecordRepository;
        
        private readonly IMapper _mapper;

        public GetAvailabilityRecordByIdCommandHandler(IAvailabilityRecordRepository availabilityRecordRepository,
            IMapper mapper)
        {
            _availabilityRecordRepository = availabilityRecordRepository;
            _mapper = mapper;
        }
        
        public async Task<AvailabilityRecordDto> Handle(GetAvailabilityRecordByIdCommand request, CancellationToken cancellationToken)
        {
            var record = await _availabilityRecordRepository.GetById(request.Id, cancellationToken)
                .ConfigureAwait(false);

            var dto = _mapper.Map<AvailabilityRecordDto>(record);
            if(dto.AvailabilityLogs != null && dto.AvailabilityLogs.Count > 0)
            {
                dto.AvailabilityLogs = dto.AvailabilityLogs
                    .OrderByDescending(e => e.CreatedAt)
                    .ToList();
            }

            return dto;
        }
    }
}