﻿using AspCore.CacheAccess.QueryContiner.Concrete;
using System.ComponentModel.DataAnnotations;

namespace AspCore.CacheAccess.QueryBuilder.Concrete
{
    public class SearchRequestItem
    {
        public FilterSource sourceFilter;
        public SortItem sortItem;
        [Required]
        public string cacheName;
        [Required]
        public int size;
        public int from;
        [Required]
        public ComplexQueryItemContainer queryContainer;
        public ComplexQueryItemContainer postFilterQueryContainer;
        public string IdFieldPropertyName;
    }
}
