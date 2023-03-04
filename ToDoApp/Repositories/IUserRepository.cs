using ShopApp.DTOs;
using ToDoApp.DTOs;

namespace ToDoApp.Repositories
{
    public interface IUserRepository
    {
        public Task<CustomReturnDTO> Login(LoginUserDTO loginUserDTO);
        public Task<CustomReturnDTO> Register(RegisterUserDTO registerUserDTO);
        public Task<CustomReturnDTO> CheckToken(string token);
        public Task<CustomReturnDTO> AddPassword(string token, PasswordDTO passwordDTO);
        public Task<CustomReturnDTO> CheckEmail(ForgotPasswordEmailDTO forgotPasswordEmailDTO);
        public Task<CustomReturnDTO> CheckPasswordToken(string token);
        public Task<CustomReturnDTO> ResetPassword(string token, PasswordDTO passwordDTO);

    }
}
