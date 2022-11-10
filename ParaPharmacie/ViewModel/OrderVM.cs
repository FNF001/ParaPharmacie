using ParaPharmacie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParaPharmacie.ViewModel
{
    public class OrderVM
    {

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

        public string? Country { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire")]
        public string? City { get; set; }

        public string? State { get; set; }

        public int? ZIPCode { get; set; }

        public decimal? Total { get; set; }

    }
}
