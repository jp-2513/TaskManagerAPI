using AutoMapper;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Mappings
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {

            CreateMap<TaskItemDTO, TaskItem>();

            CreateMap<TaskItem, TaskItemResponseDTO>()
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.Name));
        }
    }
}
