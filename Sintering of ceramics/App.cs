using Sintering_of_ceramics;
using System.Windows;

public class App : Application
{
    readonly MainWindow mainWindow;

    public App(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        mainWindow.Show();
        base.OnStartup(e);
    }
}