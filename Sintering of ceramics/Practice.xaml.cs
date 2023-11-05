using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Entity;
using Entity.Models;
using Mathematics;
using Microsoft.EntityFrameworkCore;
using ScottPlot.Drawing.Colormaps;

namespace Sintering_of_ceramics
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Material> _materialsList;
        private Material _selectedMaterial;


        public ObservableCollection<Material> MaterialsList 
        {
            get { return _materialsList; }
            set { _materialsList = value; }
        }

        public Material SelectedMaterial { get => _selectedMaterial; set => _selectedMaterial = value; }
        public double AvarageGrainSize { get => _selectedMaterial.AvarageGrainSize; set => _selectedMaterial.AvarageGrainSize = value; }


        public MainWindow(Context context)
        {
            InitializeComponent();

            _materialsList = new ObservableCollection<Material>(context.Materials.AsNoTracking().ToList());
            _selectedMaterial = context.Materials.AsNoTracking().First();

            var material = context.Materials.Include(x => x.TheoreticalMMParam)
                .AsNoTracking()
                .Where(material => material.Id == 1)
                .First();

            var model = new Sintering(
                t0: 20,
                tk: 1350,
                l0: material.AvarageGrainSize / 1000000, 
                p0: material.Porosity,
                tau1: 70 * 60,
                d: material.SurfaceLayerThickness * 0.000000001,
                db0: material.TheoreticalMMParam.PreExponentialFactorOfGraindBoundaryDiffusionCoefficient,
                ds0: material.TheoreticalMMParam.PreExponentialFactorOfSurfaceSelfCoefficient,
                eb: material.TheoreticalMMParam.GrainBoundaryDiffusionActivationEnergy * 1000,
                es: material.TheoreticalMMParam.SurfaceSelfDiffusionActivationEnergy * 1000,
                s: material.SpecificSurfaceEnergy, 
                eta0: material.CompactMaterialViscosity * 1000000,
                pg: 6 * 1000000,
                m: material.Weight,
                ro0: material.CompactMaterialDensity, 
                tau2: 30 * 60);

            var result = model.Calculate(true);

            var temperaturePlot = model.GetTemperatureChartValues();
            var densityPlot = model.GetDensityChartValues();
            var porosityPlot = model.GetPorosityChartValues();
            var grainSizePlot = model.GetGrainSizeChartValues();

            /*Temperature.Plot.AddScatter(temperaturePlot.Select(x => x.Key).ToArray(), temperaturePlot.Select(x => x.Value).ToArray(), markerSize: 1);
            Temperature.Plot.XLabel("Время, мин");
            Temperature.Plot.YLabel("Температура в печи, С");

            Density.Plot.AddScatter(densityPlot.Select(x => x.Key).ToArray(), densityPlot.Select(x => x.Value).ToArray(), markerSize: 1);
            Density.Plot.XLabel("Время, мин");
            Density.Plot.YLabel("Плотнсоть материала, кг/м^3");

            Porosity.Plot.AddScatter(porosityPlot.Select(x => x.Key).ToArray(), porosityPlot.Select(x => x.Value).ToArray(), markerSize: 1);
            Porosity.Plot.XLabel("Время, мин");
            Porosity.Plot.YLabel("Пористость материала, %");

            AvgGrainSize.Plot.AddScatter(grainSizePlot.Select(x => x.Key).ToArray(), grainSizePlot.Select(x => x.Value).ToArray(), markerSize: 1);
            AvgGrainSize.Plot.XLabel("Время, мин");
            AvgGrainSize.Plot.YLabel("Средний размер зерна, мкм");

            Temperature.Refresh();
            Density.Refresh();
            Porosity.Refresh();
            AvgGrainSize.Refresh();*/
        }

        private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            var fullText = textBox!.Text.Insert(textBox.SelectionStart, e.Text);

            double val;
            
            e.Handled = fullText != "-" && !double.TryParse(fullText,
                                         NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                                         CultureInfo.InvariantCulture,
                                         out val);
        }
    }
}
