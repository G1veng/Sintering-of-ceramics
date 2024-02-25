namespace Entity.Models
{
    public class EmergencySituation
    {
        public int Id { get; set; }
        public string Reason { get; set; } = null!;
        public string Action { get; set; } = null!;

        public virtual List<Task>? Tasks { get; set; }
    }
}
