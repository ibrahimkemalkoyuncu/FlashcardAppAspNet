
using System.ComponentModel.DataAnnotations;

namespace FlashcardApp.Models
{
    public class FlashcardCreateViewModel
    {
        [Required(ErrorMessage = "Ön yüz içeriği zorunludur")]
        [StringLength(500, ErrorMessage = "Ön yüz içeriği maksimum 500 karakter olabilir")]
        [Display(Name = "Ön Yüz")]
        public string FrontSide { get; set; }

        [Required(ErrorMessage = "Arka yüz içeriği zorunludur")]
        [StringLength(500, ErrorMessage = "Arka yüz içeriği maksimum 500 karakter olabilir")]
        [Display(Name = "Arka Yüz")]
        public string BackSide { get; set; }

        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }
    }
}