using TaskManagementAPI.Models;

/**
 * Criamos uma interface chamada `ITaskRepository`, que define contratos para operações no banco de dados.
 * O `?` em `TaskModel?` indica que esse método pode retornar `null`.
 **/

namespace TaskManagementAPI.Repositories
{
  // Interface que define os métodos do repositório de tarefas
  public interface ITaskRepository
  {
    Task<IEnumerable<TaskModel>> GetAllTasksAsync(); // Retorna todas as tarefas
    Task<TaskModel?> GetTaskByIdAsync(int id); // Retorna uma tarefa pelo ID
    Task<TaskModel> AddTaskAsync(TaskModel task); // Adiciona uma nova tarefa
    Task<bool> UpdateTaskAsync(int id, TaskModel task); // Atualiza uma tarefa
    Task<bool> DeleteTaskAsync(int id); // Exclui uma tarefa pelo ID
  }
}