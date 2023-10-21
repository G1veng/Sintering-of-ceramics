namespace Entity.Models
{
    /// <summary>
    /// Справочник материалов
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Код модели
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код математической модели
        /// </summary>
        public int MMId { get; set; }

        /// <summary>
        /// Код материала
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Код оборудования
        /// </summary>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Код показателя качества
        /// </summary>
        public int QualityId { get; set; }

        public virtual Equipment Equipment { get; set; } = null!;
        public virtual MM MM { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
        public virtual Qualities Qualities { get; set; } = null!;
    }
}
