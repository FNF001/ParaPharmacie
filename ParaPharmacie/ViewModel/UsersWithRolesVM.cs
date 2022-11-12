using System.ComponentModel.DataAnnotations;

namespace ParaPharmacie.ViewModel
{
    public class UsersWithRolesVM
    {
        [Key]
        public string Id { get; set; }
        
        public string Name { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
                                      
        public string? RoleName { get; set; }
    }
}
