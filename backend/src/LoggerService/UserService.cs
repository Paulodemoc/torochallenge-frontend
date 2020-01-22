using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Contracts;
using Entities.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StocksService;

namespace Services
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private IRepositoryWrapper _repository;

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IRepositoryWrapper repository)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }

        public User Authenticate(string username, string password)
        {
            var user = _repository.User.Get(username, password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }
    }
}