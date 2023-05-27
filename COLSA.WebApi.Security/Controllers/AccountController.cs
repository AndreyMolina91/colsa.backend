using AutoMapper;
using COLSA.Domain.Interfaces;
using COLSA.Domain.Models;
using COLSA.Infraestructure.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace COLSA.WebApi.Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDto _responseDto;

        public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            // logger
            _logger = logger;
            // to use common methods with EF and LinQ
            _unitOfWork = unitOfWork;
            // To return msg description
            _responseDto = new ResponseDto();
            // to map models and Dtos
            _mapper = mapper;
        }

        /// <summary>
        /// Methods to allow users to authenticate into the app with user credentials
        /// </summary>
        /// <param name="model">Param UserLoginDto userName and password props</param>
        /// <returns>ActionResult with _responseDto info</returns>
        [HttpPost("Login")]
        public async Task<ActionResult> UserLogin(UserLoginDto model)
        {
            _logger.LogInformation("Try to login with userName : " + model.UserName + " IP: " + HttpContext.Connection.RemoteIpAddress);
            var response = await _unitOfWork.Users.UserLogin(model.UserName, model.Password);
            if (!ModelState.IsValid)
            {
                _responseDto.IsSuccess = false;
                _responseDto.DisplayMessage = "User data is not correct";
                return BadRequest(_responseDto);
            }

            if (response.ToString() == "nouser")
            {
                _responseDto.IsSuccess = false;
                _responseDto.DisplayMessage = "User don't exist";
                return BadRequest(_responseDto);
            }

            if (response.ToString() == "wrongpassword")
            {
                _responseDto.IsSuccess = false;
                _responseDto.DisplayMessage = "Password doesn't match";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = response;
            _responseDto.DisplayMessage = "User connected succesfully";
            return Ok(_responseDto);
        }

        /// <summary>
        /// Methods to allow users to authenticate into the app with user credentials
        /// </summary>
        /// <param name="model">Param UserRegisterDto information and password props</param>
        /// <returns>ActionResult with _responseDto info</returns>
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> CreateUsers(UserRegisterDto model)
        {
            var userToDB = _mapper.Map<UserModel>(model);

            var response = await _unitOfWork.Users.UserRegister(userToDB, model.Password);

            if (response == "UserExist")
            {
                _responseDto.IsSuccess = false;
                _responseDto.DisplayMessage = "User already exist";
                return BadRequest(_responseDto);
            }

            if (response == "ErrorRegister")
            {
                _responseDto.IsSuccess = false;
                _responseDto.DisplayMessage = "Creating User error";
                return BadRequest(_responseDto);
            }
            _responseDto.DisplayMessage = "User created";
            _responseDto.Result = response;
            return Ok(_responseDto);
        }
    }
}