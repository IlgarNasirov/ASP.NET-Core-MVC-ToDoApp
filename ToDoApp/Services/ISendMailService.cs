using ToDoApp.Data.Models;

namespace ToDoApp.Services
{
    public interface ISendMailService
    {
        public void SendMail(Mail mail);
    }
}
