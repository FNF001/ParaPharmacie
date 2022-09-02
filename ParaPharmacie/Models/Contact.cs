using System.ComponentModel.DataAnnotations;

namespace ParaPharmacie.Models
{
    public class Contact
    {
        [Key]
        public int CoId { get; set; }

        public string? Name { get; set; }
        [Required(ErrorMessage ="Ce champ est obligatoire")]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Subject { get; set; }
        public string? Message { get; set; }


    }
}
