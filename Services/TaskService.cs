using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Serilog;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;
using TaskManagerAPI.Repositories;
using TaskManagerAPI.Services.Interfaces;

namespace TaskManagerAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<TaskItemDTO> _taskValidator;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IValidator<TaskItemDTO> taskValidator, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _taskValidator = taskValidator ?? throw new ArgumentNullException(nameof(taskValidator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TaskItemResponseDTO>> GetAllTasksAsync()
        {
            Log.Information("Buscando todas as tarefas.");
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskItemResponseDTO>>(tasks);
        }

        public async Task<TaskItemResponseDTO> GetTaskByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero.");

            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            return _mapper.Map<TaskItemResponseDTO>(task);
        }

        public async Task<IEnumerable<TaskItemResponseDTO>> GetTasksByStatusAsync(string status)
        {
            if (!Enum.TryParse(status, true, out Status taskStatus))
            {
                Log.Warning("Tentativa de buscar tarefas com status inválido: {Status}", status);
                throw new ArgumentException("Status inválido. Use: Pending, InProgress ou Completed.");
            }

            Log.Information("Buscando tarefas com status: {Status}", taskStatus);
            var tasks = await _taskRepository.GetTasksByStatusAsync(taskStatus);
            return _mapper.Map<IEnumerable<TaskItemResponseDTO>>(tasks);
        }

        public async Task<TaskItemResponseDTO> CreateTaskAsync(TaskItemDTO taskDto, ClaimsPrincipal user)
        {
            int userId = GetAuthenticatedUserId(user);

            var userEntity = await _userRepository.GetByIdAsync(userId);
            if (userEntity == null)
                throw new UnauthorizedAccessException("Usuário não encontrado.");

            var validationResult = await _taskValidator.ValidateAsync(taskDto);
            if (!validationResult.IsValid)
            {
                Log.Warning("Erro de validação ao criar tarefa: {Errors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var task = _mapper.Map<TaskItem>(taskDto);
            task.CreatedById = userEntity.Id;
            task.CreatedBy = userEntity;

            var createdTask = await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskItemResponseDTO>(createdTask);
        }

        public async Task<TaskItemResponseDTO> UpdateTaskAsync(int id, TaskItemDTO taskDto, ClaimsPrincipal user)
        {
            int userId = GetAuthenticatedUserId(user);
            var existingTask = await _taskRepository.GetByIdAsync(id);

            if (existingTask == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            if (existingTask.CreatedById != userId)
            {
                Log.Warning("Usuário {UserId} tentou editar a tarefa {TaskId} sem permissão.", userId, id);
                throw new UnauthorizedAccessException("Somente o criador da tarefa pode editá-la.");
            }

            var validationResult = await _taskValidator.ValidateAsync(taskDto);
            if (!validationResult.IsValid)
            {
                Log.Warning("Erro de validação ao atualizar tarefa {Id}: {Errors}", id, validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            _mapper.Map(taskDto, existingTask);

            var updatedTask = await _taskRepository.UpdateAsync(existingTask);
            return _mapper.Map<TaskItemResponseDTO>(updatedTask);
        }

        public async Task<bool> DeleteTaskAsync(int id, ClaimsPrincipal user)
        {
            int userId = GetAuthenticatedUserId(user);
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
                throw new KeyNotFoundException("Tarefa não encontrada.");

            if (task.CreatedById != userId)
            {
                Log.Warning("Usuário {UserId} tentou excluir a tarefa {TaskId} sem permissão.", userId, id);
                throw new UnauthorizedAccessException("Somente o criador da tarefa pode excluí-la.");
            }

            return await _taskRepository.DeleteAsync(id);
        }

        private int GetAuthenticatedUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Usuário não autenticado ou ID inválido.");
            }

            return userId;
        }
    }
}
