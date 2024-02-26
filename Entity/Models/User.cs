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
        public int? RoleId { get; set; }

        [Description("Роль")]
        public virtual Role? Role { get; set; }
        public virtual List<Script>? ScriptsTrainee { get; set; }
        public virtual List<Script>? ScriptsInstructor { get; set; }
    }
}
