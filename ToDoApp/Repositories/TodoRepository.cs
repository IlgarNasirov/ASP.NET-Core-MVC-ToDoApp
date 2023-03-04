using Microsoft.EntityFrameworkCore;
using ShopApp.DTOs;
using ToDoApp.Data;
using ToDoApp.Data.Models;
using ToDoApp.DTOs;

namespace ToDoApp.Repositories
{
    public class TodoRepository:ITodoRepository
    {
        private readonly TodoDbContext _todoDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TodoRepository(TodoDbContext todoDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _todoDbContext = todoDbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Todo>> AllTodos()
        {
            var result = _httpContextAccessor.HttpContext.Session.GetString("ID");
            return await _todoDbContext.Todos.Where(t => t.UserId == Convert.ToInt32(result)).ToListAsync();
        }
        public async Task<CustomReturnDTO> AddTodo(TodoDTO todoDTO)
        {
            var result = _httpContextAccessor.HttpContext.Session.GetString("ID");
            if (result != null)
            {
                Todo todo = new Todo();
                todo.Text = todoDTO.Text;
                todo.UserId = Convert.ToInt32(result);
                await _todoDbContext.Todos.AddAsync(todo);
                await _todoDbContext.SaveChangesAsync();
                return new CustomReturnDTO { Type = "Success" };
            }
            return new CustomReturnDTO { Type = "Unauthenticated" };
        }
        public async Task<CustomReturnDTO>DeleteTodo(int id)
        {
            var result = _httpContextAccessor.HttpContext.Session.GetString("ID");
            if (result != null)
            {
                var todo = await _todoDbContext.Todos.Where(t => t.Id == id && t.UserId == Convert.ToInt32(result)).FirstOrDefaultAsync();
                if (todo == null)
                {
                    return new CustomReturnDTO { Type = "Error", Message = "Todo could not found!" };
                }
                _todoDbContext.Todos.Remove(todo);
                await _todoDbContext.SaveChangesAsync();
                return new CustomReturnDTO { Type = "Success" };
            }
            return new CustomReturnDTO { Type = "Unauthenticated" };
        }
        public async Task<Todo>CheckTodo(int id)
        {
            var result = _httpContextAccessor.HttpContext.Session.GetString("ID");
            return await _todoDbContext.Todos.Where(t => t.Id == id && t.UserId == Convert.ToInt32(result)).FirstOrDefaultAsync();
        }
        public async Task<CustomReturnDTO>UpdateTodo(int id, string text)
        {
            var result = _httpContextAccessor.HttpContext.Session.GetString("ID");
            if (result != null)
            {
                var todo = await _todoDbContext.Todos.Where(t => t.Id == id && t.UserId == Convert.ToInt32(result)).FirstOrDefaultAsync();
                if (todo == null)
                {
                    return new CustomReturnDTO { Type = "Error", Message = "Todo could not found!" };
                }
                todo.Text = text;
                await _todoDbContext.SaveChangesAsync();
                return new CustomReturnDTO { Type = "Success" };
            }
            return new CustomReturnDTO { Type = "Unauthenticated" };
        }
    }
}
