using Jugueteria.Models.Segurity;
using Jugueteria.Models.StaticDictionary;
using Jugueteria.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jugueteria.Service.Repositories
{
    public class IdentityRepo : RepositoryBase<Users>, IIdentityRepo
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityRepo(ApplicationDbContext db, SignInManager<Users> signInManager,
            UserManager<Users> userManager, RoleManager<IdentityRole> roleManager) : base(db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region Login, logout, register, SignIn, SignInJwtTokenAsync --> retunrn token for JWT

        public async Task<SignInResult> LoginAsync(Login login)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public Task SignInAsync(Users user, bool isPersistent, string authenticationMethod = null)
        {
            // Get the information about the user from the external login provider
            var info = _signInManager.GetExternalLoginInfoAsync().Result;
            return _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
        }

        public async Task<Users> SignInJwtTokenAsync(Users user, string password, bool lockoutOnFailure = false)
        {
            try
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
                if (result.Succeeded)
                {
                    var userToken = await CreateToken(user.Email);
                    return userToken;
                }

                return null;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> RegisterAsync(Users user)
        {
            try
            {
                user.RegisterDate = DateTime.Now;
                user.Active = true;
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, user.Password);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region CUD 

        public async Task<IdentityResult> CreateAsync(Users user)
        {
            try
            {
                return await _userManager.CreateAsync(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IdentityResult> UpdateAsync(Users user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(user);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Getters

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            try
            {
                var users = await _db.User.ToListAsync();
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Users> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region JWT token

        public async Task<Users> CreateToken(string email)
        {
            try
            {
                var claims = new List<Claim> {
                  new Claim(JwtRegisteredClaimNames.NameId, email)

                };

                var roles = await _roleManager.Roles.ToListAsync();

                //roles               
                if (roles != null)
                {
                    foreach (var item in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.Name));
                    }
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SD.ApiKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(30), //DateTime.Now.AddDays(15),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescription);
                string tokenResult = "Bearer " + tokenHandler.WriteToken(token);

                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    user.Token = tokenResult;
                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Roles

        public async Task<List<IdentityRole>> RoleListAsync()
        {
            try
            {
                return await _roleManager.Roles.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> CreateRoleAsync(string role)
        {
            try
            {
                return await _roleManager.CreateAsync(new IdentityRole(role));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(Users user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> AddToRoleAsync(Users user, string role)
        {
            try
            {
                return await _userManager.AddToRoleAsync(user, role);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsInRole(Users user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        #endregion

        #region External Authentication

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return await _signInManager.GetExternalAuthenticationSchemesAsync();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl = null)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent = false, bool bypassTwoFactor = false)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }

        #endregion

        #region Password 

        public async Task<IdentityResult> PasswordValidatorAsync(string passwword)
        {
            try
            {
                var passwordValidator = new PasswordValidator<Users>();
                return await passwordValidator.ValidateAsync(_userManager, null, passwword);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string PasswordHashAsync(Users user, string passwod)
        {
            try
            {
                return _userManager.PasswordHasher.HashPassword(user, passwod);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(Users user)
        {
            try
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(Users user, string token, string newPassword)
        {
            try
            {
                return await _userManager.ResetPasswordAsync(user, token, newPassword);
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        public async Task<bool> UserIsActiveAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user.Active)
                {
                    return true;
                }

                return false;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IdentityResult> AddLoginAsync(Users user, UserLoginInfo info)
        {
            try
            {
                return await _userManager.AddLoginAsync(user, info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(Users user, string password, bool lockoutOnFailure = false)
        {
            try
            {
                return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            }
            catch (Exception)
            {
                throw;
            }
        }





    }
}
