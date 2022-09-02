using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ParaPharmacie.Models
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        [Required(ErrorMessage ="Ce Champ est obligatoire")]
        public string? CatName { get; set; }
        public string? CatPhoto { get; set; }

        public IFormFile File { get; set; }
    }
}
