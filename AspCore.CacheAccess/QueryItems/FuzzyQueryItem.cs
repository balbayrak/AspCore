using AspCore.CacheAccess.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheAccess.QueryItems
{
    /// <summary>
    /// Fuzzy Query aranan ifadeyi analiz eder, ifadeye yakın sonuçları return eder.
    /// </summary>
    /// <typeparam name="T">item sinifi</typeparam>
    public class FuzzyQueryItem : QueryItem
    {
        public string Value { get; set; }
        public int? PrefixLength { get; set; }
        public int? Max_Expansions { get; set; }
        public bool? Transpositions { get; set; }

        public FuzzyQueryItem(string fieldDescriptor, string value, int? prefixLength, int? maxExpansions, bool? transpositions) : base(fieldDescriptor)
        {
            this.Value = value;
            this.PrefixLength = prefixLength;
            this.Max_Expansions = maxExpansions;
            this.Transpositions = transpositions;
        }
    }
}
