using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace Recycle.Controllers

{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly UserManager<Data.Models.User> _userManager;

        public UpdateController(UserManager<Data.Models.User> userManager)
        {this._userManager = userManager;
            
        }
        [HttpPut("UserName")]
        public async Task<IActionResult> UpdateUserName(Data.Dto.UpdateUserName userName)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user =await _userManager.FindByIdAsync(userid);
            if (user == null) {
                return NotFound();
            }

            else
            {
                user.UserName = userName.UserName;
            var result=  await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { message = "updated username succefully" });
                }

                else
                {
                    return BadRequest(result.Errors);
                }

            }
            
               
        }

        [HttpPut("UserPassword")]
        public async Task<IActionResult> UpdateUserPaswword(Data.Dto.UpdateUserPassword userpwd)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound();
            }

            else
            {

                var result = await _userManager.ChangePasswordAsync(user,userpwd.CurrentPassword,userpwd.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new { message = "updated password succefully" });
                }

                else
                {
                    return BadRequest(result.Errors);
                }

            }


        }
    }
}
