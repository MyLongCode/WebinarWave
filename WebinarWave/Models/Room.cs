namespace WebinarWave.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Message[] Messages { get; set; }
    }
}
