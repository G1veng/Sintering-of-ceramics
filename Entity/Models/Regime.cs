namespace Entity.Models
{
    /// <summary>
    /// Справочник технологических режимов печей
    /// </summary>
    public class Regime
    {
        /// <summary>
        /// Код режима
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Минимальное время спекания
        /// </summary>
        public double MinSinteringTime { get; set; }

        /// <summary>
        /// Максимальное время спекания
        /// </summary>
        public double MaxSinteringTime { get; set; }

        /// <summary>
        /// Температура спекания минимальная
        /// </summary>
        public double MinFinalTempretare { get; set; }

        /// <summary>
        /// Температура спекания максимальная
        /// </summary>
        public double MaxFinalTempretare { get; set; }

        /// <summary>
        /// Время выдержки минимальное
        /// </summary>
        public double MinCuringTime { get; set; }

        /// <summary>
        /// Время выдержки максимальное
        /// </summary>
        public double MaxCuringTime { get; set; }

        /// <summary>
        /// Избыточное давление газа минимальное
        /// </summary>
        public double MinGasPressure {  get; set; }

        /// <summary>
        /// Избыточное давление газа максимальное
        /// </summary>
        public double MaxGasPressure { get; set; }

        /// <summary>
        /// Код справочника оборудования
        /// </summary>
        public int EquipmentId { get; set; }

        public virtual Equipment Equipment { get; set; } = null!;
    }
}
