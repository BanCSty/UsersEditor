using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersEditor.Models;
using UsersEditor.Repository;

namespace UsersEditor.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("Admin")]
    public class AdminController : ControllerBase
    {
        //Создание пользователя по логину...
        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserAsync(UserCreateDTO user)
        {
            await UserRepository.CreateUser(User.Identity.Name, user);
            return Ok();
        }

        //Запрос списка всех активных пользователей
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var user = await UserRepository.GetAllUser();

            if (user == null) throw new Exception("List is empty");

            return Ok(user);
        }

        //Запрос пользователя по логину и паролю
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserAsync(string login)
        {
            var user = await UserRepository.Find(login);

            if (user == null) throw new Exception("User not found");

            UserLogNameGenderBirthdayDTO result = new()
            {
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                State = user.RevokedOn == DateTime.MinValue ? true : false
            };

            return Ok(result);
        }

        //Запрос всех пользователей старше определённого возраста
        [HttpGet("GetOlderUsers")]
        public async Task<IActionResult> GetAllOlderUsersAsync(int age)
        {
            var user = await UserRepository.GetOlderUsers(age);

            if (user == null) throw new Exception("Users not found");

            return Ok(user);
        }


        //Удаление пользователя по логину полное или мягкое
        [HttpGet("SoftDeleteUser")]
        public async Task<IActionResult> SoftDeleteUserAsync(string login)
        {
            var user = await UserRepository.Find(login);

            if (user == null) throw new Exception("User not found");

            await UserRepository.SoftDeleteUser(User.Identity.Name, login);

            return Ok();
        }

        [HttpGet("HardDeleteUser")]
        public async Task<IActionResult> HardDeleteUserAsync(string login)
        {
            var user = await UserRepository.Find(login);

            if (user == null) throw new Exception("User not found");

            await UserRepository.HardDeleteUser(login);

            return Ok();
        }

        //Восстановление пользователя
        [HttpGet("RecoveryUser")]
        public async Task<IActionResult> RecoveryUserAsync(string login)
        {
            var user = await UserRepository.Find(login);

            if (user == null) throw new Exception("User not found");

            await UserRepository.HardDeleteUser(login);

            return Ok();
        }

    }
}
