using InspecaoEmpresarial.Domain;
using InspecaoEmpresarial.Infra.Data;

namespace Inspeção_Empresarial.Repositories;

public class ResponsibilityRepository : IResponsibilityRepository
{
    private InspecaoEmpresarialContext _context;

    public ResponsibilityRepository()
    {
        _context = new InspecaoEmpresarialContext();
    }

    public IList<Responsibility> GetAll()
    {
        return _context.Responsibility.ToList();
    }

    public Responsibility GetById(int id)
    {
        return _context.Responsibility.Find(id);
    }

    public void Add(Responsibility responsibility)
    {
        _context.Responsibility.Add(responsibility);
        _context.SaveChangesAsync();
    }

    public void Update(Responsibility responsibility)
    {
        _context.Responsibility.Update(responsibility);
        _context.SaveChangesAsync();
    }

    public void Delete(Responsibility responsibility)
    {
        _context.Responsibility.Remove(responsibility);
        _context.SaveChangesAsync();
    }
}
