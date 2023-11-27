using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using Sintering_of_ceramics;
using Entity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Sintering_of_ceramics.Windows;

public class Program
{
    [STAThread]
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();
                services.AddSingleton<MainWindow>();
                services.AddScoped<AuthorizationWindow>();
                services.AddScoped<EditDataBaseWindow>();
                services.AddScoped<CreateEditDeleteWindow>();
                services.AddDbContext<Context>(options =>
                    options.UseSqlite(ConfigurationManager.ConnectionStrings["mainDb"].ConnectionString));
            })
            .Build();
        
        var context = host.Services.GetService<Context>();
        if(context == null )
        {
            throw new Exception("Unable to get context of db");
        }

        context!.Database.Migrate();

        var app = host.Services.GetService<App>();        
        app?.Run();
    }
}