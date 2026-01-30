using Desafio.Umbler.Dtos;
using System.Threading.Tasks;

namespace Desafio.Umbler.Services;

public interface IDomainService
{
    Task<DomainResultDto> GetDomainAsync(string domain);
}
