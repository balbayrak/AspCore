﻿using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.General;
using AspCoreTest.Entities.Models;
using Nest;
using testbusiness.Abstract;

namespace AspCoreTest.DataSearchApi.ESProviders
{
    public class PersonElasticSearchProvider : BaseElasticSearchProvider<Person>, IElasticSearchProvider<Person>
    {
        public readonly IPersonService personService;
        public PersonElasticSearchProvider(string indexKey) : base(indexKey)
        {
            personService = DependencyResolver.Current.GetService<IPersonService>();
        }
        protected override CreateIndexDescriptor createIndexDescriptor => new CreateIndexDescriptor(indexKey)
               .Settings(t => t.NumberOfReplicas(0).NumberOfShards(1)
               .Analysis(
                   tt => tt.Analyzers(ttt => ttt.Custom("myAnalyzer", c => c.Tokenizer("keyword").Filters("lowercase", "asciifolding")))

                   .Tokenizers(tk => tk.Keyword("keyword", tkk => tkk.BufferSize(512)))))


                   .Map<Person>(m => m.AutoMap()
                   .Properties(p => p.Text(pp => pp.Name(n => n.Name)
                                                                     .Analyzer("myAnalyzer")
                                                                     .SearchAnalyzer("myAnalyzer")
                                                                     .Fields(f => f.Keyword(t => t.Name("keyword").IgnoreAbove(256)))
                                                                                      .Fielddata(true)
                                                                     )
                                                               )
                                                               )

               .Aliases(a => a.Alias(aliasKey));

        public override ServiceResult<Person[]> GetSearchableEntities()
        {
            return personService.GetSearchableEntities();
        }
    }
}
