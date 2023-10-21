namespace Entity.Models
{
    /// <summary>
    /// Справочник настраиваемых параметров теоретической математической модели
    /// </summary>
    public class TheoreticalMMParams
    {
        /// <summary>
        /// Код теоретичесой математической модели
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Предэкспоненциальный множитель для коэффициентов зернограничной диффузии
        /// </summary>
        public double PreExponentialFactorOfGraindBoundaryDiffusionCoefficient { get; set; }

        /// <summary>
        /// Предэкспоненциальный множитель для коэффициентов поверхностной самодиффузии
        /// </summary>
        public double PreExponentialFactorOfSurfaceSelfCoefficient { get; set; }

        /// <summary>
        /// Энергии активации роста зерен материала
        /// </summary>
        public double GrainBoundaryDiffusionActivationEnergy { get; set; }

        /// <summary>
        /// Энергии активации уплотнения материала
        /// </summary>
        public double SurfaceSelfDiffusionActivationEnergy { get; set; }

        /// <summary>
        /// Код материала
        /// </summary>
        public int MaterialId { get; set; }

        public virtual Material Material { get; set; } = null!;
    }
}
