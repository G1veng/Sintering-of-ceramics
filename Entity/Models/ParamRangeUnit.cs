namespace Entity.Models
{
    public class ParamRangeUnit
    {
        public int Id { get; set; }
        public string Alias { get; set; } = null!;

        public virtual List<ParamRange>? ParamRanges { get; set; }
    }
}
