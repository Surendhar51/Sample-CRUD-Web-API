using DATA.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REPOSITORY.IRepository;
using UTILITY;

namespace Sample_CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly SettingVariables _settingVariables;

        public UserController(IUserRepository userRepository,SettingVariables settingVariables)
        { 
            _userRepository = userRepository;
            _settingVariables = settingVariables;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetUser()
        {
            try
            {
                var users = _userRepository.GetAll();
                if (users.Count() != 0)
                {
                    var result = new Result()
                    {
                        Status = true,
                        Item = new { Users = users ,Count = users.Count()},
                        Message =_settingVariables.Success,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
                else
                {
                    var result = new Result()
                    {
                        Status = true,
                        Item = users,
                        Message = _settingVariables.NoUser,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                var Result = new Result()
                {
                    Status = false,
                    Item = "",
                    Message = _settingVariables.NoUser,
                    ErrorMessage = ex.Message
                };
                return BadRequest(Result);
            }
        }

        [HttpPost]
        [Route("AddUsers")]
        public IActionResult AddUser(Users users)
        {
            try
            {
                var ExistingUser = _userRepository.FindAll(x => x.EmailAddress == users.EmailAddress).FirstOrDefault();
                if (ExistingUser == null)
                {
                    _userRepository.Add(users);
                    var result = new Result()
                    {
                        Status = true,
                        Item = users,
                        Message = _settingVariables.UserAdded,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
                else
                {
                    var result = new Result()
                    {
                        Status = true,
                        Message = _settingVariables.UserAlreadyExist,
                        Item = "",
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                var badRequest = new BadRequest()
                {
                    Status = false,
                    Message = _settingVariables.Failed,
                    Trace = ex.Message
                };
                return BadRequest(badRequest);
            }
        }
    }
}
