namespace Entity.Models
{
    /// <summary>
    /// Справочник управляемых воздействий
    /// </summary>
    public class ControlAction
    {
        /// <summary>
        /// Код управляющего воздействия
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Управляющее воздействие
        /// </summary>
        public double Unit { get; set; }

        public virtual List<ExperimentalData> FirstExperimentalDatas { get; set; } = null!;
        public virtual List<ExperimentalData> SecondExperimentalDatas { get; set; } = null!;
        public virtual List<ExperimentalData> ThirdExperimentalDatas { get; set; } = null!;
    }
}
