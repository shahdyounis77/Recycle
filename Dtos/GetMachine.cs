namespace Recycle.Dtos
{
    public class GetMachine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public bool IsAvailable { get; set; }
    }
}
