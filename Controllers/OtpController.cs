using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Recycle.Services;
using System.Security.Claims;
namespace Recycle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OtpController : ControllerBase
    {
        private readonly OtpServices _otpServices;
        public OtpController(OtpServices otpServices)
        {
            _otpServices = otpServices;
        }
        [HttpGet("Generate/{MachineId}")]
        public IActionResult GenerateOtp(int MachineId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var otp = _otpServices.CheckBeforGenerationOtp(userId,MachineId);

            if (otp != null)
            {
                return Ok(new { Otp = otp, Message = "OTP already generated and is still valid for 5 minutes." });
            }
            else
            {
                return BadRequest(new { Message = "Try Again" });

            }
        }

        [HttpGet("Verify")]
        public IActionResult VerifyOtp(string otp)
        {
            var result = _otpServices.VerfiyOtp(otp);
            if (result != null)
            {
                return Ok(new { Message = "OTP verified successfully", OtpId = result.Id });
            }
            else
            {
                return BadRequest(new { Message = "Invalid or expired OTP" });
            }
        }
    }
}
