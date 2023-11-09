using Entity;
using System.ComponentModel;
using System.Windows;

namespace Sintering_of_ceramics
{
    public partial class EditDataBaseWindow : Window
    {
        private readonly Context _context;


        public EditDataBaseWindow(Context context)
        {
            _context = context;

            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
