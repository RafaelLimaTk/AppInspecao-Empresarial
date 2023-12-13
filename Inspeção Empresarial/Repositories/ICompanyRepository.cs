using InspecaoEmpresarial.Domain;

namespace Inspeção_Empresarial.Repositories;

public interface ICompanyRepository
{
    IList<Company> GetAll();
    Company GetById(int id);
    void Add(Company company);
    void Update(Company company);
    void Delete(Company company);
}
