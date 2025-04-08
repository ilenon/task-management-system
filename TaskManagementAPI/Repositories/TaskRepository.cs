using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

/**
 * - `TaskRepository` implementa `ITaskRepository`.
 * - Utiliza injeção de dependência (`AppDbContext`) para acessar o banco.
 * - Métodos assíncronos para melhor desempenho.
 * **/

namespace TaskManagementAPI.Repositories
{
  // Implementação do repositório de tarefas
  public class TaskRepository : ITaskRepository
  {
    private readonly AppDbContext _context;

    // construtor que recebe o contexto do banco de dados
    public TaskRepository(AppDbContext context)
    {
      _context = context;
    }

    // `GetAllTaskAsync()` retorna todas as tarefas do banco de dados
    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
    {
      return await _context.Tasks.ToListAsync();
    }

    // `GetTaskByIdAsync()` retorna uma única tarefa pelo ID 
    public async Task<TaskModel?> GetTaskByIdAsync(int id)
    {
      return await _context.Tasks.FindAsync(id);
    }

    // `CreateTaskAsync()` adiciona uma nova tarefa ao banco
    public async Task<TaskModel> AddTaskAsync(TaskModel task)
    {
      _context.Tasks.Add(task);
      await _context.SaveChangesAsync();
      return task;
    }

    // `UpdateTaskAsync()` Atualiza uma tarefa existente
    public async Task<bool> UpdateTaskAsync(int id, TaskModel task)
    {
      var existingTask = await _context.Tasks.FindAsync(id);
      if (existingTask == null)
        return false;

      existingTask.Name = task.Name;
      existingTask.Description = task.Description;
      existingTask.IsCompleted = task.IsCompleted;

      await _context.SaveChangesAsync();
      return true;
    }

    // `DeleteTaskAsync()` Exclui uma tarefa pelo ID
    public async Task<bool> DeleteTaskAsync(int id)
    {
      var task = await _context.Tasks.FindAsync();
      if (task == null)
        return false;

      _context.Tasks.Remove(task);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}