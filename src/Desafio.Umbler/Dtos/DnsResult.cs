using System.Collections.Generic;

namespace Desafio.Umbler.Dtos;

public class DnsResult
{
    public string Ip { get; set; } = string.Empty;
    public int Ttl { get; set; }
    public List<string> NameServers { get; set; } = new();
}