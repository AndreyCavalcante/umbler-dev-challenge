using Desafio.Umbler.Dtos;
using System.Threading.Tasks;
using Whois.NET;

namespace Desafio.Umbler.Infra;

public class WhoisService : IWhoisService
{
    public async Task<WhoisResult> QueryAsync(string query)
    {
        var response = await WhoisClient.QueryAsync(query);

        return new WhoisResult
        {
            Raw = response.Raw,
            OrganizationName = response.OrganizationName
        };
    }
}