namespace WebATB.Areas.Admin.Models
{
    public class UserItemVm
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public List<string> Roles { get; set; } = [];
        public string? Image { get; set; }
        public string FullName { get; set; } = "";
        public DateTimeOffset? LockoutEnd { get; set; } = null;
        public bool IsGoogleLogin { get; set; } = false;
    }
}
