using System.ComponentModel.DataAnnotations;

namespace ProjectTrade.Models
{
    public class SupplierDetail
    {
        [Key]
        public int Id { get; set; }
        public string supplierName { get; set; }
        public string Email { get; set; }
    }

}
