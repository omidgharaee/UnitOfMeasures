using Framework.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UnitOfMeasures.Core.Application;
using UnitOfMeasures.Core.Application.Services;
using UnitOfMeasures.Core.Domain.Dimension.Data;
using UnitOfMeasures.Core.Domain.Services;
using UnitOfMeasures.Endpoints.WPF.Windows;
using UnitOfMeasures.Infrastructure.Persistence;
using UnitOfMeasures.Infrastructure.Persistence.Contexts;
using UnitOfMeasures.Infrastructure.Persistence.Dimension;
using UnitOfMeasures.Infrastructure.Persistence.Unit;

namespace UnitOfMeasures.Endpoints.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            ServiceContainer.Instance = serviceProvider;
            var dbContext = serviceProvider.GetRequiredService<SQLiteDataBaseContext>();
            dbContext.Database.Migrate();
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }


        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<SQLiteDataBaseContext>();
            services.AddDbContext<SQLiteDataBaseContext>();
            services.AddSingleton<MainWindow>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDimensionRepository, EfDimensionRepository>();
            services.AddScoped<IUnitRepository, EfUnitRepository>();
            services.AddScoped<Core.Application.Dimension.CommandHandlers.CreateHandler>();
            services.AddScoped<Core.Application.Dimension.CommandHandlers.UpdateHandler>();
            services.AddScoped<Core.Application.Dimension.CommandHandlers.DeleteHandler>();
            services.AddScoped<IDimensionQueryService, DimensionQueryService>();

            services.AddScoped<Core.Application.Unit.CommandHandlers.CreateHandler>();
            services.AddScoped<Core.Application.Unit.CommandHandlers.UpdateHandler>();
            services.AddScoped<Core.Application.Unit.CommandHandlers.DeleteHandler>();

            services.AddSingleton<IMathParser, MathParser>();
            services.AddSingleton<IConverter, Core.Application.Services.Converter>();
        }
    }

}
