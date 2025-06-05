using Microsoft.AspNetCore.Identity;

namespace FlashcardApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<Flashcard> Flashcards { get; set; }
    }
}