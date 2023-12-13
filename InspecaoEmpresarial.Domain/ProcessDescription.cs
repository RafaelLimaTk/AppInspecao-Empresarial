namespace InspecaoEmpresarial.Domain;

public class ProcessDescription
{
    public int ProcessDescriptionId { get; set; }
    public string? Department { get; set; }
    public string? Activity { get; set; }

    public int CompanyId { get; set; }
    public Company? Company { get; set; }
}
