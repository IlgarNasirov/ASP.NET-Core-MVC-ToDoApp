using Microsoft.AspNetCore.Mvc;
using System;
using ToDoApp.Data.Models;
using ToDoApp.DTOs;
using ToDoApp.Repositories;

namespace ToDoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("ID") == null)
            {
               return RedirectToAction("Login", "User");
            }
            return View(await _todoRepository.AllTodos());
        }
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("ID") == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoDTO todoDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _todoRepository.AddTodo(todoDTO);
                if (result.Type == "Success")
                {
                    return RedirectToAction("Index", "Todo");
                }
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            var result=await _todoRepository.DeleteTodo(id);
            if (result.Type == "Error")
            {
                TempData["error"] = result.Message;
            }
            if(result.Type== "Unauthenticated")
            {
                return RedirectToAction("Login", "User");
            }
            return RedirectToAction("Index", "Todo");
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("ID") == null)
            {
                return RedirectToAction("Login", "User");
            }
            var result = await _todoRepository.CheckTodo(id);
            if (result == null)
            {
                TempData["error"] = "Todo could not found!";
                return RedirectToAction("Index", "Todo");
            }
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string text)
        {
            if(ModelState.IsValid)
            {
                var result = await _todoRepository.UpdateTodo(id, text);
                if(result.Type=="Error")
                {
                    TempData["error"] = result.Message;
                }
                if(result.Type== "Unauthenticated")
                {
                    return RedirectToAction("Login", "User");
                }
                return RedirectToAction("Index", "Todo");
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("ID");
            return RedirectToAction("Login", "User");
        }
    }
}
