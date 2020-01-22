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
    [Route("api/stocks")]
    [ApiController]
    [Authorize]
    public class StocksController : ControllerBase
    {
        private ILoggerManager _logger;
        private IStocksManager _quotes;
        private IRepositoryWrapper _repository;

        public StocksController(ILoggerManager logger, IRepositoryWrapper repository, IStocksManager quotes)
        {
            _logger = logger;
            _repository = repository;
            _quotes = quotes;
        }

        [HttpGet("quotes")]
        public async Task<IActionResult> ListQuotes()
        {
            if (_quotes.stocksValues.Count == 0)
                return NoContent();
            else
                return Ok(_quotes.stocksValues);
        }

        [HttpGet("investments/{id}")]
        public async Task<IActionResult> ListInvestments(string id)
        {
            try
            {
                var userEntity = _repository.User.Get(id);
                if (userEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                return Ok(_repository.Stock.Get(id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Buy Stocks action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("buy/{id}")]
        public async Task<IActionResult> BuyStocks(string id, [FromBody]Stock stocks)
        {
            try
            {
                if (stocks == null)
                {
                    _logger.LogError("Stocks object sent from client is null.");
                    return BadRequest("Stocks object is null");
                }

                if (stocks.Ammount <= 0)
                {
                    return BadRequest("The ammount of stocks must be greater than zero");
                }

                var userEntity = _repository.User.Get(id);
                if (userEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                Account userAccount = _repository.Account.Get(id);
                Stock userStocks = _repository.Stock.Get(id, stocks.StockCode);

                if (userAccount == null)
                {
                    _logger.LogError($"User with id: {id}, doesn't have an account with funds.");
                    return NotFound("This user doesn't have an account with Funds");
                }

                if (userStocks == null)
                {
                    userStocks = new Stock { UserId = id, StockCode = stocks.StockCode };
                    _repository.Stock.Create(userStocks);
                }

                //get price for the ammount
                var stockPrice = _quotes.stocksValues.FirstOrDefault(s => s.StockCode.Equals(stocks.StockCode));
                if (stockPrice == null)
                {
                    _logger.LogError($"Price not found for stock {stocks.StockCode}");
                    return NotFound("Stock not found");
                }

                var value = stocks.Ammount * stockPrice.Value;

                if (value > userAccount.Funds)
                {
                    _logger.LogError($"Insufficient funds");
                    return BadRequest("Insufficient funds to buy the ammount of stocks requested");
                }

                userAccount.Funds -= value;

                _repository.Account.Update(id, userAccount);

                userStocks.Ammount += stocks.Ammount;

                _repository.Stock.Update(id, userStocks);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Buy Stocks action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("sell/{id}")]
        public async Task<IActionResult> SellStocks(string id, [FromBody]Stock stocks)
        {
            try
            {
                if (stocks == null)
                {
                    _logger.LogError("Stocks object sent from client is null.");
                    return BadRequest("Stocks object is null");
                }

                if (stocks.Ammount <= 0)
                {
                    return BadRequest("The ammount of stocks must be greater than zero");
                }

                var userEntity = _repository.User.Get(id);
                if (userEntity == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                Account userAccount = _repository.Account.Get(id);
                Stock userStocks = _repository.Stock.Get(id, stocks.StockCode);

                if (userAccount == null)
                {
                    _logger.LogError($"User with id: {id}, doesn't have an account with funds.");
                    return NotFound("This user doesn't have an account with Funds");
                }

                //get price for the ammount
                var stockPrice = _quotes.stocksValues.FirstOrDefault(s => s.StockCode.Equals(stocks.StockCode));
                if (stockPrice == null)
                {
                    _logger.LogError($"Price not found for stock {stocks.StockCode}");
                    return NotFound("Stock not found");
                }

                if (userStocks == null)
                {
                    _logger.LogError($"User with id: {id}, doesn't have stocks to sell.");
                    return NotFound($"This user have 0 stocks of {stocks.StockCode} to sell");
                }

                if (stocks.Ammount > userStocks.Ammount)
                {
                    _logger.LogError($"The ammount selected is greater than the ammount owned by the user");
                    return BadRequest($"This user have only have {userStocks.Ammount} stocks to sell");
                }

                var value = stocks.Ammount * stockPrice.Value;

                userAccount.Funds += value;

                _repository.Account.Update(id, userAccount);

                userStocks.Ammount -= stocks.Ammount;

                if (userStocks.Ammount > 0)
                    _repository.Stock.Update(id, userStocks);
                else
                    _repository.Stock.Remove(id, userStocks.StockCode);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Buy Stocks action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}