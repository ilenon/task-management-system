using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories.Interfaces;

/**
 * - Criamos a camada de serviço (TaskService).
 * - Agora a lógica de negócio fica na `TaskService`, e a `TaskController` apenas chama esse serviço.
 * **/

namespace TaskManagementAPI.Services
{
  // Camada de serviço para tarefas
  public class TaskService
  {
    private readonly ITaskRepository _taskRepository;
    
    // Construtor recebe o repositório injetado
    public TaskService(ITaskRepository taskRepository)
    {
      _taskRepository = taskRepository;
    }

    // Retorna todas as tarefas
    public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
    {
      return await _taskRepository.GetAllTasksAsync();
    }

    // Retorna uma tarefa especifica pelo ID
    public async Task<TaskModel?> GetTaskByIdAsync(int id)
    {
      return await _taskRepository.GetTaskByIdAsync(id);
    }

    // Adiciona uma nova tarefa
    public async Task<TaskModel> AddTaskAsync(TaskModel task)
    {
      return await _taskRepository.AddTaskAsync(task);
    }

    // Atualiza uma tarefa existente
    public async Task<TaskModel?> UpdateTaskAsync(int id, TaskModel task)
    {
      return await _taskRepository.UpdateTaskAsync(id, task);
    }

    // Exclui uma tarefa
    public async Task<bool> DeleteTaskAsync(int id)
    {
      return await _taskRepository.DeleteTaskAsync(id);
    }
  }
}