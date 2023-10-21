using System.Linq;
using System.Windows;
using Entity;
using Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Sintering_of_ceramics
{
    public partial class MainWindow : Window
    {
        public MainWindow(Context context)
        {
            InitializeComponent();

            var materials = context.Materials.Include(x => x.TheoreticalMMParam).AsNoTracking().ToList();
        }
    }
}
