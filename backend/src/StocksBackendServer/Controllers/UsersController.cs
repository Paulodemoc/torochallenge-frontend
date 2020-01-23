using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StocksBackendServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private ILoggerManager _logger;
        private IStocksManager _quotes;
        private IRepositoryWrapper _repository;

        public UsersController(IUserService userService, ILoggerManager logger, IRepositoryWrapper repository, IStocksManager quotes)
        {
            _userService = userService;
            _logger = logger;
            _repository = repository;
            _quotes = quotes;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet("getUserData/{id}")]
        public IActionResult GetUserData(string id)
        {
            var userEntity = _repository.User.Get(id);
            if (userEntity == null)
            {
                _logger.LogError($"User with id: {id}, hasn't been found in db.");
                return NotFound();
            }
            userEntity.Account = _repository.Account.Get(id);
            userEntity.Portfolio = _repository.Stock.Get(id).ToArray();
            userEntity.Password = null;

            return Ok(userEntity);
        }
    }
}