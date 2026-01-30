using Desafio.Umbler.Dtos;
using DnsClient;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Desafio.Umbler.Infra;

public class DnsService : IDnsService
{
    public async Task<DnsResult> ResolveAsync(string domain)
    {
        var lookup = new LookupClient();
        var result = await lookup.QueryAsync(domain, QueryType.ANY);

        var record = result.Answers.ARecords().FirstOrDefault();

        return new DnsResult
        {
            Ip = record?.Address?.ToString(),
            Ttl = record?.TimeToLive ?? 0
        };
    }

    public async Task<List<string>> GetNameServersAsync(string domain)
    {
        var lookup = new LookupClient
        {
            UseCache = false
        };

        var result = await lookup.QueryAsync(domain, QueryType.NS);

        return result.Answers
            .NsRecords()
            .Select(ns => ns.NSDName.Value.TrimEnd('.'))
            .ToList();
    }
}
