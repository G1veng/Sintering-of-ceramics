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
using Microsoft.EntityFrameworkCore;
using ScottPlot;
using ScottPlot.Plottable;
using Sintering_of_ceramics.Models;

namespace Sintering_of_ceramics
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Private properties

        private readonly Context _context;
        private readonly EditDataBaseWindow _editDataBaseWindow;

        private ObservableCollection<Material> _materialsList;
        private Material _selectedMaterial;
        private bool _isothermalSinteringStageDisabled = false;
        private double _initialTemperatureInFurnace = 20;
        private double _finalTemperatureInFurnace = 1350;
        private double _sinteringTime = 70;
        private double _excerptTime = 30;
        private double _pressure = 6;
        private bool _isAdmin = true;
        private int _stepsAmount;
        private double _epsilon;

        private double _resultPorosity = 0;
        private double _resultAvarageGrainSize = 0;
        private double _resultDensity = 0;
        private double _resultViscosity = 0;

        private ObservableCollection<ChartTable> _table = new ObservableCollection<ChartTable>();
        private Crosshair? _crosshair;
        private Dictionary<WpfPlot, ScatterPlot> _charts;

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

        public double ResultPorosity { get => _resultPorosity; set { _resultPorosity = value; NotifyPropertyChanged(); } }
        public double ResultAvarageGrainSize { get => _resultAvarageGrainSize; set { _resultAvarageGrainSize = value; NotifyPropertyChanged(); } }
        public double ResultDensity { get => _resultDensity; set { _resultDensity = value; NotifyPropertyChanged(); } }
        public double ResultViscosity { get => _resultViscosity; set { _resultViscosity = value; NotifyPropertyChanged(); } }
        public int StepsAmount { get => _stepsAmount; set { _stepsAmount = value; NotifyPropertyChanged(); } }
        public double Epsilon { get => _epsilon; set { _epsilon = value; NotifyPropertyChanged(); } }

        public ObservableCollection<ChartTable> Table { get => _table; set { _table = value; NotifyPropertyChanged(); } }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow(Context context, EditDataBaseWindow editDataBaseWindow)
        {
            _context = context;
            _editDataBaseWindow = editDataBaseWindow;
            _charts = new Dictionary<WpfPlot, ScatterPlot>();

            InitializeComponent();

            _materialsList = new ObservableCollection<Material>(_context.Materials.AsNoTracking()
                .Include(material => material.TheoreticalMMParam));
            _selectedMaterial = _materialsList.FirstOrDefault() ?? new Material();
            _stepsAmount = Properties.Settings.Default.StepsAmount;
            _epsilon = Properties.Settings.Default.Epsilon;

            this.DataContext = SelectedMaterial;
            this.grid.ItemsSource = Table;

            Temperature.Plot.XLabel("Время, мин");
            Temperature.Plot.YLabel("Температура в печи, С");
            Density.Plot.XLabel("Время, мин");
            Density.Plot.YLabel("Плотность материала, кг/м^3");
            PorosityPlot.Plot.XLabel("Время, мин");
            PorosityPlot.Plot.YLabel("Пористость материала, %");
            AvgGrainSize.Plot.XLabel("Время, мин");
            AvgGrainSize.Plot.YLabel("Средний размер зерна, мкм");
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

            var result = model.Calculate(!IsothermalSinteringStageDisabled, StepsAmount, Epsilon);

            ResultPorosity = Math.Round(result.PP, 2);
            ResultDensity = Math.Round(result.Ro, 2);
            ResultViscosity = Math.Round(result.Ett, 2);
            ResultAvarageGrainSize = Math.Round(result.LL, 2);

            var temperaturePlot = model.GetTemperatureChartValues();
            var densityPlot = model.GetDensityChartValues();
            var porosityPlot = model.GetPorosityChartValues();
            var grainSizePlot = model.GetGrainSizeChartValues();

            Table.Clear();
            foreach (var key in temperaturePlot.Keys)
            {
                Table.Add(new ChartTable()
                {
                    Time = Math.Round(key, 2),
                    Temperature = Math.Round(temperaturePlot[key], 2),
                    Porosity = Math.Round(porosityPlot[key], 2),
                    Density = Math.Round(densityPlot[key], 2),
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

            MessageBox.Show("Рассчет успешно завершен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void LogOut(object sender, RoutedEventArgs e)
        {
            this.Hide();

            var authWindow = new AuthorizationWindow(_context, this);
            authWindow.Show();
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
    }
}
