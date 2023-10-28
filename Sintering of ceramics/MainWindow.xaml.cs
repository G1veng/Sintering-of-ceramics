using System.Linq;
using System.Windows;
using Entity;
using Mathematics;
using Microsoft.EntityFrameworkCore;

namespace Sintering_of_ceramics
{
    public partial class MainWindow : Window
    {
        public MainWindow(Context context)
        {
            InitializeComponent();

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
                db0: 0.35,
                ds0: 0.4,
                eb: 171.5 * 1000,
                es: 245 * 1000,
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

            Temperature.Plot.AddScatter(temperaturePlot.Select(x => x.Key).ToArray(), temperaturePlot.Select(x => x.Value).ToArray(), markerSize: 1);
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
            AvgGrainSize.Refresh();
        }
    }
}
