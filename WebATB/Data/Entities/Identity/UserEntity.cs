using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebATB.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<int>
    {
        [StringLength(100)]
        public string? FirstName { get; set; } = null;
        [StringLength(100)]
        public string? LastName { get; set; } = null;
        [StringLength(100)]
        public string? Image { get; set; } = null;

        public ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
