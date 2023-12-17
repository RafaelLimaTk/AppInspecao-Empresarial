using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Inspeção_Empresarial.Libraries.Messages;
using Inspeção_Empresarial.Repositories;
using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.ViewModels.Inspection;

public partial class ListPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string textSearch = "";

    [ObservableProperty]
    private bool invisibleLabel = true;

    private List<Company> companyListFull;

    [ObservableProperty]
    private List<Company> companyListFilter;

    private readonly ICompanyRepository _companyRepository;
    public ListPageViewModel()
    {
        _companyRepository = App.Current.Handler.MauiContext.Services.GetService<ICompanyRepository>();

        companyListFull = _companyRepository.GetAll().ToList();
        companyListFilter = companyListFull.ToList();

        InvisibleLabel = companyListFilter.Count == 0;

        WeakReferenceMessenger.Default.Register<CompanySavedMessage>(this, (r, m) => {
            LoadCompanies();
        });
    }

    private void LoadCompanies()
    {
        companyListFull = _companyRepository.GetAll().ToList();
        SearchInspections();
    }

    [RelayCommand]
    private void SearchInspections()
    {
        CompanyListFilter = companyListFull.Where(c => c.CorporateName.ToLower().Contains(textSearch.ToLower())).ToList();

        InvisibleLabel = CompanyListFilter.Count == 0;
    }

    [RelayCommand]
    private async void AddCompany()
    {
        await Shell.Current.GoToAsync("create");
    }

    [RelayCommand]
    private async Task DatailsCompany(Company company)
    {
        var companyId = company.Id;
        await Shell.Current.GoToAsync($"detail?companyId={companyId}");
    }
}
