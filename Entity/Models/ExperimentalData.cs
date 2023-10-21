namespace Entity.Models
{
    /// <summary>
    /// Справочник эксперементальных данных
    /// </summary>
    public class ExperimentalData
    {
        /// <summary>
        /// Код экспериментальных данных
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Управляющее воздействие 1
        /// </summary>
        public int FirstControlActionId { get; set; }

        /// <summary>
        /// Управляющее воздействие 2
        /// </summary>
        public int SecondControlActionId { get; set; }

        /// <summary>
        /// Управляющее воздействие 3
        /// </summary>
        public int ThirdControlActionId { get; set; }
        public double FirstActionValue { get; set; }

        /// <summary>
        /// Значение управляющего воздействия 1
        /// </summary>
        public double SecondActionValue { get; set; }

        /// <summary>
        /// Значение управляющего воздействия 2
        /// </summary>
        public double ThirdActionValue { get; set; }

        /// <summary>
        /// Значение управляющего воздействия 3
        /// </summary>
        public double QualitiesValue { get; set; }

        public virtual ControlAction FirstControlAction { get; set; } = null!;
        public virtual ControlAction SecondControlAction { get; set; } = null!;
        public virtual ControlAction ThirdControlAction { get; set; } = null!;
        public virtual Qualities Qualities { get; set; } = null!;
    }
}
