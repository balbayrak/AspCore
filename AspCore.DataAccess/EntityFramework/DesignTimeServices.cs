using Inflector;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Globalization;

namespace AspCore.DataAccess.EntityFramework.Mapping
{
    public class DesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            Debugger.Launch();
            services.AddSingleton<ICandidateNamingService, CustomCandidateNamingService>()
                     .AddSingleton<ICSharpEntityTypeGenerator, MyEntityTypeGenerator>()
                     .AddSingleton<IPluralizer, CustomPluralizer>();
        }
    }

    public class CustomCandidateNamingService : CandidateNamingService
    {
        public override string GetDependentEndCandidateNavigationPropertyName(IForeignKey foreignKey)
        {
            if (foreignKey.PrincipalKey.IsPrimaryKey())
                return foreignKey.PrincipalEntityType.ShortName();

            return base.GetDependentEndCandidateNavigationPropertyName(foreignKey);
        }
    }

    public class CustomPluralizer : IPluralizer
    {
        public string Pluralize(string identifier)
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en");

            return identifier.Pluralize();
        }

        public string Singularize(string identifier)
        {
            Inflector.Inflector.SetDefaultCultureFunc = () => new CultureInfo("en");
            return identifier.Singularize();
        }
    }
}
