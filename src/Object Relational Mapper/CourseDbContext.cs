using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Object_Relational_Mapper;

public class CourseDbContext : DbContext
{
    private readonly string _connectionString;
    private readonly string _assemblyName;

    public CourseDbContext()
    {
        _connectionString = "Server=.\\SQLEXPRESS; Database=mini-orm; " +
                                  "User Id=;Password=; Encrypt=True; " +
                                  "TrustServerCertificate=True;";
        _assemblyName = Assembly.GetExecutingAssembly().FullName;
    }

    public CourseDbContext(string connectionString, string assemblyName)
    {
        _connectionString = connectionString;
        _assemblyName = assemblyName;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        if (!dbContextOptionsBuilder.IsConfigured)
        {
            dbContextOptionsBuilder.UseSqlServer(_connectionString, m => m.MigrationsAssembly(_assemblyName));
        }

        base.OnConfiguring(dbContextOptionsBuilder);
    }

    public DbSet<Course> Course { get; set; }
}