namespace Services.DTO
{
    public class BookDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }

        public int Year { get; set; }

        public byte[] Image { get; set; }
        public byte[] FileBook { get; set; }
        public decimal Rate { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public uint RatesAmount { get; set; }
    }
}
