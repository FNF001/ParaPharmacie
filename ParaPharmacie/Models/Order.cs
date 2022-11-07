using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ParaPharmacie.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        
        public System.DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        [EmailAddress]
        public string? Email { get; set; }

        
        [RegularExpression(@"\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*", ErrorMessage = "expression doit être comme : +21651104614")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public string? City { get; set; }

        public string? State { get; set; }

        public int? ZIPCode { get; set; }

        [Required]
        public decimal? Total { get; set; }

        [NotMapped]
        public List<ShoppingCart>? OrderItems { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
