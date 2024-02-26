namespace Entity.Models
{
    public class Script
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public string Protocol { get; set; } = null!;
        public int TaskId { get; set; }
        public int InstructorId { get; set; }
        public int TraineeId { get; set; }

        public virtual User? Trainee { get; set; }
        public virtual User? Instructor { get; set; }
        public virtual Task Task { get; set; } = null!;
    }
}
