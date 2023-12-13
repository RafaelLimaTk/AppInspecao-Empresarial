using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inspeção_Empresarial.Repositories;
using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.ViewModels.Inspection;

public partial class ListPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string textSearch = "";

    private List<Company> companyListFull;

    [ObservableProperty]
    private List<Company> companyListFilter;

    private readonly ICompanyRepository _companyRepository;
    public ListPageViewModel()
    {
        _companyRepository = App.Current.Handler.MauiContext.Services.GetService<ICompanyRepository>();

        companyListFull = _companyRepository.GetAll().ToList();
        companyListFilter = companyListFull.ToList();
    }

    [RelayCommand]
    private void SearchInspections()
    {
        CompanyListFilter = companyListFull.Where(c => c.CorporateName.ToLower().Contains(textSearch.ToLower())).ToList();
    }

    [RelayCommand]
    private async void AddCompany()
    {
        await Shell.Current.GoToAsync("create");
    }
}
