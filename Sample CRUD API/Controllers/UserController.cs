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

        public UserController(IUserRepository userRepository, SettingVariables settingVariables)
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
                        Item = new { Users = users, Count = users.Count() },
                        Message = _settingVariables.Success,
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
                var badRequest = new BadRequest()
                {
                    Status = false,
                    Message = "",
                    Trace = ex.Message
                };
                return BadRequest(badRequest);
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

        [HttpGet]
        [Route("GetUserbyId{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var existingUser = _userRepository.Get(id);
                if (existingUser != null)
                {
                    var result = new Result()
                    {
                        Status = true,
                        Item = existingUser,
                        Message = _settingVariables.UserGetSuccess,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
                else
                {
                    var result = new Result()
                    {
                        Status = false,
                        Item = existingUser,
                        Message = _settingVariables.UserNotFound,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                var badRequest = new BadRequest()
                {
                    Status = false,
                    Message = ex.Message,
                    Trace = ""
                };
                return BadRequest(badRequest);
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(Users user)
        {
            try
            {
                var existingUser = _userRepository.FindAll(x=> x.Id == user.Id).FirstOrDefault();
                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.EmailAddress = user.EmailAddress;
                    var UpdateUser = _userRepository.Update(existingUser, existingUser.Id);
                    var result = new Result()
                    {
                        Status = true,
                        Item = UpdateUser,
                        Message = _settingVariables.UserUpdateSuccess,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
                else
                {
                    var result = new Result()
                    {
                        Status = false,
                        Item = null,
                        Message = _settingVariables.UserNotFound,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
            }
            catch (Exception ex) 
            {
                var badrequst = new BadRequest()
                {
                    Status = false,
                    Message = ex.Message,
                    Trace = ""
                };
                return BadRequest(badrequst);
            }
        }

        [HttpDelete]
        [Route("DeleteUser{id}")]
        public IActionResult DeleteUser(int id) 
        {
            try
            {
                var ExistingUser = _userRepository.FindAll(x => x.Id == id).FirstOrDefault();
                if (ExistingUser != null) 
                {
                    _userRepository.Delete(ExistingUser);
                    var result = new Result()
                    {
                        Status = true,
                        Item = new {Users = _userRepository.GetAll(), Count = _userRepository.Count()},
                        Message = _settingVariables.UserDeleted,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
                else
                {
                    var result = new Result()
                    {
                        Status = false,
                        Item = null,
                        Message = _settingVariables.UserNotFound,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                var badrequst = new BadRequest()
                {
                    Status = false,
                    Message = ex.Message,
                    Trace = ""
                };
                return BadRequest(badrequst);
            }
        }
    }
}
