using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Inspeção_Empresarial.Libraries.Messages;
using Inspeção_Empresarial.Repositories;
using InspecaoEmpresarial.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;

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
        Establishments = new ObservableCollection<Establishment>();

        Establishments.Clear();

        foreach (var item in CompanyDetails.Establishments)
        {
            Establishments.Add(new Establishment
            {
                Location = item.Location,
                Address = item.Address,
                Phone = item.Phone
            });
        }
    }

    //Melhorar esse metodo
    [RelayCommand]
    private async Task RemoveCompany()
    {
        _companyRepository.Delete(CompanyDetails);
        await Shell.Current.GoToAsync("//inspection");
        WeakReferenceMessenger.Default.Send(new CompanySavedMessage(CompanyDetails));
    }
}
