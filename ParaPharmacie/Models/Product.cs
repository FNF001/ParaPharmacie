using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParaPharmacie.Models
{
    public class Product
    {

        [Key]
        public int ProId { get; set; }
        [Required(ErrorMessage ="Ce champ est obligatoire")]
        public string? ProName { get; set; }
        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public decimal Price { get; set; }

        public string ProImage { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        public int CatId { get; set; }
        [ForeignKey("CatId")]

        public virtual Category Category { get; set; }

    }
}
