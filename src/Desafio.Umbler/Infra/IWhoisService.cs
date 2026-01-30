using Desafio.Umbler.Dtos;
using System.Threading.Tasks;
using Whois.NET;

namespace Desafio.Umbler.Infra;

public interface IWhoisService
{
    Task<WhoisResult> QueryAsync(string value);
}