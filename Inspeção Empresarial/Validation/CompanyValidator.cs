using FluentValidation;
using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.Validation;

class CompanyValidator : AbstractValidator<Company>
{
    public CompanyValidator()
    {
        RuleFor(company => company.CorporateName).NotEmpty().WithMessage("O nome empresarial é obrigatório.");
        RuleFor(company => company.CNPJ).NotEmpty().WithMessage("O CNPJ é obrigatório.");
        RuleFor(company => company.CNAE).NotEmpty().WithMessage("O CNAE é obrigatório.");
    }
}
