using Desafio.Umbler.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desafio.Umbler.Infra;

public interface IDnsService
{
    Task<DnsResult> ResolveAsync(string domain);
    Task<List<string>> GetNameServersAsync(string domain);
}
