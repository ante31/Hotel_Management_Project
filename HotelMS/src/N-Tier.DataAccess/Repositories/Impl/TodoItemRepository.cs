using HotelMS.Core.Entities;
using HotelMS.DataAccess.Persistence;

namespace HotelMS.DataAccess.Repositories.Impl;

public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(DatabaseContext context) : base(context) { }
}
