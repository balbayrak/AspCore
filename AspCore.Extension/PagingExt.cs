using System.Collections.Generic;
using System.Linq;

namespace AspCore.Extension
{
    public static class PagingExt
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> list, int? pageSize, int? page)
        {
            if (pageSize!=null && page!=null)
            {
                 return list.Skip((int) (page * pageSize)).Take((int) pageSize);
            }

            return list;
        }
        public static IQueryable<T> Page<T>(this IQueryable<T> list, int? pageSize, int? page)
        {
            if (pageSize != null && page != null)
            {
                return list.Skip((int)(page * pageSize)).Take((int)pageSize);
            }
            return list;
        }
    }
}
