using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    // Define que essa controller irá lidar com requisições para a rota "api/task"
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <- Protege todos os endpoints da controller
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // Construtor recebe a camada de serviço injetada
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/tasks - Endpoint para retornar todas as tarefas
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        // GET: api/tasks/5 - Endpoint para retornar uma tarefa específica pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        // POST: api/tasks - Endpoint para criar uma nova tarefa
        [HttpPost]
        public async Task<IActionResult> PostTask(TaskModel task)
        {
            var createdTask = await _taskService.AddTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }

        // PUT: api/tasks/5 - Endpoint para atualizar uma tarefa existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskModel task)
        {
            if (id != task.Id) return BadRequest();

            var updatedTask = await _taskService.UpdateTaskAsync(id, task);
            return updatedTask != false ? NoContent() : NotFound();
        }

        // DELETE: api/tasks/5 - Endpoint para excluir uma tarefa pelo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            return await _taskService.DeleteTaskAsync(id) ? NoContent() : NotFound();
        }
    }
}