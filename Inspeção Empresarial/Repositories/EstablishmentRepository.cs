using InspecaoEmpresarial.Domain;
using InspecaoEmpresarial.Infra.Data;

namespace Inspeção_Empresarial.Repositories;

public class EstablishmentRepository : IEstablishmentRepository
{
    private InspecaoEmpresarialContext _context;

    public EstablishmentRepository()
    {
        _context = new InspecaoEmpresarialContext();
    }

    public IList<Establishment> GetAll()
    {
        return _context.Establishment.ToList();
    }

    public Establishment GetById(int id)
    {
        return _context.Establishment.Find(id);
    }

    public void Add(Establishment establishment)
    {
        _context.Establishment.Add(establishment);
        _context.SaveChangesAsync();
    }

    public void Update(Establishment establishment)
    {
        _context.Establishment.Update(establishment);
        _context.SaveChangesAsync();
    }

    public void Delete(Establishment establishment)
    {
        _context.Establishment.Remove(establishment);
        _context.SaveChangesAsync();
    }
}

