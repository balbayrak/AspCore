using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.Expression;
using AspCore.Utilities;
using AspCore.WebComponents.HtmlHelpers.DataTable.Columns.Buttons;
using AspCore.WebComponents.HtmlHelpers.DataTable.Storage;
using AspCore.WebComponents.HtmlHelpers.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnaryExpression = System.Linq.Expressions.UnaryExpression;

namespace AspCore.WebComponents.HtmlHelpers.Extensions
{
    public static class DatatableExt
    {
        public static string Serialize<TEntity>(this DatatableStorageObject<TEntity> storageObject) where TEntity : class
        {
            string seralized = JsonConvert.SerializeObject(storageObject);
            seralized = seralized.Replace("\"", HelperConstant.General.SPLIT_CHAR1.ToString());
            seralized = seralized.EncrpytDecryptDataTableData(true).CompressString();
            return seralized;
        }

        public static DatatableStorageObject<TEntity> DeSerialize<TEntity>(this string serializedStorageObject) where TEntity : class
        {

            serializedStorageObject = serializedStorageObject.UnCompressString();
            serializedStorageObject = serializedStorageObject.Replace(HelperConstant.General.SPLIT_CHAR1.ToString(), "\"");
            serializedStorageObject = serializedStorageObject.EncrpytDecryptDataTableData(false);
            DatatableStorageObject<TEntity> storageObj = JsonConvert.DeserializeObject<DatatableStorageObject<TEntity>>(serializedStorageObject);
            return storageObj;
        }

        private static string EncrpytDecryptDataTableData(this string compressedString, bool isEncrpytion)
        {
            Dictionary<string, string> encryptionDict = new Dictionary<string, string>();

            encryptionDict.Add("<a class=", "é1");
            encryptionDict.Add("data-blockui=", "é2");
            encryptionDict.Add("data-target=", "é3");
            encryptionDict.Add("data-target-body=", "é4");
            encryptionDict.Add("data-target-url=", "é5");
            encryptionDict.Add("data-toggle=", "é6");
            encryptionDict.Add("<i class=", "é7");
            encryptionDict.Add("showCancelButton:", "é8");
            encryptionDict.Add("confirmButtonColor:", "é9");

            encryptionDict.Add("cancelButtonColor:", "éa0");
            encryptionDict.Add("confirmButtonText:", "éa1");
            encryptionDict.Add("closeOnConfirm:", "éa2");
            encryptionDict.Add("BlockFunc.showSpinnerBlock();", "éa3");
            encryptionDict.Add("BlockFunc.closeSpinnerBlock();", "éa4");
            encryptionDict.Add("url:decodeURIComponent(", "éa5");
            encryptionDict.Add("dataTablesDictionary", "éa6");
            encryptionDict.Add("columnIsPrimaryKey", "éa7");
            encryptionDict.Add("column_Property_Exp", "éa9");


            encryptionDict.Add("orderByDirection", "éb1");
            encryptionDict.Add("location.reload();", "éb2");
            encryptionDict.Add("if(isconfirm)", "éb3");
            encryptionDict.Add("$.ajax(", "éb4");
            encryptionDict.Add("function(isconfirm)", "éb5");
            encryptionDict.Add("data-evet-httpmethod=", "éb6");

            encryptionDict.Add("DatatableProperties", "éb7");
            encryptionDict.Add("DatatableActions", "éb8");

            encryptionDict.Add("event.preventDefault();", "éc1");
            encryptionDict.Add("cancelButtonText:", "éc2");
            encryptionDict.Add("success:function()", "éc3");
            encryptionDict.Add("error:function()", "éc4");
            encryptionDict.Add("return false;", "éc5");
            encryptionDict.Add("ActionColumnHeader", "éc6");

            encryptionDict.Add("columnProperty", "éc7");
            encryptionDict.Add("onclick=", "éc8");

            encryptionDict.Add("title:", "éc9");
            encryptionDict.Add("text:", "éc9");
            encryptionDict.Add("type:", "éd1");
            encryptionDict.Add("warning", "éd2");
            encryptionDict.Add("Confirm.showConfirm", "éd3");
            encryptionDict.Add("searchable", "éd4");
            encryptionDict.Add("ActionColumn", "éd5");
            encryptionDict.Add("mt-checkbox mt-checkbox-single mt-checkbox-outline", "éd6");

            encryptionDict.Add("_Link_Modal_Body", "éd7");
            encryptionDict.Add("checkbox", "éd8");
            encryptionDict.Add("Chk_Actions", "éd9");

            encryptionDict.Add("checkboxRowid", "ée1");
            encryptionDict.Add("checkboxes", "ée2");
            encryptionDict.Add("_Link_Modal", "ée3");
            encryptionDict.Add("<input class=", "ée4");
            encryptionDict.Add("Actions_", "ée5");
            encryptionDict.Add("btn-blockui-modal", "ée6");
            encryptionDict.Add("conditions", "ée7");
            encryptionDict.Add("property", "ée8");
            encryptionDict.Add("value", "ée9");


            foreach (var key in encryptionDict.Keys)
            {
                if (isEncrpytion)
                    compressedString = compressedString.Replace(key, encryptionDict[key]);
                else
                    compressedString = compressedString.Replace(encryptionDict[key], key);
            }

            return compressedString;
        }

        public static string GetSearchableColumnString<TEntity>(this DatatableStorageObject<TEntity> storageObject)
   where TEntity : class
        {
            var list = storageObject.DatatableProperties.Where(t => t.searchable != null);
            List<string> result = new List<string>();
            foreach (var item in list)
            {
                var propertyName = item.column_Property_Exp.Substring(item.column_Property_Exp.IndexOf('.') + 1);
                result.Add(string.Join(CoreConstants.General.SPLIT_CHAR, item.searchable, propertyName));
            }

            if (result.Count > 0)
            {
                if (result.Count > 1)
                {
                    return string.Join(CoreConstants.General.SPLIT_CHAR1, result);
                }
                else
                    return result[0];
            }

            return string.Empty;
        }

        public static Condition ToCondition<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression, object value)
        {
            Condition condition = new Condition();
            string memberStr = (expression.Body as MemberExpression).ToString();
            condition.property = memberStr;
            condition.value = value;
            return condition;
        }

        public static Condition ToCondition<TModel>(this Expression<Func<TModel, bool>> expression)
        {
            Condition condition = new Condition();
         
            if (expression.Body is BinaryExpression body)
            {
                condition.property = body.GetProperty();
                condition.IsEqual = body.NodeType == ExpressionType.Equal;
                condition.value = (body.Right as ConstantExpression).Value.ToString();
            }
            return condition;
        }

        private static string GetProperty(this Expression expression)
        {
            string property = string.Empty;
            if (expression is BinaryExpression body)
            {
                if (body.Left is UnaryExpression)
                {
                    property = ((UnaryExpression)body.Left).Operand.ToString();
                }
                else
                {
                    property = (body.Left as MemberExpression).ToString();
                }
            }
            return property;
        }
    }
}
