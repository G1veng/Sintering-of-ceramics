using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Entity.Models
{
    /// <summary>
    /// Справочник материалов
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Код типа материала
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Тип материала
        /// </summary>
        public int MaterialType { get; set; }

        /// <summary>
        /// Начальная пористость, %
        /// </summary>
        public double Porosity { get; set; }

        /// <summary>
        /// Начальный средний размер зерна, мкм
        /// </summary>
        public double AvarageGrainSize { get; set; }

        /// <summary>
        /// Толщина поверхностного слоя, нм
        /// </summary>
        public double SurfaceLayerThickness { get; set; }

        /// <summary>
        /// Удельная поверхностная энергия, Дж/м^2
        /// </summary>
        public double SpecificSurfaceEnergy { get; set; }

        /// <summary>
        /// Плотность компактного материала, кг/м^3
        /// </summary>
        public double CompactMaterialDensity { get; set; }

        /// <summary>
        /// Вязкость компактного материала, МПа*с
        /// </summary>
        public double CompactMaterialViscosity { get; set; }

        /// <summary>
        /// Масса материала, кг
        /// </summary>
        public double Weight {  get; set; }

        /// <summary>
        /// Название материала
        /// </summary>
        public string Name { get; set; } = null!;
    

        public virtual TheoreticalMMParams TheoreticalMMParam { get; set; } = null!;
        public virtual List<Model> Models { get; set; } = null!;
    }
}
