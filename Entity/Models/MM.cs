namespace Entity.Models
{
    /// <summary>
    /// Справочник эмпирических математических моделей
    /// </summary>
    public class MM
    {
        /// <summary>
        /// Код ММ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Критерий Фишера
        /// </summary>
        public double Fisher { get; set; }

        public virtual List<MMCoefficient> MMCoefficients { get; set; } = null!;
        public virtual List<Model> Models { get; set; } = null!;
    }
}
