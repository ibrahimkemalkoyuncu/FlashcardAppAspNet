namespace FlashcardAppAspNet.Models
{
    public class FlashcardEditViewModel
    {
        public int Id { get; set; }
        public string? FrontSide { get; set; }
        public string? BackSide { get; set; }
        public int? CategoryId { get; set; }
    }
}
