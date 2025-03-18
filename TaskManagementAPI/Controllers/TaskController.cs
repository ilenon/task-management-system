using Microsoft.AspNetCore.Mvc;

namespace TaskManagementAPI.Controllers
{
  // Defina este controllador como um controllador de API
  [ApiController]
  // Defina a rota base da API como "api.tasks"
  [Route("api/[controller]")]
  public class TaskController : ControllerBase
  {
    // Método para obter todas as tarefas
    [HttpGet]
    public IActionResult GetAllTasks()
    {
      // Retorna uma lista fictícia de tarefas
      var tasks = new List<object>
      {
        new { Id = 1, Title = "Learn ASP.NET", Completed = false },
        new { Id = 2, Title = "Build API", Completed = true }
      };

      return Ok(tasks); // Retorna um código HTTP 200 (OK) com dados
    }
  }
}