using Sintering_of_ceramics;
using System.Windows;

public class App : Application
{
    readonly MainWindow mainWindow;
    readonly AuthorizationWindow authorizationWindow;

    public App(MainWindow mainWindow, AuthorizationWindow authorizationWindow)
    {
        this.mainWindow = mainWindow;
        this.authorizationWindow = authorizationWindow;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        authorizationWindow.Show();
        //mainWindow.Show();
        base.OnStartup(e);
    }
}