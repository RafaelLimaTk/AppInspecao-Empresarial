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
    public async Task GerarRelatorioPdf()
    {
        string nomeBase = $"Relatorio {companyDetails.CorporateName}";
        string extensao = ".docx";
        string caminhoBase = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        string caminhoRelatorio = GerarNovoNomeDeArquivo(caminhoBase, nomeBase, extensao);

        float cmToPoints = 28.3464567f;

        var currentPage = Shell.Current.CurrentPage;

        try
        {
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
                AdicionarTextoFormatadoABNT(document, companyDetails.Introduction);

                AdicionarTituloABNT(document, "3. OBJETIVO");
                AdicionarTextoFormatadoABNT(document, companyDetails.Objective);

                AdicionarTituloABNT(document, "4. RESPONSABILIDADES");
                AdicionarColecaoAoDocumento(document, Responsibilities, FormatarEAdicionarResponsibility);

                AdicionarTituloABNT(document, "5. IDENTIFICAÇÃO DO ESTABELECIMENTO");
                AdicionarColecaoAoDocumento(document, Establishments, FormatarEAdicionarEstablishment);

                document.Save();
            }
            await currentPage.DisplayAlert("Sucesso",
                $"Relatório {companyDetails.CorporateName} salvo com sucesso.", "Ok");
        }
        catch (Exception ex)
        {
            await currentPage.DisplayAlert("Erro",
                $"Não foi possível salvar o relatório.", "Ok");
        }
    }

    private string GerarNovoNomeDeArquivo(string caminhoBase, string nomeBase, string extensao)
    {
        int contador = 1;
        string novoNome = nomeBase;
        string caminhoCompleto = Path.Combine(caminhoBase, $"{novoNome}{extensao}");

        while (File.Exists(caminhoCompleto))
        {
            novoNome = $"{nomeBase} ({contador++})";
            caminhoCompleto = Path.Combine(caminhoBase, $"{novoNome}{extensao}");
        }

        return caminhoCompleto;
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

    private void AdicionarColecaoAoDocumento<T>(DocX document, IEnumerable<T> colecao, Action<DocX, T> formatarEAdicionarItem)
    {
        foreach (var item in colecao)
        {
            formatarEAdicionarItem(document, item);
        }
    }

    private void FormatarEAdicionarResponsibility(DocX document, Responsibility responsibility)
    {
        AdicionarSubtituloABNT(document, "SUPERINTENDÊNCIA E DIRETORIAS VINCULADAS À UNIDADE:");
        AdicionarTextoFormatadoABNT(document, responsibility.Superintendence);

        AdicionarSubtituloABNT(document, "GERENTES:");
        AdicionarTextoFormatadoABNT(document, responsibility.Management);

        AdicionarSubtituloABNT(document, "ENCARREGADOS E LÍDERES:");
        AdicionarTextoFormatadoABNT(document, responsibility.InCharge);

        AdicionarSubtituloABNT(document, "SESMT:");
        AdicionarTextoFormatadoABNT(document, responsibility.SMT);

        AdicionarSubtituloABNT(document, "CIPA E BRIGADA DE INCÊNDIO:");
        AdicionarTextoFormatadoABNT(document, responsibility.FireBrigade);

        AdicionarSubtituloABNT(document, "EMPREGADOS:");
        AdicionarTextoFormatadoABNT(document, responsibility.Employees);
    }

    private void FormatarEAdicionarEstablishment(DocX document, Establishment establishment)
    {
        var textoEstablishment = $"LOCAL: {establishment.Location}\n" +
                                 $"ENDEREÇO: {establishment.Address}\n" +
                                 $"TELEFONE: {establishment.Phone}";

        var formatting = new Formatting
        {
            FontFamily = new Xceed.Document.NET.Font("Arial"),
            Size = 12D
        };

        Paragraph paragraph = document.InsertParagraph(textoEstablishment, false, formatting);
        document.InsertParagraph("");
    }

    private void AdicionarTextoFormatadoABNT(DocX document, string texto)
    {
        var formatting = new Formatting
        {
            FontFamily = new Xceed.Document.NET.Font("Arial"),
            Size = 12D
        };

        Paragraph paragraph = document.InsertParagraph(texto, false, formatting);
        paragraph.Alignment = Alignment.both;
        paragraph.LineSpacing = 18.0f;
        paragraph.IndentationFirstLine = Convert.ToInt32(1.27 * 28.35);

        document.InsertParagraph("");
    }

    private void AdicionarSubtituloABNT(DocX document, string subtitulo)
    {
        var subtitleFormat = new Formatting();
        subtitleFormat.FontFamily = new Xceed.Document.NET.Font("Arial");
        subtitleFormat.Size = 12D;
        subtitleFormat.Bold = true;

        Paragraph subtitleParagraph = document.InsertParagraph(subtitulo, false, subtitleFormat);
        subtitleParagraph.Alignment = Alignment.left;

        document.InsertParagraph("");
    }
}
