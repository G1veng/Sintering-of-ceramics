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
        [Description("Тип материала")]
        public int MaterialType { get; set; }

        /// <summary>
        /// Начальная пористость, %
        /// </summary>
        [Description("Начальная пористость, %")]
        public double Porosity { get; set; }

        /// <summary>
        /// Начальный средний размер зерна, мкм
        /// </summary>
        [Description("Начальный средний размер зерна, мкм")]
        public double AvarageGrainSize { get; set; }

        /// <summary>
        /// Толщина поверхностного слоя, нм
        /// </summary>
        [Description("Толщина поверхностного слоя, нм")]
        public double SurfaceLayerThickness { get; set; }

        /// <summary>
        /// Удельная поверхностная энергия, Дж/м^2
        /// </summary>
        [Description("Удельная поверхностная энергия, Дж/м^2")]
        public double SpecificSurfaceEnergy { get; set; }

        /// <summary>
        /// Плотность компактного материала, кг/м^3
        /// </summary>
        [Description("Плотность компактного материала, кг/м^3")]
        public double CompactMaterialDensity { get; set; }

        /// <summary>
        /// Вязкость компактного материала, МПа*с
        /// </summary>
        [Description("Вязкость компактного материала, МПа*с")]
        public double CompactMaterialViscosity { get; set; }

        /// <summary>
        /// Масса материала, кг
        /// </summary>
        [Description("Масса материала, кг")]
        public double Weight {  get; set; }

        /// <summary>
        /// Название материала
        /// </summary>
        [Description("Название материала")]
        public string Name { get; set; } = null!;
    

        public virtual TheoreticalMMParams TheoreticalMMParam { get; set; } = null!;
        public virtual List<Model> Models { get; set; } = null!;
    }
}
