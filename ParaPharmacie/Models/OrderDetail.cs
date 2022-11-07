using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParaPharmacie.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int Qty { get; set; }
        
        [Required]
        public System.DateTime OrderDate { get; set; }
        

        [Required]
        public int ProId { get; set; }
        [ForeignKey("ProId")]
        public virtual Product Product { get; set; }

        public string? ProName { get; set; }
        
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string? UserName { get; set; }
        

        public string? Name { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}
