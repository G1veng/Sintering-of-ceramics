using System.ComponentModel;

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
        [Description("Предэкспоненциальный множитель для коэффициентов зернограничной диффузии")]
        public double PreExponentialFactorOfGraindBoundaryDiffusionCoefficient { get; set; }

        /// <summary>
        /// Предэкспоненциальный множитель для коэффициентов поверхностной самодиффузии
        /// </summary>
        [Description("Предэкспоненциальный множитель для коэффициентов поверхностной самодиффузии")]
        public double PreExponentialFactorOfSurfaceSelfCoefficient { get; set; }

        /// <summary>
        /// Энергии активации роста зерен материала
        /// </summary>
        [Description("Энергии активации роста зерен материала")]
        public double GrainBoundaryDiffusionActivationEnergy { get; set; }

        /// <summary>
        /// Энергии активации уплотнения материала
        /// </summary>
        [Description("Энергии активации уплотнения материала")]
        public double SurfaceSelfDiffusionActivationEnergy { get; set; }

        /// <summary>
        /// Код материала
        /// </summary>
        [Description("Материалы")]
        public int MaterialId { get; set; }

        public virtual Material Material { get; set; } = null!;
    }
}
