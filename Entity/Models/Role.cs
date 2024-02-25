using Microsoft.EntityFrameworkCore;

namespace Entity.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Alias { get; set; } = null!;

        public virtual List<User>? Users { get; set; }
    }
}
