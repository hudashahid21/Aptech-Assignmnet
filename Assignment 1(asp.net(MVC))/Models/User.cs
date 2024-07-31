using System.ComponentModel;

namespace Assignment_1_asp.net_MVC__.Models
{
    public partial class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        [DefaultValue(2)]
        public int RoleId { get; set; }
        [DefaultValue(0)]
        public int Status { get; set; }
    }
}
