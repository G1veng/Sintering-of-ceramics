using System.ComponentModel;
using System.Windows;

namespace Sintering_of_ceramics.Windows
{
    public partial class InformationWindow : Window
    {
        public InformationWindow()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
