using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StocksBackend.Controllers
{
    [Route("api/funds")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private ILoggerManager _logger;
        private IStocksManager _quotes;
        private IRepositoryWrapper _repository;

        public AccountController(ILoggerManager logger, IRepositoryWrapper repository, IStocksManager quotes)
        {
            _logger = logger;
            _repository = repository;
            _quotes = quotes;
        }

        [HttpGet("viewFunds/{id}")]
        public async Task<IActionResult> ViewFunds(string id)
        {
            try
            {
                var userEntity = _repository.User.Get(id);
                if (userEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                return Ok(_repository.Account.Get(id) == null ? 0 : _repository.Account.Get(id).Funds);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Buy Stocks action: {ex.Message}");
                return StatusCode(500, ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        [HttpPut("deposit/{id}")]
        public async Task<IActionResult> DepositFunds(string id, [FromBody]Account account)
        {
            try
            {
                if (account == null)
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null");
                }

                if (account.Ammount <= 0)
                {
                    return BadRequest("The ammount to be deposited must be greater than zero");
                }

                var userEntity = _repository.User.Get(id);
                if (userEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                Account userAccount = _repository.Account.Get(id);

                if (userAccount == null)
                {
                    userAccount = new Account { Ammount = 0, Funds = 0, UserId = id };
                    _repository.Account.Create(userAccount);
                }

                userAccount.Funds += account.Ammount;

                _repository.Account.Update(id, userAccount);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Buy Stocks action: {ex.Message}");
                return StatusCode(500, ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        [HttpPut("withdraw/{id}")]
        public async Task<IActionResult> WithdrawFunds(string id, [FromBody]Account account)
        {
            try
            {
                if (account == null)
                {
                    _logger.LogError("Account object sent from client is null.");
                    return BadRequest("Account object is null");
                }

                if (account.Ammount <= 0)
                {
                    return BadRequest("The ammount to be withdrawn must be greater than zero");
                }

                var userEntity = _repository.User.Get(id);
                if (userEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                Account userAccount = _repository.Account.Get(id);

                if (userAccount == null || userAccount.Funds < account.Ammount)
                {
                    _logger.LogError($"User with id: {id}, doesn't have sufficient funds to withdraw");
                    return BadRequest($"User with id: {id}, doesn't have sufficient funds to withdraw");
                }

                userAccount.Funds -= account.Ammount;

                _repository.Account.Update(id, userAccount);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Buy Stocks action: {ex.Message}");
                return StatusCode(500, ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}