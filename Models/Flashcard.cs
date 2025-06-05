using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashcardApp.Models
{
    public class Flashcard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string FrontSide { get; set; }

        [Required]
        [StringLength(500)]
        public string BackSide { get; set; }

        [StringLength(100)]
        public string Category { get; set; } // Eski kategori alaný (geçiþ dönemi için)

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category CategoryObj { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastReviewedDate { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}