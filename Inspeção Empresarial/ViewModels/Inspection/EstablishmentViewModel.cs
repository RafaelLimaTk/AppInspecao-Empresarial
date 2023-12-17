using CommunityToolkit.Mvvm.ComponentModel;
using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.ViewModels.Inspection;

public partial class EstablishmentViewModel : ObservableObject
{
    public int EstablishmentId { get; set; }

    [ObservableProperty]
    private string? location;

    [ObservableProperty]
    private string? address;

    [ObservableProperty]
    private string? phone;
    public int CompanyId { get; set; }
    public Company? Company { get; set; }
}
