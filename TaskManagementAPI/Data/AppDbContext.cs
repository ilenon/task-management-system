using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
  // Representa a conexão com o banco de dados
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as opções do contexto
        // `base(options)` → Passa as opções de conexão para a classe base.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Representa a tabela "Tasks" no banco de dados
        public DbSet<TaskModel> Tasks { get; set; }
    }
}