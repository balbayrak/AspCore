using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using AspCore.Entities.EntityType;

namespace AspCore.DataAccess.EntityFramework.Mapping
{
    public class MyEntityTypeGenerator : CSharpEntityTypeGenerator
    {
        public MyEntityTypeGenerator(ICSharpHelper cSharpUtilities)
       : base(cSharpUtilities)
        { }

        public override string WriteCode(IEntityType entityType, string @namespace, bool useDataAnnotations)
        {
            string code = base.WriteCode(entityType, @namespace, useDataAnnotations);
            string inheritance = "IEntity";
            if (code.Contains("public DateTime CreatedDate { get; set; }"))
            {
                inheritance = "IBaseEntity";
            }

            if (code.Contains("public string DocumentUrl { get; set; }"))
            {
                inheritance = "IDocumentEntity";
            }

            var oldString = "public partial class " + entityType.Name;
            var newString = "public partial class " + entityType.Name + " :" + inheritance;

            var oldProperty = "public Guid Id { get; set; }";
            var newProperty = "public CoreEntityState? entityState { get; set; }" + "\n\t\t" + "public Guid Id { get; set; }";

            code = code.Replace(oldProperty, newProperty);

            code = "using " + typeof(IEntity).Namespace + ";" + "\n" + code;

            return code.Replace(oldString, newString);
        }
    }
}
