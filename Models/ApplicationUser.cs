//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;

//namespace FlashcardApp.Models
//{
//    public class ApplicationUser : IdentityUser
//    {
//        [Required]
//        [StringLength(100)]
//        public string FirstName { get; set; } = string.Empty;

//        [Required]
//        [StringLength(100)]
//        public string LastName { get; set; } = string.Empty;

//        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

//        public DateTime LastLoginDate { get; set; }

//        // Navigation properties
//        public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
//    }


//}

using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FlashcardApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? ProfilePicture { get; set; }

        // Navigation properties
        public virtual ICollection<Flashcard>? Flashcards { get; set; }
        public virtual ICollection<Category>? Categories { get; set; } 
    }
}