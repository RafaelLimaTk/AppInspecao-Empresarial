using CommunityToolkit.Mvvm.Messaging.Messages;
using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.Libraries.Messages;

public class CompanySavedMessage : ValueChangedMessage<Company>
{
    public Company Company { get; private set; }

    public CompanySavedMessage(Company company) : base(company)
    {

    }
}
