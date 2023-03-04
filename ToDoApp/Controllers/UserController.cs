using Microsoft.AspNetCore.Mvc;
using ShopApp.DTOs;
using System.Security.Cryptography;
using System.Text;
using ToDoApp.Data.Models;
using ToDoApp.DTOs;
using ToDoApp.Repositories;

namespace ToDoApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            if (Request.Cookies["Email"]!=null)
                ViewData["email"]= Request.Cookies["Email"];
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            if (ModelState.IsValid)
            {
                if (loginUserDTO.RememberMe == true)
                {
                    Response.Cookies.Append("Email", loginUserDTO.Email, new CookieOptions { Expires=DateTime.Now.AddDays(7)});
                }
                var result=await _userRepository.Login(loginUserDTO);
                if (result.Type == "Error")
                {
                    ViewData["error"] = result.Message;
                    return View();
                }
                if(result.Type=="NotIdentified")
                {
                    ViewData["error"] = result.Message;
                    return View();
                }
                HttpContext.Session.SetString("ID", result.Data);
                return RedirectToAction("Index", "Todo");

            }
            return View();
        }
        public async Task<IActionResult> Register()
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            if (ModelState.IsValid)
            {
                var result=await _userRepository.Register(registerUserDTO);
                if (result.Type == "Success")
                {
                    TempData["check"] = "Please check your email and verify your account.";
                   return RedirectToAction("Login", "User");
                }
                else
                    ViewData["error"] = result.Message;
            }
            return View();
        }
        public async Task<IActionResult> Password(string token)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            var result=await _userRepository.CheckToken(token);
            if(result.Type=="Success")
            return View();
            TempData["error"] = result.Message;
            return RedirectToAction("Login", "User");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Password(string token, PasswordDTO passwordDTO)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            if (ModelState.IsValid){
                var result=await _userRepository.AddPassword(token,passwordDTO);
                if (result.Type == "Success")
                {
                    TempData["success"] = result.Message;
                    return RedirectToAction("Login", "User");
                }
                TempData["error"] = result.Message;
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        public async Task<IActionResult> ForgotPassword()
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordEmailDTO forgotPasswordEmailDTO)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            if (ModelState.IsValid)
            {
                var result = await _userRepository.CheckEmail(forgotPasswordEmailDTO);
                if (result.Type == "Success")
                {
                    TempData["check"] = "Please check your email.";
                    return RedirectToAction("Login", "User");
                }
                ViewData["error"] = result.Message;
            }
            return View();
        }
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            var result = await _userRepository.CheckPasswordToken(token);
            if (result.Type == "Success")
                return View();
            TempData["error"] = result.Message;
            return RedirectToAction("Login", "User");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, PasswordDTO passwordDTO)
        {
            if (HttpContext.Session.GetString("ID") != null)
            {
                return RedirectToAction("Index", "Todo");
            }
            var result = await _userRepository.ResetPassword(token, passwordDTO);
            if (result.Type == "Success")
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Login", "User");
            }
            TempData["error"] = result.Message;
            return RedirectToAction("Login", "User");
        }
    }
}