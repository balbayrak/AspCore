using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
  public  static class CollectionsExt
    {
        public static bool AddIfNotContains<T>([NotNull] this ICollection<T> source, T item)
        {
            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> list, int? pageSize, int? page)
        {
            if (pageSize != null && page != null)
            {
                return list.Skip((int)(page * pageSize)).Take((int)pageSize);
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
