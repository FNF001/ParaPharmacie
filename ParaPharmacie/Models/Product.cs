using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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

        public IFormFile File { get; set; }

    }
}
