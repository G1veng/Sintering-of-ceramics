using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Entity;
using Entity.Models;
using Mathematics;
using Microsoft.EntityFrameworkCore;

namespace Sintering_of_ceramics
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Context _context;
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Material> _materialsList;
        private Material _selectedMaterial;
        private bool _isothermalSinteringStageDisabled = false;
        private double _initialTemperatureInFurnace = 20;
        private double _finalTemperatureInFurnace = 1350;
        private double _sinteringTime = 70;
        private double _excerptTime = 30;
        private double _pressure = 6;


        public ObservableCollection<Material> MaterialsList
        {
            get { return _materialsList; }
            set { _materialsList = value; }
        }

        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
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


        public MainWindow(Context context)
        {
            _context = context;
            InitializeComponent();

            _materialsList = new ObservableCollection<Material>(_context.Materials.AsNoTracking()
                .Include(material => material.TheoreticalMMParam));
            _selectedMaterial = _materialsList.First();

            this.DataContext = SelectedMaterial;

            Temperature.Plot.XLabel("Время, мин");
            Temperature.Plot.YLabel("Температура в печи, С");
            Density.Plot.XLabel("Время, мин");
            Density.Plot.YLabel("Плотнсоть материала, кг/м^3");
            PorosityPlot.Plot.XLabel("Время, мин");
            PorosityPlot.Plot.YLabel("Пористость материала, %");
            AvgGrainSize.Plot.XLabel("Время, мин");
            AvgGrainSize.Plot.YLabel("Средний размер зерна, мкм");

            Temperature.Refresh();
            Density.Refresh();
            PorosityPlot.Refresh();
            AvgGrainSize.Refresh();
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var fullText = textBox!.Text.Insert(textBox.SelectionStart, e.Text);
            
            e.Handled = !double.TryParse(fullText,
                            NumberStyles.AllowDecimalPoint,
                            CultureInfo.InvariantCulture,
                            out var val);
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

            var result = model.Calculate(!IsothermalSinteringStageDisabled);

            var temperaturePlot = model.GetTemperatureChartValues();
            var densityPlot = model.GetDensityChartValues();
            var porosityPlot = model.GetPorosityChartValues();
            var grainSizePlot = model.GetGrainSizeChartValues();

            Temperature.Plot.Clear();
            Density.Plot.Clear();
            PorosityPlot.Plot.Clear();
            AvgGrainSize.Plot.Clear();

            Temperature.Plot.AddScatter(temperaturePlot.Select(x => x.Key).ToArray(), temperaturePlot.Select(x => x.Value).ToArray(), markerSize: 1);
            Density.Plot.AddScatter(densityPlot.Select(x => x.Key).ToArray(), densityPlot.Select(x => x.Value).ToArray(), markerSize: 1);
            PorosityPlot.Plot.AddScatter(porosityPlot.Select(x => x.Key).ToArray(), porosityPlot.Select(x => x.Value).ToArray(), markerSize: 1);
            AvgGrainSize.Plot.AddScatter(grainSizePlot.Select(x => x.Key).ToArray(), grainSizePlot.Select(x => x.Value).ToArray(), markerSize: 1);

            Temperature.Refresh();
            Density.Refresh();
            PorosityPlot.Refresh();
            AvgGrainSize.Refresh();
        }
    }
}
