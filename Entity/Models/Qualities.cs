namespace Entity.Models
{
    /// <summary>
    /// Справочник показателей качества
    /// </summary>
    public class Qualities
    {
        /// <summary>
        /// Код показателя качества
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Единица измерения показателя качества
        /// </summary>
        public double Unit { get; set; }

        public virtual List<ExperimentalData> ExperimentalDatas { get; set; } = null!;
        public virtual List<Model> Models { get; set; } = null!;
    }
}
