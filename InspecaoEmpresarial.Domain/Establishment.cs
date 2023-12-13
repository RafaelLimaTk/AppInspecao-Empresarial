namespace InspecaoEmpresarial.Domain;

public class Establishment
{
    public int EstablishmentId { get; set; }    
    public string? Location { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }

    public int CompanyId { get; set; }
    public Company? Company { get; set; }
}
