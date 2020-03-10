using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.Expression;

namespace AspCore.Utilities
{
    public static class ExpressionBuilder
    {
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains");
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        private static Expression GetExpression<T>(ParameterExpression param, FilterExpression<T> filter)
        {
            //t=>t.(member) = constant
            MemberExpression member = Expression.Property(param, GetExpressionFieldName<T>(filter.Property));
            ConstantExpression constant = Expression.Constant(filter.PropertyValue, member.Type);

            switch (filter.OperationValue)
            {
                case Operation.Equals:
                    return Expression.Equal(member, constant);

                case Operation.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Operation.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Operation.LessThan:
                    return Expression.LessThan(member, constant);

                case Operation.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Operation.Contains:
                    return Expression.Call(member, ContainsMethod, constant);

                case Operation.StartsWith:
                    return Expression.Call(member, StartsWithMethod, constant);

                case Operation.EndsWith:
                    return Expression.Call(member, EndsWithMethod, constant);
            }

            return null;
        }

        public static string GetExpressionFieldName<T>(Expression<Func<T, object>> property)
        {
            if (object.Equals(property, null))
            {
                throw new NullReferenceException("Field is required");
            }

            MemberExpression expr = null;

            if (property.Body is MemberExpression)
            {
                expr = (MemberExpression)property.Body;
            }
            else if (property.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)property.Body).Operand;
            }
            else
            {
                const string Format = "Expression '{0}' not supported.";
                string message = string.Format(Format, property);

                throw new ArgumentException(message, "Field");
            }

            return expr.ToString();
        }

        public static Expression<Func<T, bool>> GetExpression<T>(FilterExpression<T> filter)
        {
            if (filter == null)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = exp = GetExpression<T>(param, filter);
            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        public static string GetExpressionFieldName<TSource>(Expression<Func<TSource>> Field)
        {
            if (object.Equals(Field, null))
            {
                throw new NullReferenceException("Field is required");
            }

            MemberExpression expr = null;

            if (Field.Body is MemberExpression)
            {
                expr = (MemberExpression)Field.Body;
            }
            else if (Field.Body is UnaryExpression)
            {
                expr = (MemberExpression)((UnaryExpression)Field.Body).Operand;
            }
            else
            {
                const string Format = "Expression '{0}' not supported.";
                string message = string.Format(Format, Field);

                throw new ArgumentException(message, "Field");
            }

            return expr.Member.Name;
        }

        public static Expression<Func<T, bool>> GetExpression<T>(IList<FilterExpression<T>> filters)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, FilterExpression<T> filter1, FilterExpression<T> filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }

        public static Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            //if (propertyName.Contains("."))
            //{
            return GetExpressionExt<TSource>(propertyName);
            //}
            //var param = Expression.Parameter(typeof(TSource), "x");
            //Expression conversion = Expression.Convert(Expression.Property
            //(param, propertyName), typeof(object));   //important to use the Expression.Convert
            //return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

        private static Expression<Func<TSource, object>> GetExpressionExt<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression body = param;

            string[] arr = propertyName.Split('.');
            for (int i = 1; i < arr.Length; i++)
            {
                body = Expression.PropertyOrField(body, arr[i]);
            }

            return Expression.Lambda<Func<TSource, object>>(body, param);
        }

        private static MemberExpression GetExpressionBody<TSource>(ParameterExpression param, string propertyName)
        {

            Expression body = param;

            string[] arr = propertyName.Split('.');
            if (arr.Length > 1)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    body = Expression.PropertyOrField(body, arr[i]);
                }
            }
            else
            {
                body = Expression.PropertyOrField(body, arr[0]);
            }

            return body as MemberExpression;
        }

        public static Expression<Func<TSource, bool>> GetSearchExpression<TSource>(IList<SearchInfo> searchableColums, string searchValue)
        {
            List<Expression> expressions = new List<Expression>();
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "t");
            foreach (var searchInfo in searchableColums)
            {
                MethodInfo containsMethodInfo = typeof(string).GetMethod(searchInfo.operation.ToString(), new[] { typeof(string) });
                MemberExpression memberExpression = GetExpressionBody<TSource>(parameter, searchInfo.propertyName);
                ConstantExpression constantExpression = Expression.Constant(searchValue, typeof(string));
                MethodCallExpression methodCallExpression = Expression.Call(memberExpression, containsMethodInfo, constantExpression);
                expressions.Add(methodCallExpression);
            }
            if (expressions.Count == 0)
                return null;
            var orExpression = expressions[0];
            for (var i = 1; i < expressions.Count; i++)
            {
                orExpression = Expression.OrElse(orExpression, expressions[i]);
            }
            Expression<Func<TSource, bool>> expression = Expression.Lambda<Func<TSource, bool>>(
                orExpression, parameter);
            return expression;
        }

        public static Expression<Func<TSource, bool>> GetEqualsExpression<TSource>(string columnName, bool value)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TSource), "t");

            MethodInfo equalsMethodInfo = typeof(bool).GetMethod(Operation.Equals.ToString(), new[] { typeof(bool) });
            MemberExpression memberExpression = GetExpressionBody<TSource>(parameter, columnName);
            ConstantExpression constantExpression = Expression.Constant(value, typeof(bool));
            MethodCallExpression methodCallExpression = Expression.Call(memberExpression, equalsMethodInfo, constantExpression);

            Expression<Func<TSource, bool>> expression = Expression.Lambda<Func<TSource, bool>>(
                methodCallExpression, parameter);
            return expression;
        }

        public static Expression<Func<TInput, bool>> CombineWithAndAlso<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        public static Expression<Func<TInput, bool>> CombineWithOrElse<TInput>(this Expression<Func<TInput, bool>> func1, Expression<Func<TInput, bool>> func2)
        {
            return Expression.Lambda<Func<TInput, bool>>(
                Expression.AndAlso(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        private class ExpressionParameterReplacer : ExpressionVisitor
        {
            public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
            {
                ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
                for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                    ParameterReplacements.Add(fromParameters[i], toParameters[i]);
            }

            private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (ParameterReplacements.TryGetValue(node, out replacement))
                    node = replacement;
                return base.VisitParameter(node);
            }
        }


        public static object Evaluation<TModel>(TModel model, string column_Property_Exp)
        {
            string columnStr = column_Property_Exp.Substring(column_Property_Exp.IndexOf('.')).TrimStart('.');
            return GetPropertyValue(model, columnStr);
        }

        private static object GetPropertyValue(Object fromObject, string propertyName)
        {
            Type objectType = fromObject.GetType();
            PropertyInfo propInfo = objectType.GetProperty(propertyName);
            if (propInfo == null && propertyName.Contains('.'))
            {
                string firstProp = propertyName.Substring(0, propertyName.IndexOf('.'));
                propInfo = objectType.GetProperty(firstProp);
                if (propInfo == null)//property name is invalid
                {
                    throw new ArgumentException(String.Format("Property {0} is not a valid property of {1}.", firstProp, fromObject.GetType().ToString()));
                }
                return GetPropertyValue(propInfo.GetValue(fromObject, null), propertyName.Substring(propertyName.IndexOf('.') + 1));
            }
            else
            {
                return propInfo.GetValue(fromObject, null);
            }
        }

    }

}
