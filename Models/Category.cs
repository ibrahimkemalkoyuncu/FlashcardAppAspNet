//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace FlashcardApp.Models
//{
//    public class Category
//    {
//        public int Id { get; set; }

//        [Required]
//        [StringLength(100)]
//        public string Name { get; set; }

//        [StringLength(500)]
//        public string? Description { get; set; }

//        [Required]
//        public string? UserId { get; set; }

//        [ForeignKey("UserId")]
//        public virtual ApplicationUser? User { get; set; }

//        // Navigation property
//        public virtual ICollection<Flashcard> Flashcards { get; set; }
//    }

//}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlashcardApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Flashcard>? Flashcards { get; set; }

        public override string ToString() => Name;
    }
}