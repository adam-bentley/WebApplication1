using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApplication1.Controllers
{
    public class AccountsController : ControllerBase
    {
        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUserStore<ApplicationUser> _userStore;

        /// <summary>
        /// The sign in manager
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// The token service
        /// </summary>
        private readonly TokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountsController"/> class.
        /// Sets the <see cref="_userManager"/>, <see cref="_signInManager"/> and <see cref="_tokenService"/> from the services.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="tokenService">The token service.</param>
        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            return result.Succeeded ? await CreateUserObject(user) : Unauthorized();
        }

        /// <summary>
        /// Creates a <see cref="UserDto"/> from an <see cref="ApplicationUser"/>
        /// </summary>
        /// <param name="user">The <see cref="ApplicationUser"/></param>
        /// <returns>A <see cref="UserDto"/> of the user</returns>
        private async Task<UserDto> CreateUserObject(ApplicationUser user)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            var i = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _tokenService.CreateToken(user, roles),
                Roles = roles
            };

            return i;
        }
    }
}