using InspecaoEmpresarial.Domain;
using InspecaoEmpresarial.Infra.Data;

namespace Inspeção_Empresarial.Repositories;

public interface IProcessDescriptionRepository
{
    IList<ProcessDescription> GetAll();
    ProcessDescription GetById(int id);
    void Add(ProcessDescription processDescription);
    void Update(ProcessDescription processDescription);
    void Delete(ProcessDescription processDescription);
}
