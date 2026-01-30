using System.Collections.Generic;

namespace Desafio.Umbler.Dtos;

public class DomainResultDto
{
    public string Domain { get; set; }
    public string Ip { get; set; }
    public string HostedAt { get; set; }
    public List<string> NameServers { get; set; }
}
