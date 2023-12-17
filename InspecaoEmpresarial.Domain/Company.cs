﻿using System.Net;

namespace InspecaoEmpresarial.Domain;

public class Company 
{
    public int Id { get; set; }
    public string? CorporateName { get; set; }
    public string? Address { get; set; }
    public string? CNPJ { get; set; }
    public string? CNAE { get; set; }
    public string? RiskGrade { get; set; }
    public string? Introduction { get; set; }
    public string? Objective { get; set; }

    public List<Establishment>? Establishments { get; set; } = new List<Establishment>();
    public List<Responsibility>? Responsibilities { get; set; } = new List<Responsibility>();
    public List<ProcessDescription>? ProcessDescriptions { get; set; } = new List<ProcessDescription>();

}
