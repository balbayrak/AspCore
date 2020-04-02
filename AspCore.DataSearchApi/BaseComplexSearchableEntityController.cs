using AspCore.Business.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Convertors;
using AspCore.Dependency.Concrete;
using AspCore.ElasticSearch.Abstract;
using AspCore.ElasticSearch.Configuration;
using AspCore.ElasticSearch.General;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using AspCore.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.DataSearchApi
{
    public abstract class BaseComplexSearchableEntityController<TEntity, TSearchableEntity, TSearchableEntityService> : BaseElasticSearchController<TSearchableEntity>
        where TEntity : class, IEntity, new()
        where TSearchableEntity : class, ISearchableEntity, new()
        where TSearchableEntityService : IComplexSearchableEntityService<TEntity, TSearchableEntity>
    {

        protected IComplexSearchableEntityService<TEntity, TSearchableEntity> _searchableEntityService { get; }

        public BaseComplexSearchableEntityController()
        {
            _searchableEntityService = DependencyResolver.Current.GetService<IComplexSearchableEntityService<TEntity,TSearchableEntity>>();
        }


        [NonAction]
        protected override IActionResult CreateIndex(InitIndexRequest initRequest)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                ServiceResult<bool> existResult = _context.ExistIndex(initRequest.indexKey);
                if (existResult.IsSucceeded)
                {
                    if (existResult.Result)
                    {
                        ServiceResult<bool> deleteResult = _context.DeleteIndex(initRequest.indexKey);
                        if (!(deleteResult.IsSucceeded && deleteResult.Result))
                        {
                            result.ErrorMessage = deleteResult.ErrorMessage;
                            result.ExceptionMessage = deleteResult.ExceptionMessage;
                        }

                        if (!string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            ServiceResult<bool> createResult = _context.CreateIndex(createIndexDescriptor);
                            if (createResult.IsSucceeded && createResult.Result)
                            {
                                if (initRequest.initializeWithData)
                                {
                                    ServiceResult<TSearchableEntity[]> entityResult = _searchableEntityService.GetSearchableEntities();
                                    if (entityResult.IsSucceededAndDataIncluded())
                                    {
                                        result = _context.BulkIndex(GetIndexAliasName(), entityResult.Result.ToList());
                                    }
                                    else
                                    {
                                        result.ErrorMessage = entityResult.ErrorMessage;
                                        result.ExceptionMessage = entityResult.ExceptionMessage;
                                    }
                                }
                            }
                            else
                            {
                                result.ErrorMessage = createResult.ErrorMessage;
                                result.ExceptionMessage = createResult.ExceptionMessage;
                            }
                        }
                    }
                }
                else
                {
                    result.ErrorMessage = existResult.ErrorMessage;
                    result.ExceptionMessage = existResult.ExceptionMessage;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result.ToHttpResponse();
        }

    }
}
