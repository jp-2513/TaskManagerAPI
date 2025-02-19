using System.Security.Claims;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItemResponseDTO>> GetAllTasksAsync();
        Task<TaskItemResponseDTO> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskItemResponseDTO>> GetTasksByStatusAsync(string status);
        Task<TaskItemResponseDTO> CreateTaskAsync(TaskItemDTO task, ClaimsPrincipal user);
        Task<TaskItemResponseDTO> UpdateTaskAsync(int id, TaskItemDTO task, ClaimsPrincipal user);
        Task<bool> DeleteTaskAsync(int id, ClaimsPrincipal user);
    }
}
