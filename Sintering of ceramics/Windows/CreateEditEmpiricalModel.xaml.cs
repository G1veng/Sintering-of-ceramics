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

        private int? _modelId;
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
            _modelId = modelId;
            this.Title = actionName;
            actionButton.Content = actionType.GetAttributeOfType<DescriptionAttribute>()?.Description ?? string.Empty;
            var modelTypes = _context.EmpiricalModelTypes.AsNoTracking().ToList();
            var materials = _context.Materials.AsNoTracking().ToList();
            var equipments = _context.Equipments.AsNoTracking().ToList();

            if (modelId.HasValue)
            {
                var model = _context.EmpiricalModels
                    .AsNoTracking()
                    .Include(x => x.ParamsRanges).ThenInclude(x => x.Unit)
                    .Include(x => x.EmpiricalModelCoeffs)
                    .First(x => x.Id == modelId);

                model.ParamsRanges.ForEach(p =>
                {
                    p.Alias = p.Unit.LetterAlias;
                    p.CoefficientAlias = p.Unit.Alias;
                });

                _empiricalModel = model;
                EmpiricalModelTypeComboBox.ItemsSource = modelTypes;
                EmpiricalModelTypeComboBox.SelectedIndex = modelTypes.FindIndex(0, m => m.Id == model.TypeId);
                MaterialsComboBox.ItemsSource = materials;
                MaterialsComboBox.SelectedIndex = materials.FindIndex(0, m => m.Id == model.MaterialId);
                OvensComboBox.ItemsSource = equipments;
                OvensComboBox.SelectedIndex = equipments.FindIndex(0, o => o.Id == model.EquipmentId);
                ParamRangesDataGrid.ItemsSource = model.ParamsRanges;
                CoefficientsDataGrid.ItemsSource = model.EmpiricalModelCoeffs;
                Formula.Text = model.Formula;
            }
            else
            {
                EmpiricalModelTypeComboBox.ItemsSource = modelTypes;
                EmpiricalModelTypeComboBox.SelectedIndex = 0;
                MaterialsComboBox.ItemsSource = materials;
                MaterialsComboBox.SelectedIndex = 0;
                OvensComboBox.ItemsSource = equipments;
                OvensComboBox.SelectedIndex = 0;
                ParamRangesDataGrid.ItemsSource = _paramRanges;
                CoefficientsDataGrid.ItemsSource = _empiricalModelCoeffs;
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
                        Alias = letter,
                    });
                }
            }

            ParamRangesDataGrid.Items.Refresh();
            CoefficientsDataGrid.Items.Refresh();
        }

        private void ActionButtonPress(object sender, RoutedEventArgs e)
        {
            if (!_modelId.HasValue)
            {
                var typeId = ((EmpiricalModelType)EmpiricalModelTypeComboBox.Items[EmpiricalModelTypeComboBox.SelectedIndex]).Id;
                var materialId = ((Material)MaterialsComboBox.Items[MaterialsComboBox.SelectedIndex]).Id;

                if (_context.EmpiricalModels.AsNoTracking()
                    .FirstOrDefault(m => m.TypeId == typeId && m.MaterialId == materialId) != null)
                {
                    MessageBox.Show("Эмпирическая модель данного типа уже сущеуствует для данного материала, пожалуйста, выберите другой тип или материал",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);

                    return;
                }

                var res = _context.EmpiricalModels.Add(new EmpiricalModel()
                {
                    EmpiricalModelCoeffs = _empiricalModelCoeffs,
                    TypeId = typeId,
                    MaterialId = materialId,
                    Formula = Formula.Text,
                    EquipmentId = ((Equipment)OvensComboBox.Items[OvensComboBox.SelectedIndex]).Id,
                    ParamsRanges = _paramRanges
                });

                var r = res.Entity;
            }
            else
            {
                var model = _context.EmpiricalModels.First(x => x.Id == _modelId);

                model.MaterialId = ((Material)MaterialsComboBox.Items[MaterialsComboBox.SelectedIndex]).Id;
                model.Formula = Formula.Text;
                model.EmpiricalModelCoeffs = _empiricalModelCoeffs;
                model.TypeId = ((EmpiricalModelType)EmpiricalModelTypeComboBox.Items[EmpiricalModelTypeComboBox.SelectedIndex]).Id;
                model.EquipmentId = ((Equipment)OvensComboBox.Items[OvensComboBox.SelectedIndex]).Id;
                model.ParamsRanges = _paramRanges;
            }

            _context.SaveChanges();

            _paramRanges.Clear();
            _empiricalModelCoeffs.Clear();
            this.Hide();
        }
    }
}
