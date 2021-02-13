namespace SharpsenStreamBackend.Classes.Dto
{
    public class StreamDto
    {
        public int StreamId { get; set; }
        public int OwnerId { get; set; }
        public string StreamName { get; set; }
        public bool Live { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
