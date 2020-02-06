using AspCore.Entities.Expression;

namespace AspCore.Entities.EntityFilter
{
    public class SearchInfo
    {
        public string propertyName { get; set; }

        public Operation operation { get; set; }
    }
}
