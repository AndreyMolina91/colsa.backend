using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using COLSA.DataAccess.Context;
using COLSA.Domain.Interfaces;
using COLSA.Domain.Models;
using COLSA.Infraestructure.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace COLSA.Infraestructure.Repositories
{
    public class UserRepo : GeneralAsyncRepo<UserModel>, IUser
    {
        private readonly UserResponseDto _userResponseDto;
        public UserRepo(ApplicationDbContext context, IConfiguration configuration) : base(context, configuration)
        {
            _userResponseDto = new UserResponseDto();
        }

        public async Task<bool> UserExist(string userName)
        {
            if (await _context.Users.AnyAsync(x => x.UserName.ToLower().Equals(userName.ToLower())))
            {
                return true;
            }
            return false;
        }

        public async Task<object> UserLogin(string userName, string password)
        {
            //verificacion de usuario si existe en la BD durante el login
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));

            if (user == null)
            {
                return "nouser";
            }
            else if (!PasswordValidate(password, user.PasswordHash, user.PasswordSalt))
            {
                return "wrongpassword";
            }
            else
            {
                _userResponseDto.UserName = user.UserName;
                _userResponseDto.Token = CreateToken(user);
                return _userResponseDto;
            }
        }

        public async Task<string> UserRegister(UserModel user, string password)
        {
            try
            {
                //Verificacion de existencia
                if (await UserExist(user.UserName))
                {
                    return "UserExist";
                }

                //Password Hash encriptacion de password del usuario y esta se guardara en la DB
                PasswordHashCreate(password, out byte[] passwordHash, out byte[] passwordSalt);

                //Despues de creada se la asignamos al objeto que se guardara en la DB
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return CreateToken(user);
            }
            catch (Exception)
            {
                return "ErrorRegister";
            }
        }

        // Users Methods 
        private void PasswordHashCreate(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool PasswordValidate(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //se genera un nuevo hash con los caracteres ingresados en password por el usuario
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                //el computedhash debe coincidir con el hash del password ingresado con el usuario
                //ese hash lo guardamos en esta variable
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                //hacemos un for que recorra toda nuestra variable y lea los caracteres que contiene
                for (int i = 0; i < computedHash.Length; i++)
                {
                    //verificamos que sea igual al guardado en la base de datos
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private string CreateToken(UserModel userModel)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()),
                new Claim(ClaimTypes.Name, userModel.UserName)
            };


            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}