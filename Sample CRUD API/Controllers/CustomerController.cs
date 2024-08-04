using DATA.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using REPOSITORY.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
                    //Checking the existing user is empty or not
                    if (existingUser == null)
                    {
                        customer.Password = PasswordHasher.HashPassword(customer.Password);
                        customer.IsActive = true;
                        customer.CreatedOn = DateTime.UtcNow;
                        customer.ModifyOn = DateTime.UtcNow;
                        customer.Role = "User";
                        _customerRepository.Add(customer);
                        var result = new Result()
                        {
                            Status = true,
                            Item = customer,
                            Message = _settingVariables.CustomerCreated,
                            ErrorMessage = ""
                        };
                        return Ok(result);
                    }
                    else
                    {
                        var badResult = new BadRequest()
                        {
                            Status = false,
                            Message = _settingVariables.CustomerAlreadyExist,
                            Trace = ""
                        };
                        return BadRequest(badResult);
                    }
                }
                else
                {
                    var badResult = new BadRequest()
                    {
                        Status = false,
                        Message = "Please fill all field",
                        Trace = ""
                    };
                    return BadRequest(badResult);
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
                    var badResult = new BadRequest()
                    {
                        Status = false,
                        Message = "Please fill all field",
                        Trace = ""
                    };
                    return BadRequest(badResult);
                }
                else
                {
                    var ValidCustomer = _customerRepository.FindAll(x => x.Email == customerLogin.Email).FirstOrDefault();
                    if (ValidCustomer == null)
                    {
                        var badResult = new BadRequest()
                        {
                            Status = false,
                            Message = _settingVariables.UserNotRegister,
                            Trace = ""
                        };
                        return BadRequest(badResult);
                    }
                    else
                    {
                        if (PasswordHasher.VerifyPassword(customerLogin.Password, ValidCustomer.Password))
                        {
                            var CustomerDetails = new CustomerDetails()
                            {
                                CustomerId = ValidCustomer.CustomerId,
                                Name = $"{ValidCustomer.FirstName} {ValidCustomer.LastName}",
                                Email = ValidCustomer.Email,
                                Role = ValidCustomer.Role,
                                Token = CreateJwt(ValidCustomer),
                            };
                            var result = new Result()
                            {
                                Status = true,
                                Item = new {CustomerDetails = CustomerDetails},
                                Message = _settingVariables.UserLoggedIn,
                                ErrorMessage = ""
                            };
                            return Ok(result);
                        }
                        else
                        {
                            var badResult = new BadRequest()
                            {
                                Status = false,
                                Message = _settingVariables.ErrorEmailPassword,
                                Trace = ""
                            };
                            return BadRequest(badResult);
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
                if (Customers.Count() > 0)
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
        private string CreateJwt(Customer customer)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("pintusharmaqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqweqwe");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, customer.Role),
                new Claim(ClaimTypes.Name, $"{customer.FirstName} {customer.LastName}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
