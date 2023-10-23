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
                t0: 20 + 273.15,
                tk: 1350 + 273.15,
                l0: material.AvarageGrainSize * 0.000001, 
                p0: material.Porosity / 100,
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
        }
    }
}
