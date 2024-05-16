using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.DataAccess.Repositories.Impl;

public class TodoListRepository : BaseRepository<TodoList>, ITodoListRepository
{
    public TodoListRepository(DatabaseContext context) : base(context) { }
}
