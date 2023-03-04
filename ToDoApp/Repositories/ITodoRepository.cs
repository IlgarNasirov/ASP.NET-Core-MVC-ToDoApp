using ShopApp.DTOs;
using ToDoApp.Data;
using ToDoApp.Data.Models;
using ToDoApp.DTOs;

namespace ToDoApp.Repositories
{
    public interface ITodoRepository
    {
        public Task<List<Todo>> AllTodos();
        public Task<CustomReturnDTO> AddTodo(TodoDTO todoDTO);
        public Task<CustomReturnDTO> DeleteTodo(int id);
        public Task<CustomReturnDTO> UpdateTodo(int id, string text);
        public Task<Todo> CheckTodo(int id);

    }
}
