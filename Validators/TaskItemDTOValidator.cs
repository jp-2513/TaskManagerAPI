using FluentValidation;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Validators
{
    public class TaskItemDTOValidator : AbstractValidator<TaskItemDTO>
    {
        public TaskItemDTOValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty().WithMessage("O título da tarefa é obrigatório.")
                .MinimumLength(3).WithMessage("O título deve ter pelo menos 3 caracteres.");

            RuleFor(task => task.Description)
                .NotEmpty().WithMessage("A descrição da tarefa é obrigatória.")
                .MinimumLength(10).WithMessage("A descrição deve ter pelo menos 10 caracteres.");

            RuleFor(task => task.Status)
                .IsInEnum().WithMessage("Status inválido. Use: Pending, InProgress ou Completed.");
        }
    }
}
