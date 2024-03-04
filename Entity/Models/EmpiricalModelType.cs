namespace Entity.Models
{
    public class EmpiricalModelType
    {
        public int Id { get; set; }
        public string Alias { get; set; } = null!;
        public string UnitAlias { get; set; } = null!;

        public virtual List<EmpiricalModel>? EmpiricalModels { get; set;}
    }
}
