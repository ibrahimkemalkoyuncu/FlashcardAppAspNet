////using System.ComponentModel.DataAnnotations;
////using System.ComponentModel.DataAnnotations.Schema;

////namespace FlashcardApp.Models
////{
////    public class Flashcard
////    {
////        [Key]
////        public int Id { get; set; }

////        [Required]
////        [StringLength(500)]
////        public string? FrontSide { get; set; }

////        [Required]
////        [StringLength(500)]
////        public string? BackSide { get; set; }

////        public int? CategoryId { get; set; }

////        [ForeignKey("CategoryId")]
////        public virtual Category? Category { get; set; }

////        public DateTime CreatedDate { get; set; }
////        public DateTime? LastReviewedDate { get; set; }

////        public string? UserId { get; set; }
////        public ApplicationUser? User { get; set; }
////    }
////}

//using System.ComponentModel.DataAnnotations;

//namespace FlashcardApp.Models
//{
//    public class Flashcard
//    {
//        public int Id { get; set; }

//        [Required]
//        [MaxLength(500)]
//        public string FrontSide { get; set; }

//        [Required]
//        [MaxLength(500)]
//        public string BackSide { get; set; }

//        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
//        public DateTime? LastReviewedDate { get; set; }

//        // Kategori ili�kisi
//        public int? CategoryId { get; set; }
//        public virtual Category Category { get; set; }

//        // Kullan�c� ili�kisi
//        public string UserId { get; set; }
//        public virtual ApplicationUser User { get; set; }
//    }
//}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashcardApp.Models
{
    public class Flashcard
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string FrontSide { get; set; }

        [Required]
        [MaxLength(500)]
        public string BackSide { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastReviewedDate { get; set; }

        [Range(1, 5)]
        public int DifficultyLevel { get; set; } = 3; // 1-5 aras� zorluk seviyesi

        // Kategori ili�kisi
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        // Kullan�c� ili�kisi
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}