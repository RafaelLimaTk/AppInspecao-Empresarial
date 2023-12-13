using InspecaoEmpresarial.Domain;
using Microsoft.EntityFrameworkCore;

namespace InspecaoEmpresarial.Infra.Data;

public class InspecaoEmpresarialContext : DbContext
{
    public DbSet<Company> Company { get; set; }
    public DbSet<Establishment> Establishment { get; set; }
    public DbSet<ProcessDescription> ProcessDescription { get; set; }
    public DbSet<Responsibility> Responsibility { get; set; }

    public InspecaoEmpresarialContext()
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var data = "Inspecao_Empresarial.db";
        var databasePath = Path.Combine(folderPath, data);

        optionsBuilder.UseSqlite($"Filename={databasePath}");
        Console.WriteLine(databasePath);

    }
}
