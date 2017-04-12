namespace Talento.Core.Utilities
{
    public class Pagination
    {
        public int Next { get; set; }
        public int Prev { get; set; }
        public int Current { get; set; }
        public int Total { get; set; }
        public string Url { get; set; }
    }
}
