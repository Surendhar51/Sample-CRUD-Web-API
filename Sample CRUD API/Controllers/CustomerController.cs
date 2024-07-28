using DATA.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOSITORY.IRepository;
using UTILITY;

namespace Sample_CRUD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly SettingVariables _settingVariables;

        public CustomerController(ICustomerRepository customerRepository, SettingVariables settingVariables)
        {
            _customerRepository = customerRepository;
            _settingVariables = settingVariables;
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(Customer customer)
        {
            try
            {
                if (customer != null)
                {
                    var existingUser = _customerRepository.FindAll(x => x.Email == customer.Email).FirstOrDefault();
                    if (existingUser == null)
                    {
                        _customerRepository.Add(customer);
                        var result = new Result()
                        {
                            Status = true,
                            Message = _settingVariables.CustomerCreated,
                            Item = "",
                            ErrorMessage = ""
                        };
                        return Ok(result);
                    }
                    else
                    {
                        var result = new Result()
                        {
                            Status = false,
                            Message = _settingVariables.CustomerAlreadyExist,
                            Item = "",
                            ErrorMessage = ""
                        };
                        return Ok(result);
                    }
                }
                else
                {
                    var result = new Result()
                    {
                        Status = false,
                        Message = "",
                        Item = "",
                        ErrorMessage = "Please enter all fields"
                    };
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                var badResult = new BadRequest()
                {
                    Status = false,
                    Message = ex.Message,
                    Trace = ""
                };
                return BadRequest(badResult);
            }

        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(CustomerLogin customerLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(customerLogin.Email) || string.IsNullOrEmpty(customerLogin.Password))
                {
                    var result = new Result()
                    {
                        Status = true,
                        Message = "",
                        Item = "",
                        ErrorMessage = "Please enter all fields"
                    };
                    return Ok(result);
                }
                else
                {
                    var ValidCustomer = _customerRepository.FindAll(x => x.Email == customerLogin.Email).FirstOrDefault();
                    if (ValidCustomer == null)
                    {
                        var result = new Result()
                        {
                            Status = true,
                            Item = "",
                            Message = _settingVariables.UserNotRegister,
                            ErrorMessage = ""
                        };
                        return Ok(result);
                    }
                    else
                    {
                        if (customerLogin.Password == ValidCustomer.Password)
                        {
                            var result = new Result()
                            {
                                Status = true,
                                Item = ValidCustomer,
                                Message = _settingVariables.UserLoggedIn,
                                ErrorMessage = ""
                            };
                            return Ok(result);
                        }
                        else
                        {
                            var result = new Result()
                            {
                                Status = true,
                                Item = "",
                                Message = _settingVariables.ErrorEmailPassword,
                                ErrorMessage = ""
                            };
                            return Ok(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var badResult = new BadRequest()
                {
                    Status = false,
                    Message = ex.Message,
                    Trace = ""
                };
                return BadRequest(badResult);
            }

        }
        [HttpGet]
        [Route("GetALlCustomer")]
        public IActionResult GetAllCustomer()
        {
            try
            {
                var Customers = _customerRepository.GetAll();
                if (Customers != null)
                {
                    var result = new Result()
                    {
                        Status = true,
                        Item = new { Customers = Customers ,Count = Customers.Count() },
                        Message = _settingVariables.GetCustomers,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
                else
                {
                    var result = new Result()
                    {
                        Status = true,
                        Item = "",
                        Message = _settingVariables.NoCustomers,
                        ErrorMessage = ""
                    };
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                var badResult = new BadRequest()
                {
                    Status = false,
                    Message = ex.Message,
                    Trace = ""
                };
                return BadRequest(badResult);
            }
        }
    }
}
