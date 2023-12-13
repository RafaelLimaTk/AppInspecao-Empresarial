using InspecaoEmpresarial.Domain;
using InspecaoEmpresarial.Infra.Data;

namespace Inspeção_Empresarial.Repositories;

public class ProcessDescriptionRepository : IProcessDescriptionRepository
{
    private InspecaoEmpresarialContext _context;

    public ProcessDescriptionRepository()
    {
        _context = new InspecaoEmpresarialContext();
    }

    public IList<ProcessDescription> GetAll()
    {
        return _context.ProcessDescription.ToList();
    }

    public ProcessDescription GetById(int id)
    {
        return _context.ProcessDescription.Find(id);
    }

    public void Add(ProcessDescription processDescription)
    {
        _context.ProcessDescription.Add(processDescription);
        _context.SaveChangesAsync();
    }

    public void Update(ProcessDescription processDescription)
    {
        _context.ProcessDescription.Update(processDescription);
        _context.SaveChangesAsync();
    }

    public void Delete(ProcessDescription processDescription)
    {
        _context.ProcessDescription.Remove(processDescription);
        _context.SaveChangesAsync();
    }
}
