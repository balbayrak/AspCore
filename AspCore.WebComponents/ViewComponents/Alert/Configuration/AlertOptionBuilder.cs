using AspCore.Entities.Configuration;
using AspCore.ViewComponents.Components.Alert.Concrete;
using AspCore.ViewComponents.ViewComponents;
using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;
using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;

namespace AspCore.ViewComponents.Components.Alert.Configuration
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

            string namespaceStr = (string.IsNullOrEmpty(alertOption.baseNameSpace) && string.IsNullOrWhiteSpace(alertOption.baseNameSpace)) ? "AspCore.ViewComponents" : alertOption.baseNameSpace;

            var embeddedFileProvider = new EmbeddedFileProvider(
                assembly,
                namespaceStr
            );

            _services.AddControllersWithViews().AddRazorRuntimeCompilation(options => options.FileProviders.Add(embeddedFileProvider));


            if (alertOption.alertStorage == EnumAlertStorage.TempData)
            {
                HttpContextWrapper.Configure(_services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>());

                _services.AddSingleton<IAlertStorage, TempDataStorage>();
            }

            if (alertOption.alertType == AlertType.Alertify)
            {
                _services.AddSingleton<IAlertService, AlertifyAlertManager>();
            }
            else if (alertOption.alertType == AlertType.Toast)
            {
                _services.AddSingleton<IAlertService, ToastAlertManager>();
            }
            else if (alertOption.alertType == AlertType.BootBox)
            {
                _services.AddSingleton<IAlertService, BootBoxAlertManager>();
            }
            else if (alertOption.alertType == AlertType.Default)
            {
                _services.AddSingleton<IAlertService, DefaultAlertManager>();
            }
            else if (alertOption.alertType == AlertType.Sweet)
            {
                _services.AddSingleton<IAlertService, SweetAlertManager>();
            }

            return this;
        }
        public AlertOptionBuilder AddConfirmService(Action<ConfirmActionOption> option)
        {
            ConfirmActionOption confirmActionOption = new ConfirmActionOption();
            option.Invoke(confirmActionOption);

            if (confirmActionOption.confirmType == ConfirmType.Alertify)
            {
                _services.AddSingleton<IConfirmService, AlertifyConfirmManager>();
            }
            else if (confirmActionOption.confirmType == ConfirmType.BootBox)
            {
                _services.AddSingleton<IConfirmService, BootBoxConfirmManager>();
            }
            else if (confirmActionOption.confirmType == ConfirmType.Default)
            {
                _services.AddSingleton<IConfirmService, DefaultConfirmManager>();
            }
            else if (confirmActionOption.confirmType == ConfirmType.Sweet)
            {
                _services.AddSingleton<IConfirmService, SweetConfirmManager>();
            }
            return this;
        }

    }
}
