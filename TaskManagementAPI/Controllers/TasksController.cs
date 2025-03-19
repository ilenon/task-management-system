using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
  // Define que essa classe é um controllador de API
  [Route("api/[controller]")] // Define a rota base para o controlador. A URL será api/tasks.
  [ApiController] // Indica que essa classe é um controlador de API. Ele aplica validações automáticas nas entradas de dados (como tipos e requisitos de campos).
  public class TasksController : ControllerBase // `ControllerBase` - Base para controladores sem visualização (apenas para API).
  {
    private readonly AppDbContext _context;

    // Construtor para injeção de dependência do DbContext
    // O construtor recebe uma instância do AppDbContext e a armazena em um campo privado _context, permitindo acessar o banco de dados dentro dos métodos.
    public TasksController(AppDbContext context)
    {
      _context = context;
    }

    // 1. Endpoint para criar uma nova tarefa (POST)
    [HttpPost] // A rota POST permite criar uma nova tarefa. O método recebe um objeto TaskModel (a tarefa a ser criada).
    public async Task<ActionResult<TaskModel>> CreateTask(TaskModel task)
    {
      // Adiciona a tarefa ao contexto do banco
      _context.Tasks.Add(task);

      // Salva a nova tarefa no banco de dados
      await _context.SaveChangesAsync();

      // Retorna um status 201 (Created) junto com a URL do recurso recém-criado
      return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // 2. Endpoint para obter todas as tarefas (GET)
    [HttpGet] // Essa rota `GET` retorna todas as tarefas do banco de dados.
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
    {
      // Consulta todas as tarefas e as retorna como uma lista
      var tasks = await _context.Tasks.ToListAsync();

      // Retorna um status 200 (OK) com a lista de tarefas
      return Ok(tasks);
    }

    // 3. Endpoint para obter uma tarefa específica pelo ID (GET)
    [HttpGet("{id}")] // Essa rota permite obter uma tarefa específica pelo seu ID.
    public async Task<ActionResult<TaskModel>> GetTask(int id)
    {
      // Procura uma tarefa no banco de dados pelo ID
      var task = await _context.Tasks.FindAsync(id);

      // Verifica se a tarefa foi encontrada
      if (task == null)
      {
        return NotFound(); // Retorna 404 se a tarefa não for encontrada
      } 

      // Retorna a tarefa encontrada
      return Ok(task);
    }

    // 4. Endpoint para atualizar uma tarefa (PUT)
    [HttpPut("{id}")] // Essa rota permite atualizar uma tarefa existente.
    public async Task<IActionResult> UpdateTask(int id, TaskModel task)
    {
      // Verifica se o ID da tarefa corresponde ao ID da URL
      if (id != task.Id)
      {
        return BadRequest(); // Retorna 400 se os IDs não coincidirem
      }

      // Marca a tarefa como modificada para que o Entity Framework saiba que deve atualizar o banco de dados.
      _context.Entry(task).State = EntityState.Modified;

      try
      {
        // Salva as mudanças no banco de dados
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TaskExists(id))
        {
          return NotFound(); // Retorna 404 se a tarefa bão existir
        }
        else
        {
          throw; // Lança a exceção se ocorrer outro erro
        }
      }

      // Retorna 204 (No Content) quando a atualização for bem-sucedida
      return NoContent();
    }

    // 5. Endpoint para excluir uma tarefa (DELETE)
    [HttpDelete("{id}")] // Essa rota permite excluir uma tarefa com o ID especificado.
    public async Task<IActionResult> DeleteTask(int id)
    {
      // Procura a tarefa pelo ID
      var task = await _context.Tasks.FindAsync(id);

      // Verifica se a tarefa existe
      if (task == null)
      {
        return NotFound(); // Retorna 404 se não encontrar a tarefa
      }

      // Remove a tarefa do contexto do banco de daods
      _context.Tasks.Remove(task);

      // Salva as mudanças e exclui a tarefa no banco de dados
      await _context.SaveChangesAsync();

      // Retorna 204 (No Content) indicando que a exclusão foi bem-sucedida
      return NoContent();
    }

    // Método auxiliar para verificar se uma tarefa existe no banco
    // Verifica se uma tarefa existe no banco com o ID especificado, usado na atualização para garantir que a tarefa exista antes de tentar modificar.
    private bool TaskExists(int id)
    {
      return _context.Tasks.Any(e => e.Id == id);
    }
  }
}