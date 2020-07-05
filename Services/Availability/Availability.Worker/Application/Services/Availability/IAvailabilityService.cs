using System.Threading;
using System.Threading.Tasks;
using Availability.Worker.Application.Services.Availability.Models;

namespace Availability.Worker.Application.Services.Availability
{
    public interface IAvailabilityService
    {
        Task<AvailabilityResponseModel> Request(string url, CancellationToken cancellationToken);
    }
}