using System.ComponentModel.DataAnnotations.Schema;

namespace ParaPharmacie.Models
{
    public class ShoppingCart
    {
        public int CartId { get; set; }
        public int ProId { get; set; }
        [ForeignKey("ProId")]
        public virtual Product Product { get; set; }

        public decimal Price { get; set; }

        public int Qty { get; set; }
    }
}
