using System.ComponentModel;

namespace Entity.Models
{
    public class User
    {
        public int Id { get; set; }

        [Description("Логин")]
        public string Login { get; set; } = null!;

        [Description("Пароль")]
        public string Password { get; set; } = null!;

        [Description("Администратор")]
        public bool IsAdmin { get; set; }
    }
}
