using System.Net;

namespace InspecaoEmpresarial.Domain;

public class Company
{
    public int Id { get; set; }
    public string? CorporateName { get; set; }
    public string? Address { get; set; }
    public string? CNPJ { get; set; }
    public string? CNAE { get; set; }
    public string? RiskGrade { get; set; }


}
