using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AspCore.Utilities
{
    public class CustomComparison
    {
        public static Comparison<T> GetCustomComparison<T>(Expression<Func<T, object>> filter)
        {
            return delegate (T x, T y)
            {
                string propertyName = ExpressionBuilder.GetExpressionFieldName(filter);
                IEnumerable<PropertyInfo> properties = typeof(T).GetProperties();
                PropertyInfo propInfo = properties.Where(t => t.Name.Equals(propertyName)).FirstOrDefault();
                Type propType = propInfo.GetType();

                object XValue = propInfo.GetValue(x, null);
                object YValue = propInfo.GetValue(x, null);

                var ac = XValue as IComparable;
                var bc = YValue as IComparable;

                if (ac == null || bc == null)
                    throw new NotSupportedException();

                return ac.CompareTo(bc);
            };
        }

        public static Comparison<T> GetCustomComparison<T>(string propertyName)
        {
            return delegate (T x, T y)
            {
                IEnumerable<PropertyInfo> properties = typeof(T).GetProperties();
                PropertyInfo propInfo = properties.Where(t => t.Name.Equals(propertyName)).FirstOrDefault();
                Type propType = propInfo.GetType();

                object XValue = propInfo.GetValue(x, null);
                object YValue = propInfo.GetValue(x, null);

                var ac = XValue as IComparable;
                var bc = YValue as IComparable;

                if (ac == null || bc == null)
                    throw new NotSupportedException();

                return ac.CompareTo(bc);
            };
        }
    }
}
