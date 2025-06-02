using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashcardApp.Models
{
    // Flashcard model sınıfı, veritabanındaki Flashcards tablosunu temsil eder
    public class Flashcard
    {
        [Key] // Primary key olduğunu belirtiyoruz
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ID'nin otomatik artan bir değer olmasını sağlıyoruz
        public int Id { get; set; }

        [Required(ErrorMessage = "Front side is required.")] // FrontSide alanının zorunlu olduğunu belirtiyoruz
        [StringLength(500, ErrorMessage = "Front side cannot be longer than 500 characters.")] // Maksimum uzunluk belirliyoruz
        public string FrontSide { get; set; }

        [Required(ErrorMessage = "Back side is required.")] // BackSide alanının zorunlu olduğunu belirtiyoruz
        [StringLength(500, ErrorMessage = "Back side cannot be longer than 500 characters.")] // Maksimum uzunluk belirliyoruz
        public string BackSide { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot be longer than 100 characters.")] // Maksimum uzunluk belirliyoruz
        public string? Category { get; set; } // Nullable olarak işaretliyoruz

        public DateTime CreatedDate { get; set; } = DateTime.Now; // Oluşturulma tarihi, varsayılan olarak şimdiki zaman
        public DateTime? LastReviewedDate { get; set; } // Son gözden geçirme tarihi, nullable
    }
}