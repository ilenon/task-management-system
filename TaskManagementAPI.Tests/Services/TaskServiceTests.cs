using Moq;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using TaskManagementAPI.Repositories;
using Xunit;

namespace TaskManagementAPI.Tests.Services
{
  /// <summary>
  /// Classe de testes unitários para TaskService.
  /// </summary>
  public class TaskServiceTests
  {
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly ITaskService _taskService;

    /// <summary>
    /// Construtor dos testes, inicializa os mocks e a service.
    /// </summary>
    public TaskServiceTests()
    {
      _taskRepositoryMock = new Mock<ITaskRepository>();
      _taskService = new TaskService(_taskRepositoryMock.Object);
    }

    /// <summary>
    /// Testa se GetAllTasksAsync retorna uma lista de tarefas corretamente.
    /// </summary>
    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnTaskList()
    {
      // Given
      // Arrange: criamos uma lista simulada de tarefas
      var tasks = new List<TaskModel>
    {
      new TaskModel { Id = 1, Name = "Task 1", Description = "Description 1" },
      new TaskModel { Id = 2, Name = "Task 2", Description = "Description 2" }
    };

    _taskRepositoryMock.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(tasks);

      // When
      // Act: chamamos o método da service
      var result = await _taskService.GetAllTasksAsync();

      // Then
      // Assert: verificamos se a lista retornada é igual a esperada
      Assert.Equal(2, result.Count());
      Assert.Equal("Task 1", result.First().Name);
    }

    /// <summary>
    /// Testa se GetTaskByIdAsync retorna a tarefa correta quando existe
    /// </summary>
    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnCorrectTask()
    {
      // Given
      // Arrange: criamos uma tarefa de exemplo
      var task = new TaskModel { Id = 1, Name = "Task 1", Description = "Description 1" };

      _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(task);

      // When
      // Act: chamamos o método com o ID 1
      var result = await _taskService.GetTaskByIdAsync(1);

      // Then
      // Assert: verificamos se a tarefa retornada está correta
      Assert.NotNull(result);
      Assert.Equal(1, result.Id);
      Assert.Equal("Task 1", result.Name);
    }

    /// <summary>
    /// Testa se CreateTaskAsync chama o repositório corretamente.
    /// </summary>
    [Fact]
    public async Task CreateTaskAsync_ShouldCallRepositoryAndTask()
    {
      // Given
      // Arrange: criamos uma tarefa de exemplo
      var task = new TaskModel { Id = 1, Name = "New Task", Description = "New Description" };

      _taskRepositoryMock.Setup(repo => repo.AddTaskAsync(It.IsAny<TaskModel>())).ReturnsAsync(task);

      // When
      // Act: criamos a nova tarefa
      var result = await _taskService.AddTaskAsync(task);

      // Then
      // Assert: verificamos se o método foi chamado uma vez
      _taskRepositoryMock.Verify(repo => repo.AddTaskAsync(It.IsAny<TaskModel>()), Times.Once);
      Assert.Equal("New Task", result.Name);
    }

    /// <summary>
    /// Testa se deleteTaskAsync retorna true ao excluir uma tarefa existente.
    /// </summary>
    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnTrue_WhenTaskExists()
    {
      // Given
      // Arrange: simulamos que a exclusão foi bem-sucedida
      _taskRepositoryMock.Setup(repo => repo.DeleteTaskAsync(1)).ReturnsAsync(true);

      // When
      // Act: tentamos excluir a tarefa com o ID 1
      var result = await _taskService.DeleteTaskAsync(1);

      // Then
      // Assert: verificamos se o resultado foi verdadeiro
      Assert.True(result);
    }
  }
}