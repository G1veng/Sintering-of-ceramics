using Entity;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
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
        private List<ParamRange> _paramRanges;
        private List<EmpiricalModelCoeff> _empiricalModelCoeffs;

        public EmpiricalModel EmpiricalModel { get => _empiricalModel ?? new EmpiricalModel(); set { _empiricalModel = value; } }


        public CreateEditEmpiricalModel(Context context)
        {
            InitializeComponent();

            _context = context;
            _empiricalModel = null;
            _paramRanges = new();
            _empiricalModelCoeffs = new();
        }

        public void InitializeWindow(string actionName, WindowActionTypeEnum actionType, int? modelId)
        {
            this.Title = actionName;
            actionButton.Content = actionType.GetAttributeOfType<DescriptionAttribute>()?.Description ?? string.Empty;
            EmpiricalModelTypeComboBox.ItemsSource = _context.EmpiricalModelTypes.AsNoTracking().ToList();
            EmpiricalModelTypeComboBox.SelectedIndex = 0;
            MaterialsComboBox.ItemsSource = _context.Materials.AsNoTracking().ToList();
            MaterialsComboBox.SelectedIndex = 0;
            OvensComboBox.ItemsSource = _context.Equipments.AsNoTracking().ToList();
            OvensComboBox.SelectedIndex = 0;
            ParamRangesDataGrid.ItemsSource = _paramRanges;
            CoefficientsDataGrid.ItemsSource = _empiricalModelCoeffs;

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
            _paramRanges.Clear();
            _empiricalModelCoeffs.Clear();
            this.Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _paramRanges.Clear();
            _empiricalModelCoeffs.Clear();
            e.Cancel = true;
            this.Hide();
        }

        private void ParseFormula(object sender, RoutedEventArgs e)
        {
            _paramRanges.Clear();
            _empiricalModelCoeffs.Clear();

            var text = Formula.Text.Replace('*', ' ').Replace('/', ' ').Replace('+', ' ').Replace('-', ' ').Split(' ');
            var templateLetters = _context.ParamsRangesUnits.AsNoTracking()
                .ToList();

            foreach (var letter in text )
            {
                if (templateLetters.Select(x => x.LetterAlias).Contains(letter))
                {
                    _paramRanges.Add(new ParamRange
                    {
                        Alias = letter,
                        UnitId = templateLetters.First(x => x.LetterAlias == letter).Id,
                        CoefficientAlias = templateLetters.First(x => x.LetterAlias == letter).Alias
                    });
                }
                else
                {
                    _empiricalModelCoeffs.Add(new EmpiricalModelCoeff
                    {
                        Alias = letter
                    });
                }
            }

            ParamRangesDataGrid.Items.Refresh();
            CoefficientsDataGrid.Items.Refresh();
        }
    }
}
