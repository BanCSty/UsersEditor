using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersEditor.Models;
using UsersEditor.Repository;

namespace UsersEditor.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        //Изменение имени, пола или даты рождения пользователя
        [Authorize]
        [HttpPost("UpdateName")]
        public async Task<IActionResult> UpdateNameAsync(string name, int gender, DateTime birthday)
        {
            var user = await UserRepository.Find(User.Identity.Name);

            UserValidator(user);

            await UserRepository.UpdateName(user, name, gender, birthday);

            return Ok();
        }

        //Изменение пароля
        [Authorize]
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePasswordAsync(string password)
        {
            var user = await UserRepository.Find(User.Identity.Name);

            UserValidator(user);

            await UserRepository.UpdatePassword(user, password);

            return Ok();
        }
        //Изменение логина
        [Authorize]
        [HttpPost("UpdateLogin")]
        public async Task<IActionResult> UpdateLoginAsync(string login)
        {
            var user = await UserRepository.Find(User.Identity.Name);

            UserValidator(user);

            await UserRepository.UpdateLogin(user, login);

            //После смены логина в "БД" User.Identity.Name остаётся прежним и я не нашёл способа переназначить логин
            //Единственное, что пришло в голову - повторная авторизация. При повторной авторизации -
            //Создатся новый принципал с User.Identity.Name взятого User.Login с "БД".

            return LocalRedirect("~/logout"); //Принудительный logout
        }

        private void UserValidator(User user)
        {
            if (user == null || user.RevokedOn > DateTime.MinValue)
                throw new ArgumentException("User not found");
        }
    }
}

