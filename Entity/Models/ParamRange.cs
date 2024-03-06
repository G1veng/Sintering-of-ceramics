using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Models
{
    public class ParamRange
    {
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public int UnitId { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double Step { get; set; }

        public virtual EmpiricalModel? Model { get; set; }
        public virtual ParamRangeUnit Unit { get; set; } = null!;

        #region NotMapped
        [NotMapped]
        public string CoefficientAlias { get; set; } = null!;
        [NotMapped]
        public string Alias { get; set; } = null!;
        #endregion
    }
}
