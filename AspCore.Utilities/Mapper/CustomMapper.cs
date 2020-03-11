using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using AspCore.Utilities.Mapper.Concrete;

namespace AspCore.Utilities.Mapper
{

    public class CustomMapper : IDisposable
    {
        public TDestination MapProperties<TSource, TDestination>(TSource source)
            where TSource : class, new()
            where TDestination : class, new()
        {
            TDestination destination = Activator.CreateInstance<TDestination>();
            destination = SetProperties(source, destination);
            return destination;
        }

        public TDestination MapProperties<TSource, TDestination>(TDestination destination,TSource source)
            where TSource : class, new()
            where TDestination : class, new()
        {
            destination = SetProperties(source, destination);
            return destination;
        }

        private static TDestination SetProperties<TSource, TDestination>(TSource source, TDestination destination)
            where TSource : class, new()
            where TDestination : class, new()
        {
            var setterBase = new SetterBase<TSource, TDestination>();
            setterBase.SetProperties(source, destination);
            return destination;
        }

        //yeni
        public TDataSet MapProperties<TDataSet, TDataTable, TSource>(TSource source)
            where TDataSet : DataSet, new()
            where TSource : class, new()
            where TDataTable : DataTable, new()
        {
            if (source == null)
                return null;
            TDataSet ds = new TDataSet();
            DataTable dt = SetColumnValues(source);
            TDataTable stronglyTyped = ds.Tables.OfType<TDataTable>().FirstOrDefault();
            ds.EnforceConstraints = false;
            if (stronglyTyped != null) stronglyTyped.Merge(dt);
            return ds;
        }

        public DataSet MapProperties<TSource>(TSource source)
          where TSource : class, new()
        {
            if (source == null)
                return null;
            DataSet ds = new DataSet();
            DataTable dt = SetColumnValues(source);
            ds.Tables.Add(dt);
            return ds;
        }

        public List<TDestination> MapToList<TSource, TDestination>(List<TSource> source)
            where TSource : class, new()
            where TDestination : class, new()
        {
            if (source == null || (source.Count <= 0)) return null;
            List<TDestination> list = new List<TDestination>();
            foreach (TSource item in source)
            {
                TDestination newItem = MapProperties<TSource, TDestination>(item);
                list.Add(newItem);
            }
            return list;
        }
        public TDestination[] MapToArray<TSource, TDestination>(TSource[] source)
            where TSource : class, new()
            where TDestination : class, new()
        {
            if (source == null || (source.Length <= 0)) return null;
            TDestination[] list = new TDestination[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                var newItem = MapProperties<TSource, TDestination>(source[i]);
                list[i] = newItem;
            }
            return list;
        }

        public List<TDestination> MapDataSet<TDestination>(DataSet source)
        {
            PropertyDescriptorCollection fields = TypeDescriptor.GetProperties(typeof(TDestination));

            List<TDestination> lst = new List<TDestination>();

            DataTable dt = source.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<TDestination>();

                foreach (PropertyDescriptor fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            var propertyValue = dr[dc.ColumnName];

                            if (propertyValue == null || propertyValue.Equals(DBNull.Value))
                            {
                                Type dataType = fieldInfo.PropertyType;
                                propertyValue = GetNullPropertyDefaultValue(dataType);
                            }

                            try
                            {
                                fieldInfo.SetValue(ob, propertyValue);
                            }
                            catch (Exception)
                            {
                                // ignored
                            }

                            break;
                        }
                    }
                }

                lst.Add(ob);
            }
            return lst;
        }

        public TDestination MapDataSetToObj<TDestination>(DataSet source)
        {
            PropertyDescriptorCollection fields = TypeDescriptor.GetProperties(typeof(TDestination));
            var ob = Activator.CreateInstance<TDestination>();
            DataTable dt = source.Tables[0];
            foreach (PropertyDescriptor fieldInfo in fields)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    // Matching the columns with fields
                    if (fieldInfo.Name == dc.ColumnName)
                    {
                        // Get the value from the datatable cell
                        var propertyValue = dt.Rows[0][dc.ColumnName];

                        if (propertyValue == null || propertyValue.Equals(DBNull.Value))
                        {
                            Type dataType = fieldInfo.PropertyType;
                            propertyValue = GetNullPropertyDefaultValue(dataType);
                        }
                        try
                        {
                            fieldInfo.SetValue(ob, propertyValue);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        break;
                    }
                }
            }
            return ob;
        }
        public TValue GetDataSetColumnValue<TValue>(DataSet source, int rowIndex, string columnName)
        {
            object propertyValue = null;

            DataTable dt = source.Tables[0];
            DataRow dr = dt.Rows[rowIndex];
            if (dr != null)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    // Matching the columns with fields
                    if (dc.ColumnName == columnName)
                    {
                        // Get the value from the datatable cell
                        propertyValue = dr[dc.ColumnName];

                        if (propertyValue == null || propertyValue.Equals(DBNull.Value))
                        {

                            Type dataType = typeof(TValue);

                            if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                propertyValue = null;
                            }

                            if (dataType == typeof(int))
                            {
                                propertyValue = -1;
                            }

                            if (dataType == typeof(long))
                            {
                                propertyValue = -1;
                            }

                            if (dataType == typeof(short))
                            {
                                propertyValue = -1;
                            }

                            if (dataType == typeof(bool))
                            {
                                propertyValue = false;
                            }

                            if (dataType == typeof(float))
                            {
                                propertyValue = -1;
                            }

                            if (dataType == typeof(double))
                            {
                                propertyValue = -1;
                            }

                            if (dataType == typeof(decimal))
                            {
                                propertyValue = -1;
                            }

                            if (dataType == typeof(byte))
                            {
                                propertyValue = 0;
                            }

                            if (dataType == typeof(string))
                            {
                                propertyValue = null;
                            }

                            if (dataType == typeof(DateTime))
                            {
                                propertyValue = DateTime.MinValue;
                            }
                        }
                        break;
                    }
                }
            }
            TValue value = default(TValue);
            if (propertyValue != null)
            {
                value = (TValue)propertyValue;
            }
            return value;

        }

        public List<TU> MapDbRowList<TU, TRow>(List<TRow> rowList)
          where TU : class, new()
            where TRow : DataRow
        {
            if (rowList == null || rowList.Count <= 0) return null;

            PropertyDescriptorCollection fields = TypeDescriptor.GetProperties(typeof(TU));

            List<TU> lst = new List<TU>();

            foreach (TRow dr in rowList)
            {
                var ob = Activator.CreateInstance<TU>();

                foreach (PropertyDescriptor fieldInfo in fields)
                {
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            object value = dr[dc.ColumnName];

                            if (value != null && !value.Equals(DBNull.Value))
                            {
                                try
                                {
                                    fieldInfo.SetValue(ob, value);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                            break;
                        }
                    }
                }
                lst.Add(ob);
            }
            return lst;
        }

        public TU MapDbRow<TU, TRow>(TRow row)
         where TU : class, new()
           where TRow : DataRow
        {
            if (row == null) return null;

            PropertyDescriptorCollection fields = TypeDescriptor.GetProperties(typeof(TU));

            var ob = Activator.CreateInstance<TU>();

            foreach (PropertyDescriptor fieldInfo in fields)
            {
                foreach (DataColumn dc in row.Table.Columns)
                {
                    // Matching the columns with fields
                    if (fieldInfo.Name == dc.ColumnName)
                    {
                        // Get the value from the datatable cell
                        object value = row[dc.ColumnName];

                        // Set the value into the object
                        if (value != null && !value.Equals(DBNull.Value))
                        {
                            try
                            {
                                fieldInfo.SetValue(ob, value);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                        break;
                    }
                }
            }
            return ob;
        }

        public List<TDestination> MapDataTable<TDestination>(DataTable dt)
      where TDestination : class, new()
        {
            PropertyDescriptorCollection fields = TypeDescriptor.GetProperties(typeof(TDestination));

            List<TDestination> lst = new List<TDestination>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<TDestination>();

                foreach (PropertyDescriptor fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];

                            // Set the value into the object
                            if (value != null && !value.Equals(DBNull.Value))
                            {
                                try
                                {
                                    fieldInfo.SetValue(ob, value);
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }
            return lst;
        }

        public TDataSet MapList<TDataSet, TDataTable, TSource>(List<TSource> list)
            where TDataSet : DataSet, new()
            where TSource : class, new()
            where TDataTable : DataTable, new()
        {
            if (list == null || list.Count <= 0) return null;
            TDataSet ds = new TDataSet();
            DataTable dt = SetColumnValues(list);
            TDataTable stronglyTyped = ds.Tables.OfType<TDataTable>().FirstOrDefault();
            stronglyTyped?.Merge(dt);
            return ds;
        }

        public DataSet MapList<TSource>(List<TSource> list)
           where TSource : class, new()
        {
            if (list == null || list.Count <= 0) return null;
            DataSet ds = new DataSet();
            DataTable dt = SetColumnValues<TSource>(list);
            ds.Tables.Add(dt);
            return ds;
        }

        public void Dispose()
        {

        }

        private DataTable GetDataTableFromType<TSource>()
        {
            Type type = typeof(TSource);
            var properties = type.GetProperties();
            DataTable dt = new DataTable();

            foreach (PropertyInfo item in properties)
            {
                DataColumn column = new DataColumn();
                column.ColumnName = item.Name;
                Type returnType = null;
                var underlyingType = Nullable.GetUnderlyingType(item.PropertyType);
                if (underlyingType != null)
                {
                    column.DataType = underlyingType;
                    column.AllowDBNull = true;
                }
                else
                {
                    returnType = item.PropertyType;
                }
                column.DataType = returnType;

                dt.Columns.Add(column);
            }

            return dt;

        }

        private DataTable SetColumnValues<TSource>(TSource source)
        {
            DataTable dt = GetDataTableFromType<TSource>();
            Type type = typeof(TSource);
            var properties = type.GetProperties();

            dt.Rows.Add(dt.NewRow());
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].AllowDBNull = true;

                PropertyInfo property = properties.FirstOrDefault(t => t.Name == dt.Columns[i].ColumnName);

                //DTO class tablodaki bütün kolonları içermeyebilir.
                if (property != null)
                {
                    object propertyValue = GetPropertyValue(source, property);
                    dt.Rows[0][i] = propertyValue;
                }
            }


            return dt;
        }

        private DataTable SetColumnValues<TSource>(List<TSource> source)
        {
            DataTable dt = GetDataTableFromType<TSource>();
            Type type = typeof(TSource);
            var properties = type.GetProperties();

            for (int i = 0; i < source.Count; i++)
            {
                dt.Rows.Add(dt.NewRow());
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dt.Columns[j].AllowDBNull = true;

                    PropertyInfo property = properties.FirstOrDefault(t => t.Name == dt.Columns[j].ColumnName);

                    //DTO class tablodaki bütün kolonları içermeyebilir.
                    if (property != null)
                    {
                        object propertyValue = GetPropertyValue(source[i], property);
                        dt.Rows[i][j] = propertyValue;
                    }
                }
            }
            return dt;
        }

        private object GetNullPropertyDefaultValue(Type dataType)
        {
            object propertyValue = null;

            if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyValue = null;
            }

            if (dataType == typeof(int))
            {
                propertyValue = -1;
            }

            if (dataType == typeof(long))
            {
                propertyValue = -1;
            }

            if (dataType == typeof(short))
            {
                propertyValue = -1;
            }

            if (dataType == typeof(bool))
            {
                propertyValue = false;
            }

            if (dataType == typeof(float))
            {
                propertyValue = -1;
            }

            if (dataType == typeof(double))
            {
                propertyValue = -1;
            }

            if (dataType == typeof(decimal))
            {
                propertyValue = -1;
            }

            if (dataType == typeof(byte))
            {
                propertyValue = 0;
            }

            if (dataType == typeof(string))
            {
                propertyValue = null;
            }

            if (dataType == typeof(DateTime))
            {
                propertyValue = DateTime.MinValue;
            }

            return propertyValue;
        }

        private object GetPropertyValue<TSource>(TSource source, PropertyInfo property)
        {
            object propertyValue = property.GetValue(source, null);

            Type dataType = property.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(dataType);
            if (underlyingType != null)
            {
                dataType = underlyingType;
            }

            //DateTime nullable kontrolü, diğer tiplerde eklenebilir.
            if (dataType.Equals(typeof(DateTime)))
            {
                var value = Convert.ToDateTime(propertyValue);
                if (value == DateTime.MinValue || value == DateTime.MaxValue)
                    propertyValue = DBNull.Value;
            }

            return propertyValue;
        }

    }

}
