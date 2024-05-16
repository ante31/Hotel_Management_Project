using AutoMapper;
using HotelMS.Application.Models.TodoList;
using HotelMS.Core.Entities;

namespace HotelMS.Application.MappingProfiles;

public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap<CreateTodoListModel, TodoList>();

        CreateMap<TodoList, TodoListResponseModel>();
    }
}
