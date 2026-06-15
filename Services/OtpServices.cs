using Recycle.Dtos;
using Recycle.Interfaces;
using System.Security.Cryptography;
using Recycle.Data.Models;

namespace Recycle.Services
{
    public class OtpServices
    {
        private readonly IOtp _otpRepo;
        public OtpServices(IOtp otpRepo)
        {
            _otpRepo = otpRepo;
        }
        public string GenerateOtp(string userid, int machineid)
        {

            string otp;
            do
            {
                otp = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            }
            while (_otpRepo.GetAllOtps().Any(o => o.Code == otp && o.Status == StatusOtp.Active && o.IsUsed == false));

            var otpobj = new AddOtp
            {
                Code = otp,
                MachineId = machineid,
                IsUsed = false,
                ExpireAt = DateTime.UtcNow.AddMinutes(5),
                Status = StatusOtp.Active,


            };
            _otpRepo.AddOtp(otpobj, userid);
            return otp;
        }

        public ViewOtp VerfiyOtp(string otp)
        {
            var existingotp = _otpRepo.GetAllOtps().FirstOrDefault(o => o.Code == otp && o.Status == StatusOtp.Active && o.IsUsed == false);
            if (existingotp != null && existingotp.ExpireAt > DateTime.UtcNow)
            {


                _otpRepo.updateOtpIsUsed(existingotp.Id, true);

                return new ViewOtp
                {


                    Id = existingotp.Id,
                    MachineId = existingotp.MachineId,
                    UserId = existingotp.UserId
                };

            }
            else
            {
                return null;
            }


        }

        public string CheckBeforGenerationOtp(string userid, int machineid)
        {

            var otps = _otpRepo.GetAllOtps().Where(o => o.UserId == userid && o.Status == StatusOtp.Active && o.IsUsed == false && o.ExpireAt > DateTime.UtcNow).ToList();
            if (otps.Count == 0)
            {
                return GenerateOtp(userid, machineid);
            }

            else
            {
                foreach (var item in otps)

                {
                    var status = new UpdateStatusOfOtp
                    {

                        Status = StatusOtp.Stopped
                    };
                    _otpRepo.UpdateOtpStatus(status, item.Id);

                }

                return GenerateOtp(userid, machineid);



            }

        }

        public void UpdateOtp(UpdateStatusOfOtp status, int otpId)
        {

            _otpRepo.UpdateOtpStatus(status, otpId);





        }
    }
}
