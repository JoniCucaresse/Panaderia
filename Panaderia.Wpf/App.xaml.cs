using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Panaderia.Domain.Repositories;
using Panaderia.Infrastructure.Persistence;
using Panaderia.Infrastructure.Repositories;
using Panaderia.Wpf.ViewModels;
using Panaderia.Wpf.Views;
using Panaderia.App.Services;
using Panaderia.App.Services.Interfaces;
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
        public IServiceProvider Services { get; private set; } = null!;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            services.AddDbContext<PanaderiaDbContext>(options =>
                options.UseSqlServer(
                    System.Configuration.ConfigurationManager
                        .ConnectionStrings["PanaderiaDb"].ConnectionString));

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<MateriasPrimasViewModel>();
            services.AddTransient<MateriaPrimaFormViewModel>();

            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<MateriasPrimasView>();
            services.AddTransient<ProductosView>(); // ✅ Nuevo

            // Repositories
            services.AddScoped<IMateriaPrimaRepository, MateriaPrimaRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>(); // ✅ Nuevo
            services.AddScoped<IRecetaRepository, RecetaRepository>(); // ✅ Nuevo
            services.AddTransient<ProductosViewModel>();

            // ✅ Services (Application Layer)
            services.AddScoped<IMateriaPrimaService, MateriaPrimaService>();
            services.AddScoped<IProductoService, ProductoService>(); // ✅ Nuevo


            // ✅ Logging (opcional pero recomendado)
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddDebug();
                builder.AddConsole();
            });

            Services = services.BuildServiceProvider();

            // ✅ Abrir ventana de productos
            var mainWindow = Services.GetRequiredService<ProductosView>();
            mainWindow.DataContext = Services.GetRequiredService<ProductosViewModel>();
            mainWindow.Show();
        }
    }
}
