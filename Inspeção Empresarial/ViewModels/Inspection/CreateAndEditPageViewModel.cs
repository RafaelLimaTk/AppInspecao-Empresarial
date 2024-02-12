using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FluentValidation.Results;
using Inspeção_Empresarial.Libraries.Messages;
using Inspeção_Empresarial.Repositories;
using Inspeção_Empresarial.Validation;
using InspecaoEmpresarial.Domain;
using System.Collections.ObjectModel;

namespace Inspeção_Empresarial.ViewModels.Inspection;

[QueryProperty(nameof(CompanyId), "companyId")]
public partial class CreateAndEditPageViewModel : ObservableObject
{
    #region Company
    [ObservableProperty]
    private Company company;

    [ObservableProperty]
    string companyId;

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
    private int processDescriptionId;

    [ObservableProperty]
    private string department;

    [ObservableProperty]
    private string activity;
    #endregion

    #region Responsabilidades
    [ObservableProperty]
    private int responsabilityId;

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
    private bool IsEditMode => !string.IsNullOrWhiteSpace(CompanyId);

    private readonly ICompanyRepository _companyRepository;
    private readonly IEstablishmentRepository _establishmentRepository;
    private readonly IResponsibilityRepository _responsibilityRepository;
    private readonly IProcessDescriptionRepository _processDescriptionRepository;
    public CreateAndEditPageViewModel()
    {
        _companyRepository = App.Current.Handler.MauiContext.Services.GetService<ICompanyRepository>();
        _establishmentRepository = App.Current.Handler.MauiContext.Services.GetService<IEstablishmentRepository>();
        _responsibilityRepository = App.Current.Handler.MauiContext.Services.GetService<IResponsibilityRepository>();
        _processDescriptionRepository = App.Current.Handler.MauiContext.Services.GetService<IProcessDescriptionRepository>();

        Company = new Company();
        EstablishmentViewModel = new EstablishmentViewModel();
        Establishments = new ObservableCollection<EstablishmentViewModel>();
    }

    partial void OnCompanyIdChanged(string value)
    {
        if (IsEditMode)
        {
            LoadCompanyEdit(CompanyId);
        }
    }

    private void LoadCompanyEdit(string companyId)
    {
        var companyid = int.Parse(companyId);
        Company = _companyRepository.GetById(companyid);

        if (Company != null)
        {
            PopulateCompanyData(Company);
            PopulateEstablishments(Company.Establishments);

            var firstResponsibility = Company.Responsibilities.FirstOrDefault();
            if (firstResponsibility != null)
            {
                PopulateResponsibilities(firstResponsibility);
            }

            var firstProcessDescription = Company.ProcessDescriptions.FirstOrDefault();
            if (firstProcessDescription != null)
            {
                PopulateProcessDescriptions(firstProcessDescription);
            }
        }
    }

    #region Metodos do LoadCompanyEdit 
    private void PopulateCompanyData(Company companyData)
    {
        CorporateName = companyData.CorporateName;
        AddressCompany = companyData.Address;
        Cnpj = companyData.CNPJ;
        Cnae = companyData.CNAE;
        Riskgrade = companyData.RiskGrade;
        Introduction = companyData.Introduction;
        Objective = companyData.Objective;
    }

    private void PopulateEstablishments(IEnumerable<Establishment> establishments)
    {
        foreach (var establishment in establishments)
        {
            var estViewModel = new EstablishmentViewModel
            {
                EstablishmentId = establishment.EstablishmentId,
                Location = establishment.Location,
                Address = establishment.Address,
                Phone = establishment.Phone
            };
            Establishments.Add(estViewModel);
        }
    }

    private void PopulateResponsibilities(Responsibility responsability)
    {
        ResponsabilityId = responsability.ResponsibilityId;
        SuperintendencyDiretorias = responsability.Superintendence;
        Manager = responsability.Management;
        Supervisors = responsability.InCharge;
        Sesmt = responsability.SMT;
        Employees = responsability.Employees;
        Brigade = responsability.FireBrigade;
    }

    private void PopulateProcessDescriptions(ProcessDescription processDescription)
    {
        ProcessDescriptionId = processDescription.ProcessDescriptionId;
        Department = processDescription.Department;
        Activity = processDescription.Activity;
    }
    #endregion

    [RelayCommand]
    public void SaveEstablishment()
    {
        var newEstablishment = new Establishment(EstablishmentViewModel.Location, EstablishmentViewModel.Address, EstablishmentViewModel.Phone, Company.Id, Company);

        Establishments.Add(EstablishmentViewModel);
        Company.Establishments.Add(newEstablishment);

        EstablishmentViewModel = new EstablishmentViewModel();

        Location = string.Empty;
        AddressEstablishment = string.Empty;
        Phone = string.Empty;
    }

    [RelayCommand]
    public async Task SaveCompany()
    {
        Company.CreateCompany(CorporateName, AddressCompany, Cnpj, Cnae, Riskgrade, Introduction, Objective);

        ProcessResponsibility();
        ProcessProcessDescription();

        var validationResult = _validator.Validate(Company);
        if (!validationResult.IsValid)
        {
            UpdateValidationErrors(validationResult);
            return;
        }

        SaveCompanyToDatabase(Company);
        WeakReferenceMessenger.Default.Send(new CompanySavedMessage(Company));
        await Shell.Current.GoToAsync("//inspection");
    }

    private void UpdateValidationErrors(ValidationResult validationResult)
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

    public void SaveCompanyToDatabase(Company company)
    {
        if (company.Id == 0)
            _companyRepository.Add(company);
        else
            _companyRepository.Update(company);

    }

    [RelayCommand]
    public void DeleteEstablishment(EstablishmentViewModel establishmentViewModel)
    {
        var establishmentToDelete = Company.Establishments
       .FirstOrDefault(e => e.EstablishmentId == establishmentViewModel.EstablishmentId);

        if (establishmentToDelete != null)
        {
            if (IsEditMode)
            {
                RemoveEstablishmentFromDatabase(establishmentToDelete);
            }

            Company.Establishments.Remove(establishmentToDelete);
            Establishments.Remove(establishmentViewModel);
        }
    }

    private void RemoveEstablishmentFromDatabase(Establishment establishment)
    {
        _establishmentRepository.Delete(establishment);
    }

    #region Methods ProcessResponsibility & ProcessProcessDescription
    private void ProcessResponsibility()
    {
        var responsibility = ResponsabilityId != 0
            ? UpdateExistingResponsibility()
            : CreateNewResponsibility();

        if (responsibility != null)
            _responsibilityRepository.Update(responsibility);
        else
            Company.Responsibilities.Add(responsibility);
    }

    private Responsibility UpdateExistingResponsibility()
    {
        var existingResponsibility = _responsibilityRepository.GetById(ResponsabilityId);
        if (existingResponsibility == null) return null;

        existingResponsibility.UpdateDetailsResponsability(SuperintendencyDiretorias, Manager, Supervisors, Sesmt, Brigade, Employees);
        return existingResponsibility;
    }

    private Responsibility CreateNewResponsibility()
    {
        return new Responsibility(SuperintendencyDiretorias, Manager, Supervisors, Sesmt, Brigade, Employees, Company.Id, Company);
    }

    private void ProcessProcessDescription()
    {
        var processDescription = ProcessDescriptionId != 0
            ? UpdateExistingProcessDescription()
            : CreateNewProcessDescription();

        if (processDescription != null)
            _processDescriptionRepository.Update(processDescription);
        else
            Company.ProcessDescriptions.Add(processDescription);
    }

    private ProcessDescription UpdateExistingProcessDescription()
    {
        var existingProcessDescription = _processDescriptionRepository.GetById(ProcessDescriptionId);
        if (existingProcessDescription == null) return null;

        existingProcessDescription.UpdateDetailsProcessDescription(Activity, Department);
        return existingProcessDescription;
    }

    private ProcessDescription CreateNewProcessDescription()
    {
        return new ProcessDescription(Activity, Department, Company.Id, Company);
    }
    #endregion
}
