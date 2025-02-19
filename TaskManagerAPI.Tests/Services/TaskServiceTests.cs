using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidator<TaskItemDTO>> _mockValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidator = new Mock<IValidator<TaskItemDTO>>();
            _mockMapper = new Mock<IMapper>();

            _taskService = new TaskService(
                _mockTaskRepository.Object,
                _mockUserRepository.Object,
                _mockValidator.Object,
                _mockMapper.Object
            );
        }

        private ClaimsPrincipal GetFakeUser(int userId)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));
        }

        [Fact]
        public async Task GetAllTasksAsync_Should_Return_TaskList()
        {
            var tasks = new List<TaskItem> { new TaskItem { Id = 1, Title = "Test Task" } };
            _mockTaskRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskItemResponseDTO>>(tasks))
                       .Returns(new List<TaskItemResponseDTO> { new TaskItemResponseDTO { Id = 1, Title = "Test Task" } });

            var result = await _taskService.GetAllTasksAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.Should().ContainSingle(t => t.Title == "Test Task");
        }

        
        [Fact]
        public async Task UpdateTaskAsync_Should_Throw_Exception_If_User_Not_Owner()
        {
            var taskDto = new TaskItemDTO { Title = "Updated Task", Description = "Updated Desc", Status = Status.InProgress };
            var fakeUser = GetFakeUser(2);

            var existingTask = new TaskItem { Id = 1, Title = "Old Task", CreatedById = 1 };

            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingTask);

            Func<Task> act = async () => await _taskService.UpdateTaskAsync(1, taskDto, fakeUser);

            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Somente o criador da tarefa pode editá-la.");
        }
    }
}
