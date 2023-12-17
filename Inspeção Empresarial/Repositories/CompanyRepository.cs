using InspecaoEmpresarial.Domain;
using InspecaoEmpresarial.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Inspeção_Empresarial.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private InspecaoEmpresarialContext _context;
    public CompanyRepository()
    {
        _context = new InspecaoEmpresarialContext();
    }

    public IList<Company> GetAll()
    {
        return _context.Company.ToList();
    }

    public Company GetById(int id)
    {
        return _context.Company
            .Include(c => c.Establishments)
            .Include(c => c.ProcessDescriptions)
            .Include(c => c.Responsibilities)
            .FirstOrDefault(c => c.Id == id);
    }

    public void Add(Company company)
    {
        _context.Company.Add(company);
        _context.SaveChangesAsync();
    }

    public void Update(Company company)
    {
        _context.Company.Update(company);
        _context.SaveChangesAsync();
    }

    public void Delete(Company company)
    {
        _context.Company.Remove(company);
        _context.SaveChangesAsync();
    }
}
