using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Inspeção_Empresarial.Libraries.Messages;
using Inspeção_Empresarial.Repositories;
using Inspeção_Empresarial.Validation;
using InspecaoEmpresarial.Domain;
using System.Collections.ObjectModel;

namespace Inspeção_Empresarial.ViewModels.Inspection;

public partial class CreateAndEditPageViewModel : ObservableObject
{
    #region Company
    [ObservableProperty]
    private Company company;

    [ObservableProperty]
    private string corporateName;

    [ObservableProperty]
    private string addressCompany;

    [ObservableProperty]
    private string cnpj;

    [ObservableProperty]
    private string cnae;

    [ObservableProperty]
    private string riskgrade;

    [ObservableProperty]
    private string introduction;

    [ObservableProperty]
    private string objective;
    #endregion

    #region Establishment
    [ObservableProperty]
    private EstablishmentViewModel establishmentViewModel;

    [ObservableProperty]
    private string location;

    [ObservableProperty]
    private string addressEstablishment;

    [ObservableProperty]
    private string phone;
    #endregion

    #region Descrição do processo
    [ObservableProperty]
    private string department;

    [ObservableProperty]
    private string activity;
    #endregion

    #region Responsabilidades
    [ObservableProperty]
    private string superintendencyDiretorias;

    [ObservableProperty]
    private string manager;

    [ObservableProperty]
    private string supervisors;

    [ObservableProperty]
    private string sesmt;

    [ObservableProperty]
    private string employees;

    [ObservableProperty]
    private string brigade;
    #endregion

    #region Propriedades para mensagens de erro
    [ObservableProperty]
    private string corporateNameError;

    [ObservableProperty]
    private string cnaeError;

    [ObservableProperty]
    private string cnpjError;
    #endregion

    #region Propriedades de animação
    [ObservableProperty]
    private double arrowRotationFirst = 180;

    [ObservableProperty]
    private double arrowRotationSecond;

    [ObservableProperty]
    private double arrowRotationThird;

    [ObservableProperty]
    private double arrowRotationFourth;

    [ObservableProperty]
    private bool isFirstExpanderOpen = true;

    [ObservableProperty]
    private bool isSecondExpanderOpen;

    [ObservableProperty]
    private bool isThirdExpanderOpen;

    [ObservableProperty]
    private bool isFourthExpanderOpen;
    #endregion

    private readonly CompanyValidator _validator = new CompanyValidator();
    public ObservableCollection<EstablishmentViewModel> Establishments { get; set; }

    private readonly ICompanyRepository _companyRepository;
    public CreateAndEditPageViewModel()
    {
        _companyRepository = App.Current.Handler.MauiContext.Services.GetService<ICompanyRepository>();

        Company = new Company();
        EstablishmentViewModel = new EstablishmentViewModel();
        Establishments = new ObservableCollection<EstablishmentViewModel>();
    }

    [RelayCommand]
    public void SaveEstablishment()
    {
        var newEstablishment = new Establishment
        {
            Location = EstablishmentViewModel.Location,
            Address = EstablishmentViewModel.Address,
            Phone = EstablishmentViewModel.Phone,
            CompanyId = Company.Id,
            Company = Company
        };

        Establishments.Add(EstablishmentViewModel);

        Company.Establishments.Add(newEstablishment);

        EstablishmentViewModel = new EstablishmentViewModel();

        Location = string.Empty;
        AddressEstablishment = string.Empty;
        Phone = string.Empty;
    }

    // Melhorar esse metodo
    [RelayCommand]
    public async Task SaveCompany()
    {
        var newResponsibility = new Responsibility
        {
            Superintendence = SuperintendencyDiretorias,
            Management = Manager,
            InCharge = Supervisors,
            SMT = Sesmt,
            Employees = Employees,
            FireBrigade = Brigade,
            CompanyId = Company.Id,
            Company = Company
        };

        var newProcessDescription = new ProcessDescription
        {
            Department = Department,
            Activity = Activity,
            CompanyId = Company.Id,
            Company = Company
        };

        UpdateCompanyEntity();

        Company.ProcessDescriptions.Add(newProcessDescription);
        Company.Responsibilities.Add(newResponsibility);

        var validationResult = _validator.Validate(Company);
        if (validationResult.IsValid)
        {
            SaveCompanyToDatabase(Company);
            WeakReferenceMessenger.Default.Send(new CompanySavedMessage(Company));
            await Shell.Current.GoToAsync("//inspection");
        }
        else
        {
            CorporateNameError = string.Empty;
            CnaeError = string.Empty;
            CnpjError = string.Empty;

            foreach (var failure in validationResult.Errors)
            {
                switch (failure.PropertyName)
                {
                    case nameof(Company.CorporateName):
                        CorporateNameError = failure.ErrorMessage;
                        break;
                    case nameof(Company.CNAE):
                        CnaeError = failure.ErrorMessage;
                        break;
                    case nameof(Company.CNPJ):
                        CnpjError = failure.ErrorMessage;
                        break;
                }
            }
        }
    }

    private void UpdateCompanyEntity()
    {
        Company.CorporateName = CorporateName;
        Company.Address = AddressCompany;
        Company.CNPJ = Cnpj;
        Company.CNAE = Cnae;
        Company.RiskGrade = Riskgrade;
        Company.Introduction = Introduction;
        Company.Objective = Objective;
    }


    public void SaveCompanyToDatabase(Company company)
    {
        _companyRepository.Add(company);
    }

    [RelayCommand]
    public void DeleteEstablishment(EstablishmentViewModel establishmentViewModel)
    {
        var establishmentToDelete = Company.Establishments
       .FirstOrDefault(e => e.EstablishmentId == establishmentViewModel.EstablishmentId);

        if (establishmentToDelete != null)
        {
            Company.Establishments.Remove(establishmentToDelete);
            Establishments.Remove(establishmentViewModel);
        }
    }
}
