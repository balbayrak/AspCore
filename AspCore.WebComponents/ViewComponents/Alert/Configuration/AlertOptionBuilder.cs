using AspCore.Entities.Configuration;
using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;
using AspCore.WebComponents.HtmlHelpers.Extensions;
using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;

namespace AspCore.WebComponents.ViewComponents.Alert.Configuration
{
    public class AlertOptionBuilder : ConfigurationOption
    {
        public AlertOptionBuilder(IServiceCollection services) : base(services)
        {
        }

        public AlertOptionBuilder AddAlertViewComponent(Action<AlertOption> option)
        {
            AlertOption alertOption = new AlertOption();
            option.Invoke(alertOption);

            var assembly = typeof(AlertViewComponent).Assembly;

            string namespaceStr = (string.IsNullOrEmpty(alertOption.baseNameSpace) && string.IsNullOrWhiteSpace(alertOption.baseNameSpace)) ? "AspCore.WebComponents" : alertOption.baseNameSpace;

            var embeddedFileProvider = new EmbeddedFileProvider(
                assembly,
                namespaceStr
            );

            services.AddControllersWithViews().AddRazorRuntimeCompilation(options => options.FileProviders.Add(embeddedFileProvider));


            if (alertOption.alertStorage == EnumAlertStorage.TempData)
            {
             //   HttpContextWrapper.Configure(services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>());

                services.AddSingleton<IAlertStorage, TempDataStorage>();
            }

            if (alertOption.alertType == AlertType.Alertify)
            {
                services.AddSingleton<IAlertService, AlertifyAlertManager>();
            }
            else if (alertOption.alertType == AlertType.Toast)
            {
                services.AddSingleton<IAlertService, ToastAlertManager>();
            }
            else if (alertOption.alertType == AlertType.BootBox)
            {
                services.AddSingleton<IAlertService, BootBoxAlertManager>();
            }
            else if (alertOption.alertType == AlertType.Default)
            {
                services.AddSingleton<IAlertService, DefaultAlertManager>();
            }
            else if (alertOption.alertType == AlertType.Sweet)
            {
                services.AddSingleton<IAlertService, SweetAlertManager>();
            }

            return this;
        }
        public AlertOptionBuilder AddConfirmService(Action<ConfirmActionOption> option)
        {
            ConfirmActionOption confirmActionOption = new ConfirmActionOption();
            option.Invoke(confirmActionOption);

            if (confirmActionOption.confirmType == ConfirmType.Alertify)
            {
                services.AddSingleton<IConfirmService, AlertifyConfirmManager>();
            }
            else if (confirmActionOption.confirmType == ConfirmType.BootBox)
            {
                services.AddSingleton<IConfirmService, BootBoxConfirmManager>();
            }
            else if (confirmActionOption.confirmType == ConfirmType.Default)
            {
                services.AddSingleton<IConfirmService, DefaultConfirmManager>();
            }
            else if (confirmActionOption.confirmType == ConfirmType.Sweet)
            {
                services.AddSingleton<IConfirmService, SweetConfirmManager>();
            }
            return this;
        }

    }
}
