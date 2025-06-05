using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlashcardApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Navigation property
        public ICollection<Flashcard> Flashcards { get; set; }
    }
}