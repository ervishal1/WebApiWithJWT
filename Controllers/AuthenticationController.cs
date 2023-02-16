using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiWithJWT.Authentication;
using WebApiWithJWT.Data;
using WebApiWithJWT.Models;

namespace WebApiWithJWT.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<ApplicationUser> roleManager;
		private readonly IConfiguration configuration;

		public AuthenticationController(UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationUser> roleManager,
			IConfiguration configuration)
		{
			_userManager = userManager;
			this.roleManager = roleManager;
			this.configuration = configuration;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			var userExist = await _userManager.FindByNameAsync(model.UserName);
			if(userExist != null)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Alrady Exists!" });
			}
			ApplicationUser user = new ApplicationUser()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.UserName,
			};
			var result = await _userManager.CreateAsync(user,model.Password);

			if (!result.Succeeded) {
				return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Creation Faild!" });	
			}

			return Ok(new Response { Status = "Success", Message = "User Created Successfully!" });

		}
	}
}
