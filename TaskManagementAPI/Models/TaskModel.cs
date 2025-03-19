using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models
{
  // Representa a tabela "Tasks" no banco de dados
  [Table("Tasks")] // Define que essa classe será mapeada para a tabela Tasks no banco.
  public class TaskModel
  {
    // Identificador único da tarefa (Primary Key)
    [Key] // Indica que a propriedade Id será a chave primária.
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Faz com que o banco gere automaticamente os IDs.
    public int Id { get; set; }

    // Nome da tarefa (Campo obrigatório)
    [Required] // Garante que `Name` seja obrigatório.
    [MaxLength(100)] // Define que o Name terá no máximo 100 caracteres.
    public string Name { get; set; } = string.Empty;

    // Descrição da tarefa
    [MaxLength(500)]
    public string? Description { get; set; }

    // Indica se a tarefa foi concluída ou não
    public bool IsCompleted { get; set; } = false; // Campo para indicar se a tarefa foi concluída ou não, começando sempre como false.
  }
}