using BackendFriend.Contracts;
using BackendFriend.Data.Models;
using BackendFriend.Extensions.Exceptions;
using BackendFriend.Repositories;
using BackendFriend.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BackendFriend.Controllers
{
    [Route("v1")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _users;

        public UsersController(IUserRepository users)
        {
            _users = users;
        }

        [HttpPost]
        [Route("auth/register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new MyBadRequestException("Данные введены некорректно");
            }

            if (await _users.UserExists(request.Login!))
            {
                throw new MyBadRequestException("Пользователь уже существует");
            }

            DateTime dob = DateTime.Parse(request.Birth!);
            int userAge = AgeValidation.UserAge(dob);

            if (userAge < 18 && userAge > 0)
            {
                throw new MyBadRequestException("Сервис доступен только для совершеннолетних");
            }
            else if (userAge <= 0)
            {
                throw new MyBadRequestException("Дата рождения введена некорректно");
            }

            User user = new()
            {
                Id = Guid.NewGuid(),
                Phone = request.Phone,
                Login = request.Login,
                Password = request.Password,
                Name = request.Name,
                Birth = request.Birth,
                Tg = request.Tg,
                Email = request.Email
            };

            var resp = new AuthUserResponse
            {
                Id = user.Id
            };

            await _users.Create(user);

            return StatusCode(201, resp);
        }

        [HttpPost]
        [Route("auth/login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                throw new MyBadRequestException("Данные введены некорректно");
            }

            if (!await _users.UserExists(request.Login!))
            {
                throw new MyNotFoundException("Пользователь не найден");
            }

            var user = await _users.GetUserByLogin(request.Login!);

            if (user.Password != request.Password)
            {
                throw new MyBadRequestException("Пароль введен некорректно");
            }

            var resp = new AuthUserResponse
            {
                Id = user.Id
            };

            return StatusCode(200, resp);
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<User>> GetUser([BindRequired] Guid id)
        {
            var user = await _users.GetUserById(id);

            if (user == null)
            {
                throw new MyNotFoundException("Пользователь не найден");
            }

            var resp = new GetUserResponse
            {
                Id = user.Id,
                Phone = user.Phone,
                Login = user.Login,
                Name = user.Name,
                Birth = user.Birth,
                Tg = user.Tg,
                Email = user.Email
            };

            return StatusCode(200, resp);
        }
    }
}
