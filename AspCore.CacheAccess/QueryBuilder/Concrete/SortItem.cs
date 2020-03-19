namespace AspCore.CacheAccess.QueryBuilder.Concrete
{
    public class SortItem
    {
        public string[] ascendingFields { get; set; }
        public string[] descendingFields { get; set; }

        public SortItem()
        {
            this.ascendingFields = null;
            this.descendingFields = null;
        }
    }
}
