using Recycle.Data.Models;

namespace Recycle.Dtos
{
    public class AddTransaction
    {
        public int OtpId { get; set; }
       

        public string ItemType { get; set; }

        public double Weight { get; set; }

        public int CoinsEarned { get; set; }

       
    }
}
