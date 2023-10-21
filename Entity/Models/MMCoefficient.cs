namespace Entity.Models
{
    /// <summary>
    /// Справочник коэффициентов эмпирических ММ
    /// </summary>
    public class MMCoefficient
    {
        /// <summary>
        /// Код коэффициента ММ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Коэффициент
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Значение коэффициента
        /// </summary>
        public double Value { get; set; }

        public virtual MM MM { get; set; } = null!;
    }
}
