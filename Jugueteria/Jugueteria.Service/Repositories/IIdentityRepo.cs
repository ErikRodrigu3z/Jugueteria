using Jugueteria.Models.Segurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jugueteria.Service.Repositories
{
    public interface IIdentityRepo
    {
        Task<IdentityResult> RegisterAsync(Users user);
        Task<SignInResult> LoginAsync(Login login);
        Task<SignInResult> CheckPasswordSignInAsync(Users user, string password, bool lockoutOnFailure = false);
        Task<Users> SignInJwtTokenAsync(Users user, string password, bool lockoutOnFailure = false);
        Task LogoutAsync();
        Task<bool> UserIsActiveAsync(string email);
        Task<IdentityResult> CreateAsync(Users user);
        Task<IdentityResult> AddLoginAsync(Users user, UserLoginInfo info);
        Task SignInAsync(Users user, bool isPersistent, string authenticationMethod = null);
        Task<IdentityResult> UpdateAsync(Users user);
        Task<IdentityResult> DeleteUserAsync(string id);

        //getters
        Task<Users> GetUserByEmailAsync(string email);
        Task<Users> GetUserByIdAsync(string id);
        Task<List<Users>> GetUsersAsync();

        //jwt token
        Task<Users> CreateToken(string email);

        //roles
        Task<List<IdentityRole>> RoleListAsync();
        Task<IdentityResult> CreateRoleAsync(string role);
        Task<IdentityResult> RemoveFromRoleAsync(Users user, string role);
        Task<IdentityResult> AddToRoleAsync(Users user, string role);
        Task<bool> IsInRole(Users user, string role);


        //external authentication
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string returnUrl = null);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent = false, bool bypassTwoFactor = false);

        //password validator
        Task<IdentityResult> PasswordValidatorAsync(string passwword);

        //password hasher
        string PasswordHashAsync(Users user, string passwod);

        //password reset
        Task<string> GeneratePasswordResetTokenAsync(Users user);
        Task<IdentityResult> ResetPasswordAsync(Users user, string token, string newPassword);



    }
}
