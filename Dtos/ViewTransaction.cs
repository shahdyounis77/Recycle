using Recycle.Data.Models;

namespace Recycle.Dtos
{
    public class ViewTransaction
    {
        public string ItemType { get; set; }

        public double Weight { get; set; }

        public int CoinsEarned { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
