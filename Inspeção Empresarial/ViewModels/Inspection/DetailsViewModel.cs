using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Inspeção_Empresarial.Libraries.Messages;
using Inspeção_Empresarial.Repositories;
using InspecaoEmpresarial.Domain;
using System.Collections.ObjectModel;
using System.Drawing;
using Xceed.Document.NET;
using Xceed.Words.NET;

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

    [RelayCommand]
    public void GerarRelatorioPdf()
    {
        string caminhoRelatorio = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", $"Relatorio {companyDetails.CorporateName}");

        float cmToPoints = 28.3464567f;

        using (var document = DocX.Create(caminhoRelatorio))
        {
            document.MarginTop = 3 * cmToPoints;
            document.MarginLeft = 3 * cmToPoints;
            document.MarginBottom = 2 * cmToPoints;
            document.MarginRight = 2 * cmToPoints;

            AdicionarTituloABNT(document, "1. IDENTIFICAÇÃO DA EMPRESA");

            Table table = document.AddTable(5, 1);
            table.Design = TableDesign.None;

            table.Rows[0].Cells[0].Paragraphs.First().Append($"NOME EMPRESARIAL: {companyDetails.CorporateName}").FontSize(12).Font("Arial");
            table.Rows[1].Cells[0].Paragraphs.First().Append($"ENDEREÇO: {companyDetails.Address}").FontSize(12).Font("Arial");
            table.Rows[2].Cells[0].Paragraphs.First().Append($"CNPJ: {companyDetails.CNPJ}").FontSize(12).Font("Arial");
            table.Rows[3].Cells[0].Paragraphs.First().Append($"C.N.A.E.: {companyDetails.CNAE}").FontSize(12).Font("Arial");
            table.Rows[4].Cells[0].Paragraphs.First().Append($"GRAU DE RISCO: {companyDetails.RiskGrade}").FontSize(12).Font("Arial");

            foreach (var row in table.Rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.Paragraphs.First().LineSpacing = 18.0f;
                }
            }

            document.InsertTable(table);
            document.InsertParagraph("");

            AdicionarTituloABNT(document, "2. INTRODUÇÃO");
            AdicionarIntroducaoEObjetivoABNT(document, companyDetails.Introduction);

            AdicionarTituloABNT(document, "3. OBJETIVO");
            AdicionarIntroducaoEObjetivoABNT(document, companyDetails.Objective);

            document.Save();
        }
    }

    private void AdicionarTituloABNT(DocX document, string titulo)
    {
        var titleFormat = new Formatting();
        titleFormat.FontFamily = new Xceed.Document.NET.Font("Arial");
        titleFormat.Size = 14D;
        titleFormat.Bold = true;

        Paragraph titleParagraph = document.InsertParagraph(titulo, false, titleFormat);
        titleParagraph.Highlight(Highlight.lightGray);

        document.InsertParagraph("");
    }

    private void AdicionarIntroducaoEObjetivoABNT(DocX document, string textoIntroducao)
    {
        var formatting = new Formatting();
        formatting.FontFamily = new Xceed.Document.NET.Font("Arial");
        formatting.Size = 12D;

        Paragraph paragraph = document.InsertParagraph(textoIntroducao, false, formatting);
        paragraph.Alignment = Alignment.both;
        paragraph.LineSpacing = 18.0f;
        paragraph.IndentationFirstLine = Convert.ToInt32(1.27 * 28.35);

        document.InsertParagraph("");
    }
}
