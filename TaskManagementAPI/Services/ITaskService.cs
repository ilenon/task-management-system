using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
  // Interface para o serviço de tarefas
  public interface ITaskService
  {
    Task<IEnumerable<TaskModel>> GetAllTasksAsync();
    Task<TaskModel?> GetTaskByIdAsync(int id);
    Task<TaskModel> AddTaskAsync(TaskModel task);
    Task<bool> UpdateTaskAsync(int id,TaskModel task);
    Task<bool> DeleteTaskAsync(int id);
  }
}