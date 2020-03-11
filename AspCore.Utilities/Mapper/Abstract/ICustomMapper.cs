using AspCore.Dependency.Abstract;
using System;
using System.Collections.Generic;
using System.Data;

namespace AspCore.Utilities.Mapper.Abstract
{
    public interface ICustomMapper:ISingletonType,IDisposable
    {
        TDestination MapProperties<TSource, TDestination>(TSource source)
            where TSource : class, new()
            where TDestination : class, new();

        TDestination MapProperties<TSource, TDestination>(TSource source, TDestination destination)
            where TSource : class, new()
            where TDestination : class, new();

        TDataSet MapProperties<TDataSet, TDataTable, TSource>(TSource source)
            where TDataSet : DataSet, new()
            where TSource : class, new()
            where TDataTable : DataTable, new();

        DataSet MapProperties<TSource>(TSource source)
            where TSource : class, new();

        List<TDestination> MapToList<TSource, TDestination>(List<TSource> source)
            where TSource : class, new()
            where TDestination : class, new();

        List<TDestination> MapToList<TSource, TDestination>(List<TSource> source, List<TDestination> destination)
            where TSource : class, new()
            where TDestination : class, new();
        TDestination[] MapToArray<TSource, TDestination>(TSource[] source)
            where TSource : class, new()
            where TDestination : class, new();

        List<TDestination> MapDataSet<TDestination>(DataSet source);

        TDestination MapDataSetToObj<TDestination>(DataSet source);

        TValue GetDataSetColumnValue<TValue>(DataSet source, int rowIndex, string columnName);

        List<TDestination> MapDbRowList<TDestination, TRow>(List<TRow> rowList)
            where TDestination : class, new()
            where TRow : DataRow;

        TDestination MapDbRow<TDestination, TRow>(TRow row)
            where TDestination : class, new()
            where TRow : DataRow;

        List<TDestination> MapDataTable<TDestination>(DataTable dt)
            where TDestination : class, new();

        TDataSet MapListToDataSet<TDataSet, TDataTable, TSource>(List<TSource> list)
            where TDataSet : DataSet, new()
            where TSource : class, new()
            where TDataTable : DataTable, new();

        DataSet MapListToDataSet<TSource>(List<TSource> list)
            where TSource : class, new();

    }
}
