using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApp.DTOs;
using System.Security.Cryptography;
using ToDoApp.Data;
using ToDoApp.Data.Models;
using ToDoApp.DTOs;
using ToDoApp.Services;
using static System.Net.WebRequestMethods;

namespace ToDoApp.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly TodoDbContext _todoDbContext;
        private readonly ISendMailService _sendMailService;
        public UserRepository(TodoDbContext todoDbContext, ISendMailService sendMailService)
        {
            _todoDbContext = todoDbContext;
            _sendMailService = sendMailService;
        }
        public async Task<CustomReturnDTO> Login(LoginUserDTO loginUserDTO)
        {
            var user=await _todoDbContext.Users.Where(u=>u.Email == loginUserDTO.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomReturnDTO{ Type="Error", Message="Invalid email or password!"};
            }
            if (user.Status == false)
            {
                return new CustomReturnDTO { Type = "NotIdentified", Message = "User not identified!" };
            }
            if (!BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
            {
                return new CustomReturnDTO { Type = "Error", Message = "Invalid email or password!" };
            }
            return new CustomReturnDTO { Type = "Success", Data=user.Id.ToString()};
        }
        public async Task<CustomReturnDTO> Register(RegisterUserDTO registerUserDTO)
        {

            var user=await _todoDbContext.Users.Where(u=>u.Email==registerUserDTO.Email).FirstOrDefaultAsync();
            string link;
            if (user == null)
            {
                string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                user = new User
                {
                    FullName = registerUserDTO.FullName,
                    Email = registerUserDTO.Email,
                    ActivateAccountToken = token
                };
                await _todoDbContext.Users.AddAsync(user);
                await _todoDbContext.SaveChangesAsync();
                link = "https://localhost:7099/user/password?token=" + token;
                _sendMailService.SendMail(
                new Mail
                {
                    Subject = "Verify your account",
                    From = registerUserDTO.Email,
                    Body = string.Format("<a href={0}>Please click this link and verify your account.</a>", link)
                });
                return new CustomReturnDTO { Type = "Success" };
            }
                if (user.Status == false)
                {
                    link = "https://localhost:7099/user/password?token=" + user.ActivateAccountToken;
                    _sendMailService.SendMail(new Mail
                    {
                        Subject = "Verify your account",
                        From = user.Email,
                        Body = string.Format("<a href={0}>Please click this link and verify your account.</a>", link)
                    });
                    return new CustomReturnDTO { Type = "Success" };
                }
                return new CustomReturnDTO { Type = "Error", Message = "This email already exists!" };
        }
        public async Task<CustomReturnDTO> CheckToken(string token)
        {
            var user=await _todoDbContext.Users.Where(u=>u.ActivateAccountToken== token&&token!=null).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomReturnDTO { Type = "Error", Message = "Invalid url!" };
            }
            return new CustomReturnDTO { Type = "Success"};
        }
        public async Task<CustomReturnDTO> AddPassword(string token, PasswordDTO passwordDTO)
        {
            var user = await _todoDbContext.Users.Where(u => u.ActivateAccountToken == token && token != null).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomReturnDTO { Type = "Error", Message = "Invalid url!" };
            }
            user.ActivateAccountToken = null;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordDTO.Password);
            user.Status = true;
            user.ActivatedTime = DateTime.Now;
            await _todoDbContext.SaveChangesAsync();
            return new CustomReturnDTO { Type = "Success", Message = "User successfully verified!" };
        }
        public async Task<CustomReturnDTO> CheckEmail(ForgotPasswordEmailDTO forgotPasswordEmailDTO)
        {
            var user = await _todoDbContext.Users.Where(u => u.Email == forgotPasswordEmailDTO.Email).FirstOrDefaultAsync();
            string link;
            if (user == null)
            {
                return new CustomReturnDTO { Type = "Error", Message = "User with this email could not be found!" };
            }
            if(user.PasswordResetToken!=null && user.ResetExpireDate > DateTime.Now)
            {
                link = "https://localhost:7099/user/resetpassword?token=" + user.PasswordResetToken;
                _sendMailService.SendMail(new Mail
                {
                    From = user.Email,
                    Subject = "Change your password",
                    Body = string.Format("<a href={0}>Please click this link and change your password.</a>", link)
                });
                return new CustomReturnDTO { Type = "Success"};
            }
            string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            user.PasswordResetToken = token;
            user.ResetExpireDate = DateTime.Now.AddMinutes(30);
            await _todoDbContext.SaveChangesAsync();
            link = "https://localhost:7099/user/resetpassword?token=" + token;
            _sendMailService.SendMail(new Mail
            {
                From = user.Email,
                Subject="Change your password",
                Body = string.Format("<a href={0}>Please click this link and change your password.</a>", link)
            });
            return new CustomReturnDTO { Type = "Success"};
        }
        public async Task<CustomReturnDTO>CheckPasswordToken(string token)
        {
            var user = await _todoDbContext.Users.Where(u => u.PasswordResetToken == token&&u.ResetExpireDate>DateTime.Now && token != null).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomReturnDTO { Type = "Error", Message = "Invalid url!" };
            }
            return new CustomReturnDTO { Type = "Success"};
        }
        public async Task<CustomReturnDTO>ResetPassword(string token, PasswordDTO passwordDTO)
        {
            var user = await _todoDbContext.Users.Where(u => u.PasswordResetToken == token && u.ResetExpireDate > DateTime.Now && token != null).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CustomReturnDTO { Type = "Error", Message = "Invalid url!" };
            }
            user.PasswordResetToken = null;
            user.ResetExpireDate = null;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordDTO.Password);
            await _todoDbContext.SaveChangesAsync();
            return new CustomReturnDTO { Type = "Success", Message = "Password successfully changed!" };
        }
    }
}
