using Desafio.Umbler.Dtos;
using Desafio.Umbler.Infra;
using Desafio.Umbler.Models;
using DnsClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Umbler.Services;

public class DomainService : IDomainService
{
    private readonly DatabaseContext _db;
    private readonly IDnsService _dns;
    private readonly IWhoisService _whois;

    public DomainService(DatabaseContext db, IDnsService dns, IWhoisService whois)
    {
        _db = db;
        _dns = dns;
        _whois = whois;
    }

    private bool IsValidDomain(string domain)
    {
        if (string.IsNullOrWhiteSpace(domain))
            return false;

        var regex = @"^(?:[a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,}$";
        return System.Text.RegularExpressions.Regex.IsMatch(domain, regex);
    }

    public async Task<DomainResultDto> GetDomainAsync(string domainName)
    {

        if (string.IsNullOrWhiteSpace(domainName))
            throw new Exception("Nenhum domínio informado");

        if (!IsValidDomain(domainName))
            throw new Exception("Domínio fora do padrão");

        var domain = await _db.Domains.FirstOrDefaultAsync(d => d.Name == domainName);

        var mustRefresh = domain == null ||
            DateTime.Now.Subtract(domain.UpdatedAt).TotalSeconds > domain.Ttl;

        var nameServers = await _dns.GetNameServersAsync(domainName);

        if (!nameServers.Any())
        {
            nameServers.Add("Name servers não encontrados");
        }

        if (mustRefresh)
        {
            var dns = await _dns.ResolveAsync(domainName);
            var whoisDomain = await _whois.QueryAsync(domainName);
            var whoisIp = await _whois.QueryAsync(dns.Ip);

            if (domain == null)
            {
                domain = new Domain();
                _db.Domains.Add(domain);
            }

            domain.Name = domainName;
            domain.Ip = dns.Ip;
            domain.Ttl = dns.Ttl;
            domain.UpdatedAt = DateTime.UtcNow;
            domain.WhoIs = whoisDomain.Raw;
            domain.HostedAt = whoisIp.OrganizationName;

            await _db.SaveChangesAsync();
        }

        return new DomainResultDto
        {
            Domain = domain.Name,
            Ip = domain.Ip ?? "Ip não encontrado",
            HostedAt = domain.HostedAt ?? "Origem da hospeagem não encontrada",
            NameServers = nameServers
        };
    }
}

