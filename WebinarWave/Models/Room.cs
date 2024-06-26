namespace WebinarWave.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
        public bool IsPrivate { get; set; }
        public string? Password { get; set; }
    }
}
