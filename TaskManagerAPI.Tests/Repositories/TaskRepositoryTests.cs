using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;

namespace TaskManagerAPI.Tests.Repositories
{
    public class TaskRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly TaskRepository _taskRepository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new ApplicationDbContext(options);
            _taskRepository = new TaskRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Task_Successfully()
        {
            var task = new TaskItem
            {
                Title = "Test Task",
                Description = "Test Description",
                Status = Status.Pending,
                CreatedById = 1
            };

            var result = await _taskRepository.AddAsync(task);
            var savedTask = await _context.Tasks.FirstOrDefaultAsync();


            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            savedTask.Should().NotBeNull();
            savedTask.Title.Should().Be(task.Title);
        }



        [Fact]
        public async Task UpdateAsync_Should_Update_Task_Successfully()
        {
            var task = new TaskItem { Title = "Old Title", Description = "Old Desc", CreatedById = 1 };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            task.Title = "Updated Title";
            var result = await _taskRepository.UpdateAsync(task);

            result.Title.Should().Be("Updated Title");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Task()
        {

            var task = new TaskItem { Title = "Task to Delete", Description = "Desc", CreatedById = 1 };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var result = await _taskRepository.DeleteAsync(task.Id);
            var deletedTask = await _context.Tasks.FindAsync(task.Id);

            result.Should().BeTrue();
            deletedTask.Should().BeNull();
        }
    }
}
