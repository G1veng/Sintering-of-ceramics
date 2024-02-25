namespace Entity.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int EmergencySituationId { get; set; }
        public int ScriptId { get; set; }
        public int RegimeId { get; set; }
        public int QualityId { get; set; }
        public int OvenTypeId { get; set; }
        public int MaterialId { get; set; }

        public virtual Equipment OvenType { get; set; } = null!;
        public virtual Qualities Quality { get; set; } = null!;
        public virtual Regime Regime { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
        public virtual List<EmergencySituation> EmergencySituation { get; set; } = null!;
        public virtual Script? Script { get; set; }
    }
}
