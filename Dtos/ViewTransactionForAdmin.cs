namespace Recycle.Dtos
{
    public class ViewTransactionForAdmin
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MachineLocation { get; set; }
        public string ItemType { get; set; }
        public double Weight { get; set; }
        public double CoinsEarned { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
