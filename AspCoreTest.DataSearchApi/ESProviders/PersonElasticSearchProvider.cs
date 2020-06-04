using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Concrete;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.SearchableEntities;
using Nest;
using System;
using testbusiness.Abstract;

namespace AspCoreTest.DataSearchApi.ESProviders
{
    public class PersonElasticSearchProvider : BaseElasticSearchProvider<PersonSearchEntity, IPersonSearchEntityService>, IElasticSearchProvider<PersonSearchEntity>
    {
        public PersonElasticSearchProvider(IServiceProvider serviceProvider, string indexKey) : base(serviceProvider, indexKey)
        {

        }
        protected override CreateIndexDescriptor createIndexDescriptor => new CreateIndexDescriptor(indexKey)
               .Settings(t => t.NumberOfReplicas(0).NumberOfShards(1)
               .Analysis(
                   tt => tt.Analyzers(ttt => ttt.Custom("myAnalyzer", c => c.Tokenizer("keyword").Filters("lowercase", "asciifolding")))

                   .Tokenizers(tk => tk.Keyword("keyword", tkk => tkk.BufferSize(512)))))


                   .Map<PersonSearchEntity>(m => m.AutoMap()
                   .Properties(p => p.Text(pp => pp.Name(n => n.Name)
                                                                     .Analyzer("myAnalyzer")
                                                                     .SearchAnalyzer("myAnalyzer")
                                                                     .Fields(f => f.Keyword(t => t.Name("keyword").IgnoreAbove(256)))
                                                                                      .Fielddata(true)
                                                                     )
                   .Text(pp => pp.Name(n => n.Surname)
                                                                     .Analyzer("myAnalyzer")
                                                                     .SearchAnalyzer("myAnalyzer")
                                                                     .Fields(f => f.Keyword(t => t.Name("keyword").IgnoreAbove(256)))
                                                                                      .Fielddata(true)
                                                                     )
                                                               )
                                                               )

               .Aliases(a => a.Alias(aliasKey));

    }
}
