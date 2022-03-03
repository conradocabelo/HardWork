namespace HeavyWork.Entities
{
    public class TestConfiguration
    {
        public int NumberThreads { get; set; }
        public int NumberRequests { get; set; }
        public string PathOutReport { get; set; }

        public TypePersistent TypePersistent { get; set; }
        public string ConnectionInfo { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
