using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Entity;
using Entity.Models;
using Mathematics;
using Mathematics.Models;
using Microsoft.EntityFrameworkCore;
using ScottPlot;
using ScottPlot.Plottable;
using Sintering_of_ceramics.Enums;
using Sintering_of_ceramics.Models;
using Sintering_of_ceramics.Windows;

namespace Sintering_of_ceramics
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Private properties

        private readonly Context _context;
        private readonly EditDataBaseWindow _editDataBaseWindow;
        private readonly InstuctorWindow _instuctorWindow;
        private readonly InformationWindow _informationWindow;

        private ObservableCollection<Material> _materialsList;
        private Material _selectedMaterial;
        private ObservableCollection<Equipment> _equipmentList;
        private Equipment _selectedEquipment;
        private bool _isothermalSinteringStageDisabled = false;
        private double _initialTemperatureInFurnace = 20;
        private double _finalTemperatureInFurnace = 1450;
        private double _sinteringTime = 70;
        private double _excerptTime = 30;
        private double _pressure = 6;
        private bool _isAdmin = true;
        private bool _isInstructor = true;
        private int _stepsAmount;
        private double _epsilon;
        private int _maxStepDivision;

        private double _resultPorosity = 0;
        private double _resultAvarageGrainSize = 0;
        private double _resultDensity = 0;
        private double _resultViscosity = 0;
        private double? _bendingStrengthE = null;
        private double? _hardnessE = null;
        private double? _porosityE = null;
        private double? _densityE = null;

        private ObservableCollection<ChartTable> _table = new ObservableCollection<ChartTable>();
        private Crosshair? _crosshair;
        private Dictionary<WpfPlot, ScatterPlot> _charts;

        List<object> _isInvalidElements = new List<object>();

        #endregion

        #region Properties

        public ObservableCollection<Material> MaterialsList
        {
            get { return _materialsList; }
            set 
            {
                _materialsList = value;
                NotifyPropertyChanged(nameof(MaterialsList)); 
            }
        }

        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                if(value == null)
                {
                    return;
                }

                _selectedMaterial = value;

                NotifyPropertyChanged(nameof(AvarageGrainSize));
                NotifyPropertyChanged(nameof(SurfaceLayerThickness));
                NotifyPropertyChanged(nameof(Porosity));
                NotifyPropertyChanged(nameof(SpecificSurfaceEnergy));
                NotifyPropertyChanged(nameof(CompactMaterialViscosity));
                NotifyPropertyChanged(nameof(CompactMaterialDensity));
                NotifyPropertyChanged(nameof(Weight));
                NotifyPropertyChanged(nameof(PreExponentialFactorOfSurfaceSelfCoefficient));
                NotifyPropertyChanged(nameof(GrainBoundaryDiffusionActivationEnergy));
                NotifyPropertyChanged(nameof(PreExponentialFactorOfGraindBoundaryDiffusionCoefficient));
                NotifyPropertyChanged(nameof(SurfaceSelfDiffusionActivationEnergy));
            }
        }

        public ObservableCollection<Equipment> EquipmentList
        {
            get { return _equipmentList; }
            set
            {
                _equipmentList = value;
                NotifyPropertyChanged(nameof(EquipmentList));
            }
        }

        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedEquipment = value;
                NotifyPropertyChanged(nameof(MinFinalTempretare));
                NotifyPropertyChanged(nameof(MaxFinalTempretare));
                NotifyPropertyChanged(nameof(MinSinteringTime));
                NotifyPropertyChanged(nameof(MaxSinteringTime));
                NotifyPropertyChanged(nameof(MinCuringTime));
                NotifyPropertyChanged(nameof(MaxCuringTime));
                NotifyPropertyChanged(nameof(MinGasPressure));
                NotifyPropertyChanged(nameof(MaxGasPressure));
            }
        }

        public int UserId { get; set; } = 1;
        public double AvarageGrainSize { get => _selectedMaterial.AvarageGrainSize; set => _selectedMaterial.AvarageGrainSize = value; }
        public double SurfaceLayerThickness { get => _selectedMaterial.SurfaceLayerThickness; set => _selectedMaterial.SurfaceLayerThickness = value; }
        public double Porosity { get => _selectedMaterial.Porosity; set => _selectedMaterial.Porosity = value; }
        public double SpecificSurfaceEnergy { get => _selectedMaterial.SpecificSurfaceEnergy; set => _selectedMaterial.SpecificSurfaceEnergy = value; }
        public double CompactMaterialDensity { get => _selectedMaterial.CompactMaterialDensity; set => _selectedMaterial.CompactMaterialDensity = value; }
        public double CompactMaterialViscosity { get => _selectedMaterial.CompactMaterialViscosity; set => _selectedMaterial.CompactMaterialViscosity = value; }
        public double Weight { get => _selectedMaterial.Weight; set => _selectedMaterial.Weight = value; }
        public double PreExponentialFactorOfSurfaceSelfCoefficient
        {
            get => _selectedMaterial.TheoreticalMMParam.PreExponentialFactorOfSurfaceSelfCoefficient;
            set => _selectedMaterial.TheoreticalMMParam.PreExponentialFactorOfSurfaceSelfCoefficient = value;
        }
        public double GrainBoundaryDiffusionActivationEnergy
        {
            get => _selectedMaterial.TheoreticalMMParam.GrainBoundaryDiffusionActivationEnergy; 
            set => _selectedMaterial.TheoreticalMMParam.GrainBoundaryDiffusionActivationEnergy = value;
        }
        public double PreExponentialFactorOfGraindBoundaryDiffusionCoefficient 
        { 
            get => _selectedMaterial.TheoreticalMMParam.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient;
            set => _selectedMaterial.TheoreticalMMParam.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient = value; 
        }
        public double SurfaceSelfDiffusionActivationEnergy
        {
            get => _selectedMaterial.TheoreticalMMParam.SurfaceSelfDiffusionActivationEnergy;
            set => _selectedMaterial.TheoreticalMMParam.SurfaceSelfDiffusionActivationEnergy = value;
        }
        public bool IsothermalSinteringStageDisabled { get => _isothermalSinteringStageDisabled; set => _isothermalSinteringStageDisabled = value; }
        public double InitialTemperatureInFurnace { get => _initialTemperatureInFurnace; set => _initialTemperatureInFurnace = value; }
        public double FinalTemperatureInFurnace { get => _finalTemperatureInFurnace; set => _finalTemperatureInFurnace = value; }
        public double SinteringTime { get => _sinteringTime; set => _sinteringTime = value; }
        public double ExcerptTime { get => _excerptTime; set => _excerptTime = value; }
        public double Pressure { get => _pressure; set => _pressure = value; }
        public bool IsAdmin { get => _isAdmin; set { _isAdmin = value; NotifyPropertyChanged(); } }
        public bool IsInstructor { get => _isInstructor; set { _isInstructor = value; NotifyPropertyChanged(); } }
        public int? RoleId { get ; set; }
        public bool IsCalculateButtonEnabled { get => !_isInvalidElements.Any(); }

        public double ResultPorosity { get => _resultPorosity; set { _resultPorosity = value; NotifyPropertyChanged(); } }
        public double ResultAvarageGrainSize { get => _resultAvarageGrainSize; set { _resultAvarageGrainSize = value; NotifyPropertyChanged(); } }
        public double ResultDensity { get => _resultDensity; set { _resultDensity = value; NotifyPropertyChanged(); } }
        public double ResultViscosity { get => _resultViscosity; set { _resultViscosity = value; NotifyPropertyChanged(); } }
        public int StepsAmount { get => _stepsAmount; set { _stepsAmount = value; NotifyPropertyChanged(); } }
        public double Epsilon { get => _epsilon; set { _epsilon = value; NotifyPropertyChanged(); } }
        public int MathModelMaxDivisionAmount { get => _maxStepDivision; set { _maxStepDivision = value; NotifyPropertyChanged(); } }
        public double BendingStrengthE { get => Math.Round(_bendingStrengthE ?? 0, 2); set { _bendingStrengthE = value; NotifyPropertyChanged(); } }
        public double DensityE { get => Math.Round(_densityE ?? 0, 2); set { _densityE = value; NotifyPropertyChanged(); } }
        public double PorosityE { get => Math.Round(_porosityE ?? 0, 2); set { _porosityE = value; NotifyPropertyChanged(); } }
        public double HardnessE { get => Math.Round(_hardnessE ?? 0, 2); set { _hardnessE = value; NotifyPropertyChanged(); } }

        public double MinFinalTempretare { get => _selectedEquipment.Regime.MinFinalTempretare; set { _selectedEquipment.Regime.MinFinalTempretare = value; } }
        public double MaxFinalTempretare { get => _selectedEquipment.Regime.MaxFinalTempretare; set { _selectedEquipment.Regime.MaxFinalTempretare = value; } }
        public double MinSinteringTime { get => _selectedEquipment.Regime.MinSinteringTime; set { _selectedEquipment.Regime.MinSinteringTime = value; } }
        public double MaxSinteringTime { get => _selectedEquipment.Regime.MaxSinteringTime; set { _selectedEquipment.Regime.MaxSinteringTime = value; } }
        public double MinCuringTime { get => _selectedEquipment.Regime.MinCuringTime; set { _selectedEquipment.Regime.MinCuringTime = value; } }
        public double MaxCuringTime { get => _selectedEquipment.Regime.MaxCuringTime; set { _selectedEquipment.Regime.MaxCuringTime = value; } }
        public double MinGasPressure { get => _selectedEquipment.Regime.MinGasPressure; set { _selectedEquipment.Regime.MinGasPressure = value; } }
        public double MaxGasPressure { get => _selectedEquipment.Regime.MaxGasPressure; set { _selectedEquipment.Regime.MaxGasPressure = value; } }

        public ObservableCollection<ChartTable> Table { get => _table; set { _table = value; NotifyPropertyChanged(); } }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow(Context context, EditDataBaseWindow editDataBaseWindow,
            InstuctorWindow instuctorWindow, InformationWindow informationWindow)
        {
            _context = context;
            _editDataBaseWindow = editDataBaseWindow;
            _instuctorWindow = instuctorWindow;
            _informationWindow = informationWindow;
            _charts = new Dictionary<WpfPlot, ScatterPlot>();

            _materialsList = new ObservableCollection<Material>(_context.Materials.AsNoTracking()
                .Include(material => material.TheoreticalMMParam));
            _selectedMaterial = _materialsList.FirstOrDefault() ?? new Material();
            _stepsAmount = Properties.Settings.Default.StepsAmount;
            _epsilon = Properties.Settings.Default.Epsilon;
            _maxStepDivision = Properties.Settings.Default.MaxDivisionAmount;
            _equipmentList = new ObservableCollection<Equipment>(_context.Equipments.AsNoTracking()
                .Include(e => e.Regime));
            _selectedEquipment = _equipmentList.FirstOrDefault() ?? new Equipment();

            this.DataContext = this;

            InitializeComponent();
            
            this.grid.ItemsSource = Table;
            
            Temperature.Plot.XLabel("Время, мин");
            Temperature.Plot.YLabel("Температура в печи, С");
            Density.Plot.XLabel("Время, мин");
            Density.Plot.YLabel("Плотность материала, кг/м³");
            PorosityPlot.Plot.XLabel("Время, мин");
            PorosityPlot.Plot.YLabel("Пористость материала, %");
            AvgGrainSize.Plot.XLabel("Время, мин");
            AvgGrainSize.Plot.YLabel("Средний диаметр зерна, мкм");
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            MaterialCharacteristicsDTO? result = null;

            var model = new Sintering(
                t0: InitialTemperatureInFurnace,
                tk: FinalTemperatureInFurnace,
                l0: AvarageGrainSize / 1000000,
                p0: Porosity,
                tau1: SinteringTime * 60,
                d: SurfaceLayerThickness * 0.000000001,
                db0: PreExponentialFactorOfGraindBoundaryDiffusionCoefficient,
                ds0: PreExponentialFactorOfSurfaceSelfCoefficient,
                eb: GrainBoundaryDiffusionActivationEnergy * 1000,
                es: SurfaceSelfDiffusionActivationEnergy * 1000,
                s: SpecificSurfaceEnergy,
                eta0: CompactMaterialViscosity * 1000000,
                pg: Pressure * 1000000,
                m: Weight,
                ro0: CompactMaterialDensity,
                tau2: ExcerptTime * 60);

            try
            {
                result = model.Calculate(!IsothermalSinteringStageDisabled, StepsAmount, Epsilon, MathModelMaxDivisionAmount);
            }
            catch
            {
                MessageBox.Show("Возникла непредвиденная ошибка при расчете", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (result == null)
            {
                MessageBox.Show("Не найдено устойчивого решения", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            ResultPorosity = Math.Round(result.PP, 2);
            ResultDensity = Math.Round(result.Ro, 0);
            ResultViscosity = Math.Round(result.Ett, 2);
            ResultAvarageGrainSize = Math.Round(result.LL, 2);

            CalculateImpericalModels();

            var temperaturePlot = model.GetTemperatureChartValues();
            var densityPlot = model.GetDensityChartValues();
            var porosityPlot = model.GetPorosityChartValues();
            var grainSizePlot = model.GetGrainSizeChartValues();

            Table.Clear();
            double prevPorosity = porosityPlot.FirstOrDefault().Value,
                prevDensity = densityPlot.FirstOrDefault().Value,
                prevGrainSize = grainSizePlot.FirstOrDefault().Value;

            foreach (var key in temperaturePlot.Keys)
            {
                if (prevPorosity == Math.Round(porosityPlot[key], 2) &&
                    prevDensity == Math.Round(densityPlot[key], 0) &&
                    Math.Round(grainSizePlot[key], 2) == prevGrainSize)
                    continue;

                prevPorosity = Math.Round(porosityPlot[key], 2);
                prevDensity = Math.Round(densityPlot[key], 0);
                prevGrainSize = Math.Round(grainSizePlot[key], 2);

                Table.Add(new ChartTable()
                {
                    Time = Math.Round(key, 2),
                    Temperature = Math.Round(temperaturePlot[key], 2),
                    Porosity = Math.Round(porosityPlot[key], 2),
                    Density = Math.Round(densityPlot[key], 0),
                    GrainSize = Math.Round(grainSizePlot[key], 2)
                });
            }

            Temperature.Plot.Clear();
            Density.Plot.Clear();
            PorosityPlot.Plot.Clear();
            AvgGrainSize.Plot.Clear();
            _charts.Clear();

            _charts.Add(Temperature,
                Temperature.Plot.AddScatter(temperaturePlot.Select(x => x.Key).ToArray(),
                    temperaturePlot.Select(x => x.Value).ToArray(), markerSize: 1));
            _charts.Add(Density, 
                Density.Plot.AddScatter(densityPlot.Select(x => x.Key).ToArray(),
                    densityPlot.Select(x => x.Value).ToArray(), markerSize: 1));
            _charts.Add(PorosityPlot, 
                PorosityPlot.Plot.AddScatter(porosityPlot.Select(x => x.Key).ToArray(),
                    porosityPlot.Select(x => x.Value).ToArray(), markerSize: 1));
            _charts.Add(AvgGrainSize, 
                AvgGrainSize.Plot.AddScatter(grainSizePlot.Select(x => x.Key).ToArray(),
                    grainSizePlot.Select(x => x.Value).ToArray(), markerSize: 1));

            Temperature.Refresh();
            Density.Refresh();
            PorosityPlot.Refresh();
            AvgGrainSize.Refresh();

            MessageBox.Show("Расчет успешно завершен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EditDataBase(object sender, RoutedEventArgs e)
        {
            _editDataBaseWindow.ShowDialog();

            MaterialsList = new ObservableCollection<Material>(_context.Materials.AsNoTracking()
                .Include(material => material.TheoreticalMMParam));

            var material = MaterialsList.FirstOrDefault(m => m.Id == SelectedMaterial.Id);
            materialsListComboBox.SelectedIndex = material == null 
                ? 0
                : MaterialsList.IndexOf(material);
        }

        private void ShowInstructorWindow(object sender, RoutedEventArgs e)
        {
            _instuctorWindow.InitializeWindow(UserId);
            _instuctorWindow.Show();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.Hide();

            var authWindow = new AuthorizationWindow(_context, this);
            authWindow.Show();
        }

        private void TextBox_Error(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            if (!_isInvalidElements.Contains(sender))
                _isInvalidElements.Add(sender);

            NotifyPropertyChanged(nameof(IsCalculateButtonEnabled));
        }

        private void TextBox_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (_isInvalidElements.Contains(sender))
                _isInvalidElements.Remove(sender);

            NotifyPropertyChanged(nameof(IsCalculateButtonEnabled));
        }

        private void CalculateImpericalModels()
        {
            var empiricalModels = _context.EmpiricalModels.AsNoTracking()
                .Include(x => x.ParamsRanges)
                .Include(x => x.EmpiricalModelCoeffs)
                .Where(x => x.MaterialId == _selectedMaterial.Id)
                .ToList();

            if (!empiricalModels.Any())
                return;

            empiricalModels.ForEach(model =>
            {
                int satisfiesAmount = 0;

                model.ParamsRanges.ForEach(param =>
                {
                    if (param.UnitId == (int)EmpiricalModelUnitTypeEnum.Temperature &&
                        param.MaxValue >= _finalTemperatureInFurnace &&
                        param.MinValue <= _finalTemperatureInFurnace)
                        satisfiesAmount++;

                    if (param.UnitId == (int)EmpiricalModelUnitTypeEnum.Time &&
                        param.MaxValue >= _excerptTime &&
                        param.MinValue <= _excerptTime)
                        satisfiesAmount++;

                    if (param.UnitId == (int)EmpiricalModelUnitTypeEnum.Atmosphere &&
                        param.MaxValue >= _pressure &&
                        param.MinValue <= _pressure)
                        satisfiesAmount++;
                });

                if (satisfiesAmount == model.ParamsRanges.Count)
                {
                    NCalc.Expression e = new NCalc.Expression(model.Formula);

                    e.Parameters["tao"] = _excerptTime;
                    e.Parameters["T"] = _finalTemperatureInFurnace;
                    e.Parameters["Pg"] = _pressure;

                    model.EmpiricalModelCoeffs.ForEach(coef =>
                    {
                        e.Parameters[coef.Alias] = coef.Value;
                    });

                    try
                    {
                        if (model.TypeId == (int)EmpiricalModelTypeEnum.Endurance)
                        {
                            BendingStrengthE = e.Evaluate() as double? ?? 0;
                        }
                        else if (model.TypeId == (int)EmpiricalModelTypeEnum.Hardness)
                        {
                            HardnessE = e.Evaluate() as double? ?? 0;
                        }
                        else if (model.TypeId == (int)EmpiricalModelTypeEnum.Porosity)
                        {
                            PorosityE = e.Evaluate() as double? ?? 0;
                        }
                        else if (model.TypeId == (int)EmpiricalModelTypeEnum.Density)
                        {
                            DensityE = e.Evaluate() as double? ?? 0;
                        }
                    }
                    catch 
                    {
                        MessageBox.Show("Произошла ошибка при расчете эмипирических моделей",
                            "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            });
        }

        #region Crosshair

        private void PlotMouseEnter(object sender, MouseEventArgs e)
        {
            if(_charts.Count == 0)
            {
                return;
            }

            var plot = (WpfPlot)sender;
            var chart = _charts[plot];

            (double mouseCoordX, double mouseCoordY) = plot.GetMouseCoordinates();
            double xyRatio = plot.Plot.XAxis.Dims.PxPerUnit / plot.Plot.YAxis.Dims.PxPerUnit;
            (double pointX, double pointY, int pointIndex) = chart.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);

            _crosshair = plot.Plot.AddCrosshair(mouseCoordX, mouseCoordY);
            _crosshair.X = pointX; _crosshair.Y = pointY;

            plot.Refresh();
        }

        private void PlotMouseLeave(object sender, MouseEventArgs e)
        {
            if (_crosshair != null)
            {
                var plot = (WpfPlot)sender;

                _crosshair.IsVisible = false;

                plot.Refresh();
            }
        }

        private void PlotMouseMove(object sender, MouseEventArgs e)
        {
            if (_crosshair != null)
            {
                var plot = (WpfPlot)sender;
                var chart = _charts[plot];

                (double mouseCoordX, double mouseCoordY) = plot.GetMouseCoordinates();
                double xyRatio = plot.Plot.XAxis.Dims.PxPerUnit / plot.Plot.YAxis.Dims.PxPerUnit;
                (double pointX, double pointY, int pointIndex) = chart.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);

                _crosshair.X = pointX; _crosshair.Y = pointY;

                plot.Refresh();
            }
        }

        #endregion

        private void InformationClick(object sender, RoutedEventArgs e)
        {
            _informationWindow.ShowDialog();
        }
    }
}
