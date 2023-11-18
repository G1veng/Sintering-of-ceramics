using System.Collections.Generic;

namespace Sintering_of_ceramics.Models
{
    public class ModelParamDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? SValue { get; set; }
        public double? DValue { get; set; }
        public bool? BValue { get; set; }
        public List<object>? LValues { get; set; }
        public string? DisplayMemberPath { get; set; }
        public int? SelectedIndex { get; set; }
    }
}
