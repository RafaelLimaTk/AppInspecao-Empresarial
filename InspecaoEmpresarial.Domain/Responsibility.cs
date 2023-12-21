namespace InspecaoEmpresarial.Domain;

public class Responsibility
{
    public int ResponsibilityId { get; set; }
    public string? Superintendence { get; set; } 
    public string? Management { get; set; }
    public string? InCharge { get; set; }
    public string? SE { get; set; } 
    public string? SMT { get; set; }
    public string? CIPA { get; set; } 
    public string? FireBrigade { get; set; } 
    public string? Employees { get; set; } 

    public int CompanyId { get; set; }
    public Company? Company { get; set; }

    public Responsibility() { }

    public Responsibility(string? superintendence, string? management, 
                        string? inCharge, string? smt, string? fireBrigade, 
                        string? employees, int companyId, Company company)
    {
        Superintendence = superintendence;
        Management = management;
        InCharge = inCharge;
        SMT = smt;
        FireBrigade = fireBrigade;
        Employees = employees;
        CompanyId = companyId;
        Company = company;
    }

    public void UpdateDetailsResponsability(string superintendencyDiretorias, string manager, string supervisors, string sesmt, string brigade, string employees)
    {
        Superintendence = superintendencyDiretorias;
        Management = manager;
        InCharge = supervisors;
        SMT = sesmt;
        Employees = employees;
        FireBrigade = brigade;
    }
}
