using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using Xunit;

namespace TaskManegementAPI.Tests.Controllers
{
  /// <summary>
  /// Classe de testes para TaskController.
  /// </summary>
  public class TaskControllerTests
  {
    private readonly Mock<ITaskService> _taskServiceMock;
    private readonly TaskController _taskController;

    /// <summary>
    /// Construtor da classe de testes, inicializa os mocks e a controller.
    /// </summary>
    public TaskControllerTests()
    {
      _taskServiceMock = new Mock<ITaskService>();
      _taskController = new TaskController(_taskServiceMock.Object);
    }

    /// <summary>
    /// Testa se GetAllTasks retorna uma lista de tarefas corretamente.
    /// </summary>
    [Fact]
    public async Task GetAllTasks_ShouldReturnOkWithList()
    {
      // Given
      // Arrange: criamos uma lista simulada de tarefas
      var tasks = new List<TaskModel>
      {
        new TaskModel { Id = 1, Name = "Task 1", Description = "Description 1" },
        new TaskModel { Id = 2, Name = "Task 2", Description = "Description 2" }
      };

      // Configuramos o mock para retornar a lista simulada
      _taskServiceMock.Setup(service => service.GetAllTasksAsync()).ReturnsAsync(tasks);

      // When
      // Act: chamamos o método da controller
      var result = await _taskController.GetTasks();

      // Then
      // Assert: verificamos se o resultado é um OkObjectResult e contém a lista esperada
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedTasks = Assert.IsType<List<TaskModel>>(okResult.Value);
      Assert.Equal(2, returnedTasks.Count);
    }

    /// <summary>
    /// Testa se GetTaskById retorna a tarefa correta quando existe.
    /// </summary>
    [Fact]
    public async Task GetTaskById_ShouldReturnOkWithTask_WhenTaskExists()
    {
      // Given
      // Arrange: criamos uma tarefa simulada
      var task = new TaskModel { Id = 1, Name = "Task 1", Description = "Description 1" };

      // Configuramos o mock para retornar a tarefa quando solicitada pelo ID
      _taskServiceMock.Setup(service => service.GetTaskByIdAsync(1)).ReturnsAsync(task);
    
      // When
      // Act: chamamos o método da controller passando o ID 1
      var result = await _taskController.GetTaskById(1);
    
      // Then
      // Assert: verificamos se o resultado é um OkObjectResult e contém a tarefa esperada
      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedTask = Assert.IsType<TaskModel>(okResult.Value);
      Assert.Equal(1, returnedTask.Id);
    }

    /// <summary>
    /// Testa se GetTaskById retorna NotFound quando a tarefa não existe.
    /// </summary>
    [Fact]
    public async Task GetTaskById_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
      // Given
      // Arrange: configuramos o mock para retornar null quando o ID for inválido
      _taskServiceMock.Setup(service => service.GetTaskByIdAsync(1)).ReturnsAsync((TaskModel?)null);
    
      // When
      // Act: chamamos o método da controller passando um ID inexistente
      var result = await _taskController.GetTaskById(1);
    
      // Then
      // Assert: verificamos se o resultado é um NotFoundResult
      Assert.IsType<NotFoundResult>(result);
    }

    /// <summary>
    /// Teste se CreateTask retorna CreatedAction quando a criação é bem-sucedida.
    /// </summary>
    [Fact]
    public async Task CreateTask_ShouldReturnCreatedAction_WhenTaskIsCreated()
    {
      // Given
      // Arrange: criamos uma tarefa simulada
      var task = new TaskModel { Id = 1, Name = "New Task", Description = "New Description" };

      // Configuramos o mock para simular a criação da tarefa
      _taskServiceMock.Setup(service => service.AddTaskAsync(task)).ReturnsAsync(task);

      // When
      // Act: chamamos o método da controller passando a nova tarefa
      var result = await _taskController.PostTask(task);

      // Then
      // Assert: verificamos se o resultado é um CreatedAtActionResult e contém a tarefa criada
      var createdResult = Assert.IsType<CreatedAtActionResult>(result);
      var returnedTask = Assert.IsType<TaskModel>(createdResult.Value);
      Assert.Equal("New Task", returnedTask.Name);
    }

    /// <summary>
    /// Testa se DeleteTask retorna NoContent quando a exclusão é bem-sucedida.
    /// </summary>
    [Fact]
    public async Task DeleteTask_ShouldReturnNoContent_WhenDeletionIsSuccessful()
    {
      // Given
      // Arrange: configuramos o mock para simular uma exclusão bem-sucedida
      _taskServiceMock.Setup(service => service.DeleteTaskAsync(1)).ReturnsAsync(true);
    
      // When
      // Act: chamamos o método da controller para excluír a tarefa com o ID 1
      var result = await _taskController.DeleteTask(1);
    
      // Then
      // Assert: verificamos se o resultado é um NoContentResult
      Assert.IsType<NoContentResult>(result);
    }

    /// <summary>
    /// Testa se DeleteTask retorna NotFound quando a tarefa não existe.
    /// </summary>
    [Fact]
    public async Task DeleteTask_ShouldReturnNotFound_WhenTaskDoesNotExist()
    {
      // Given
      // Arrange: configuramos o mock para simular falha na exclusão
      _taskServiceMock.Setup(service => service.DeleteTaskAsync(1)).ReturnsAsync(false);
    
      // When
      // Act: chamamos o método da controller para excluir a tarefa inexistente
      var result = await _taskController.DeleteTask(1);
    
      // Then
      // Assert: verificamos se o resultado é um NotFoundResult
      Assert.IsType<NotFoundResult>(result);
    }
  }
}