namespace Entity.Models
{
    /// <summary>
    /// Справочник оборудования
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// Код типа печи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Тип печи
        /// </summary>
        public string EquipmentType { get; set; } = null!;

        /// <summary>
        /// Марка
        /// </summary>
        public string EquipmentBrand { get; set; } = null!;

        /// <summary>
        /// Производитель
        /// </summary>
        public string Manufacturer { get; set; } = null!;

        /// <summary>
        /// Масса садки (загрузка)
        /// </summary>
        public double ChargeWightLoad {  get; set; }

        /// <summary>
        /// Тип нагревателей
        /// </summary>
        public int HeaterType { get; set; }

        public virtual Regime Regime { get; set; } = null!;
        public virtual List<Model> Models { get; set; } = null!;
    }
}
