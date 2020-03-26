using AspCore.CacheEntityClient.QueryContiner.Concrete;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AspCore.CacheEntityClient.General
{
    public static class ExpressionExt
    {
        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpr = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = expression.Body as MemberExpression;
            }
            return memberExpr.Member.Name;
        }

        public static string GetSortPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpr = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = expression.Body as MemberExpression;
            }

            var propertyInfo = (PropertyInfo)memberExpr.Member;

            if (propertyInfo.PropertyType.Name.ToLower().Equals(typeof(string).Name.ToLower()))
            {
                return memberExpr.Member.Name + CacheClientConstants.ELK_ConstString.KEYWORD_FIELD;
            }

            return memberExpr.Member.Name;
        }

        public static void CheckAdd(this ComplexQueryItemContainer item, ComplexQueryItemContainer complexItem)
        {
            if (complexItem.filterQueryContainer != null)
            {
                item.filterQueryContainer = complexItem.filterQueryContainer;
            }
            if (complexItem.mustNotQueryContainer != null)
            {
                item.mustNotQueryContainer = complexItem.mustNotQueryContainer;
            }
            if (complexItem.mustQueryContainer != null)
            {
                item.mustQueryContainer = complexItem.mustQueryContainer;
            }
            if (complexItem.shouldQueryContainer != null)
            {
                item.shouldQueryContainer = complexItem.shouldQueryContainer;
            }
        }

        public static bool CheckNull(this ComplexQueryItemContainer item)
        {
            if (item == null) return true;
            if (item.filterQueryContainer != null)
            {
                return false;
            }
            if (item.mustNotQueryContainer != null)
            {
                return false;
            }
            if (item.mustQueryContainer != null)
            {
                return false;
            }
            if (item.shouldQueryContainer != null)
            {
                return false;
            }

            return true;
        }
    }
}
