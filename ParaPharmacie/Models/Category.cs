using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParaPharmacie.Models
{
    public class Category
    {
        [Key]
        public int CatId { get; set; }
        [Required(ErrorMessage ="Ce Champ est obligatoire")]
        public string? CatName { get; set; }
        public string? CatPhoto { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
