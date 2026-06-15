using System.ComponentModel.DataAnnotations.Schema;

namespace Recycle.Data.Models
{
    
    public class Transaction
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        
        
       
        public User User { get; set; }
        
        [ForeignKey("Machine")]
        public int MachineId { get; set; }

        public Machine Machine { get; set; }

        public TypeOfItem ItemType { get; set; }

        public double Weight { get; set; }

        public int CoinsEarned { get; set; }

        public DateTime CreatedAt { get; set; }
    }

   public enum TypeOfItem
    {
        Plastic,
       Aluminum,
        
    }
}
