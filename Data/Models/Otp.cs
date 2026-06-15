using System.ComponentModel.DataAnnotations.Schema;

namespace Recycle.Data.Models
{
    public class Otp
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Machine")]
        public int MachineId { get; set; }
        public Machine Machine { get; set; }

        public User User { get; set; }
        public string Code { get; set; }

        public DateTime ExpireAt { get; set; }

        public bool IsUsed { get; set; }

        public StatusOtp Status { get; set; }

        

    }

    public enum StatusOtp {
     
         Active,
          Expired,
          Stopped,
          Finished


    }
}
