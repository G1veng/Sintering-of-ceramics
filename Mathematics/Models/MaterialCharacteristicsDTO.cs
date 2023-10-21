namespace Mathematics.Models
{
    public class MaterialCharacteristicsDTO
    {
        /// <summary>
        /// Конечная пористость, %
        /// </summary>
        public double PP {  get; set; }

        /// <summary>
        /// Конечный средний размер зерна, мкм
        /// </summary>
        public double LL { get; set; }

        /// <summary>
        /// Конечная вязкость материала, МПа*с
        /// </summary>
        public double Ett { get; set; }

        /// <summary>
        /// Максимальная конечная пористость (Не точно)
        /// </summary>
        public double PPP { get; set; }

        /// <summary>
        /// Конечная плотность материала,  кг/м^3
        /// </summary>
        public double Ro { get; set; }
    }
}
