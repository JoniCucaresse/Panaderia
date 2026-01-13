using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Panaderia.Infrastructure.Persistence;
using Panaderia.Infrastructure.Repositories;
using Panaderia.Wpf.ViewModels;
using Panaderia.Wpf.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Panaderia.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<PanaderiaDbContext>(options =>
                options.UseSqlServer(
                    System.Configuration.ConfigurationManager
                        .ConnectionStrings["PanaderiaDb"].ConnectionString));

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>();
            services.AddTransient<MateriasPrimasViewModel>();
            services.AddTransient<MateriasPrimasView>();
            services.AddTransient<MateriaPrimaFormView>();
            services.AddScoped<IMateriaPrimaRepository, MateriaPrimaRepository>();

            var provider = services.BuildServiceProvider();

            var mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            var window = provider.GetRequiredService<MateriasPrimasView>();
            window.DataContext = provider.GetRequiredService<MateriasPrimasViewModel>();
            window.Show();
        }
    }
}
