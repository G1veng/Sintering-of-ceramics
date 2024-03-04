using Entity;
using Entity.Models;
using ScottPlot.Drawing.Colormaps;
using Sintering_of_ceramics.Enums;
using Sintering_of_ceramics.Helpers;
using Sintering_of_ceramics.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sintering_of_ceramics.Windows
{
    /// <summary>
    /// Interaction logic for CreateEditEmpiricalModel.xaml
    /// </summary>
    public partial class CreateEditEmpiricalModel : Window
    {
        private readonly Context _context;

        private EmpiricalModel? _empiricalModel;

        public EmpiricalModel EmpiricalModel { get => _empiricalModel ?? new EmpiricalModel(); set { _empiricalModel = value; } }


        public CreateEditEmpiricalModel(Context context)
        {
            InitializeComponent();

            _context = context;
            _empiricalModel = null;
        }

        public void InitializeWindow(string actionName, WindowActionTypeEnum actionType, int? modelId)
        {
            this.Title = actionName;
            actionButton.Content = actionType.GetAttributeOfType<DescriptionAttribute>()?.Description ?? string.Empty;

            if (modelId.HasValue)
            {
                _empiricalModel = _context.EmpiricalModels.FirstOrDefault(x => x.Id == modelId);
            }
            else
            {
                _empiricalModel = new EmpiricalModel();
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
