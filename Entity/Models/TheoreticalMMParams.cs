using System.ComponentModel;

namespace Entity.Models
{
    /// <summary>
    /// Справочник настраиваемых параметров теоретической математической модели
    /// </summary>
    public class TheoreticalMMParams
    {
        public int Id { get; set; }

        [Description("Предэкспоненциальный множитель для коэффициентов зернограничной диффузии")]
        public double PreExponentialFactorOfGraindBoundaryDiffusionCoefficient { get; set; }

        [Description("Предэкспоненциальный множитель для коэффициентов поверхностной самодиффузии")]
        public double PreExponentialFactorOfSurfaceSelfCoefficient { get; set; }

        [Description("Энергии активации роста зерен материала")]
        public double GrainBoundaryDiffusionActivationEnergy { get; set; }

        [Description("Энергии активации уплотнения материала")]
        public double SurfaceSelfDiffusionActivationEnergy { get; set; }

        [Description("Материалы")]
        public int MaterialId { get; set; }

        public virtual Material Material { get; set; } = null!;
    }
}
