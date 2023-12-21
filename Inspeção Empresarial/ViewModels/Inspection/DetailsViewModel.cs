using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Inspeção_Empresarial.Libraries.Messages;
using Inspeção_Empresarial.Repositories;
using InspecaoEmpresarial.Domain;
using System.Collections.ObjectModel;

namespace Inspeção_Empresarial.ViewModels.Inspection;

[QueryProperty(nameof(CompanyId), "companyId")]
public partial class DetailsViewModel : ObservableObject
{
    [ObservableProperty]
    string companyId;

    [ObservableProperty]
    private Company companyDetails;

    [ObservableProperty]
    private ObservableCollection<Establishment> establishments;

    [ObservableProperty]
    private ObservableCollection<Responsibility> responsibilities;

    private readonly ICompanyRepository _companyRepository;
    public DetailsViewModel()
    {
        _companyRepository = App.Current.Handler.MauiContext.Services.GetService<ICompanyRepository>();
    }

    partial void OnCompanyIdChanged(string value)
    {
        LoadCompanyDetails(value);
    }

    private void LoadCompanyDetails(string id)
    {
        CompanyDetails = _companyRepository.GetById(int.Parse(id));

        Establishments = new ObservableCollection<Establishment>(
        CompanyDetails.Establishments.Select(item => new Establishment
        {
            Location = item.Location,
            Address = item.Address,
            Phone = item.Phone
        }));

        Responsibilities = new ObservableCollection<Responsibility>(
        CompanyDetails.Responsibilities.Select(item => new Responsibility
        {
            Superintendence = item.Superintendence,
            Management = item.Management,
            InCharge = item.InCharge,
            SMT = item.SMT,
            FireBrigade = item.FireBrigade,
            Employees = item.Employees
        }));
    }

    [RelayCommand]
    private async Task RemoveCompany()
    {
        var currentPage = Shell.Current.CurrentPage;
        bool isConfirmed = await currentPage.DisplayAlert("Confirmação de exclusão",
            $"Tem certeza que deseja excluir permanentemente o relatório da empresa {CompanyDetails.CorporateName}? Esta ação não pode ser desfeita.", "Excluir", "Cancelar");
        if (isConfirmed)
        {
            _companyRepository.Delete(CompanyDetails);
            await Shell.Current.GoToAsync("//inspection");
            WeakReferenceMessenger.Default.Send(new CompanySavedMessage(CompanyDetails));
        }
    }

    [RelayCommand]
    private async Task UpdateCompany()
    {
        await Shell.Current.GoToAsync($"create?companyId={CompanyId}");
    }
}
