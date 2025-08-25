//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using AutoMapper;
using Commons.Dtos.Configurations;
using Commons.Dtos.Domain;
using Infraestructure.Entities;
using Infraestructure.Interfaces;
using Interfaces.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Services.Resources;

namespace Services.Services
{
    public class PropertyTraceService : IPropertyTraceService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitofwork;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper _mapper;

        public PropertyTraceService(IMapper mapper, IConfiguration _configuration, IUnitOfWork _unitofwork, IMemoryCache _memoryCache)
        {
            configuration = _configuration;
            unitofwork = _unitofwork;
            memoryCache = _memoryCache;
            _mapper = mapper;
        }



        /// <inheritdoc />
        public async Task<ResultModel<PropertyTraceDto[]>> PropertyTraceList()
        {
            ResultModel<PropertyTraceDto[]> ResultModel = new ResultModel<PropertyTraceDto[]>();

            return await memoryCache.GetOrCreateAsync(CachingList.ListPropertyTrace, async cacheEntry =>
            {
                Cache cacheSettings = configuration.GetSection("Cache").Get<Cache>() ?? new Cache { ExpirationCacheInHours = 1 };

                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(cacheSettings.ExpirationCacheInHours));
                cacheEntry.SetPriority(CacheItemPriority.Normal);


                var result = new ResultModel<PropertyTraceDto[]>();

                try
                {
                    List<PropertyTrace> PropertyTrace = (await unitofwork.GetRepository<PropertyTrace>().Get()).ToList();


                    result.HasError = false;
                    result.Data = PropertyTrace.ToArray().Any() ? _mapper.Map<PropertyTraceDto[]>(PropertyTrace) : Array.Empty<PropertyTraceDto>();
                    result.Messages = "PropertyTrace listed successfully";
                    return result;
                }
                catch (Exception ex)
                {
                    return new ResultModel<PropertyTraceDto[]>
                    {
                        HasError = true,
                        Messages = "Technical error listing PropertyTrace",
                        Data = Array.Empty<PropertyTraceDto>(),
                        ExceptionMessage = ex.ToString()
                    };
                }
            });
        }
        /// <inheritdoc />
        public async Task<ResultModel<PropertyTraceDto[]>> PropertyTraceListByPropertyId(int PropertyId)
        {
            ResultModel<PropertyTraceDto[]> ResultModel = new ResultModel<PropertyTraceDto[]>();

            return await memoryCache.GetOrCreateAsync(CachingList.ListPropertyTrace, async cacheEntry =>
            {
                Cache cacheSettings = configuration.GetSection("Cache").Get<Cache>() ?? new Cache { ExpirationCacheInHours = 1 };

                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(cacheSettings.ExpirationCacheInHours));
                cacheEntry.SetPriority(CacheItemPriority.Normal);


                var result = new ResultModel<PropertyTraceDto[]>();

                try
                {
                    List<PropertyTrace> ListPropertyTrace = (await unitofwork.GetRepository<PropertyTrace>().Get(x => x.IdProperty == PropertyId)).ToList();


                    result.HasError = false;
                    result.Data = ListPropertyTrace.ToArray().Any() ? _mapper.Map<PropertyTraceDto[]>(ListPropertyTrace) : Array.Empty<PropertyTraceDto>();
                    result.Messages = "PropertyTrace listed successfully";
                    return result;
                }
                catch (Exception ex)
                {
                    return new ResultModel<PropertyTraceDto[]>
                    {
                        HasError = true,
                        Messages = "Technical error listing PropertyTrace",
                        Data = Array.Empty<PropertyTraceDto>(),
                        ExceptionMessage = ex.ToString()
                    };
                }
            });
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyTraceAdd(PropertyTraceDto PropertyTraceDto)
        {
            try
            {
                PropertyTrace PropertyTrace = _mapper.Map<PropertyTrace>(PropertyTraceDto);
                PropertyTrace.IdPropertyTrace = null;

                unitofwork.GetRepository<PropertyTrace>().Add(PropertyTrace);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "PropertyTrace not created",
                        Data = string.Empty
                    };
                }

                memoryCache.Remove(CachingList.ListPropertyTrace);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "PropertyTrace successfully created",
                    Data = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error saving PropertyTrace: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<PropertyTraceDto>> GetPropertyTraceByPropertyTraceId(int id)
        {
            try
            {
                ResultModel<PropertyTraceDto[]> listResult = await PropertyTraceList();

                if (listResult == null || listResult.HasError)
                {
                    return new ResultModel<PropertyTraceDto>
                    {
                        HasError = true,
                        Messages = listResult?.Messages ?? "Error retrieving trace list",
                        Data = null,
                        ExceptionMessage = listResult?.ExceptionMessage
                    };
                }

                PropertyTraceDto? PropertyTraceDto = listResult.Data?.FirstOrDefault(x => x.IdPropertyTrace == id);

                return new ResultModel<PropertyTraceDto>
                {
                    HasError = false,
                    Messages = PropertyTraceDto != null ? "trace found" : "User not found",
                    Data = PropertyTraceDto
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<PropertyTraceDto>
                {
                    HasError = true,
                    Messages = $"Technical error searching user: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyTraceUpdate(PropertyTraceDto PropertyTraceDto)
        {
            try
            {
                PropertyTrace? PropertyTrace = (await unitofwork.GetRepository<PropertyTrace>().Get(x => x.IdPropertyTrace == PropertyTraceDto.IdPropertyTrace)).FirstOrDefault();

                if (PropertyTrace == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "PropertyTrace not found",
                        Data = null
                    };
                }

                PropertyTrace.DateSale = PropertyTraceDto.DateSale; 
                PropertyTrace.Name = PropertyTraceDto.Name; 
                PropertyTrace.Value = PropertyTraceDto.Value; 
                PropertyTrace.Tax = PropertyTraceDto.Tax; 
                PropertyTrace.IdProperty = PropertyTraceDto.IdProperty;


                unitofwork.GetRepository<PropertyTrace>().Update(PropertyTrace);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "PropertyTrace not updated",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListPropertyTrace);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "PropertyTrace successfully updated",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error updating PropertyTrace: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }


        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyTraceDelete(int PropertyTraceId)
        {
            try
            {
                ResultModel<PropertyTraceDto> PropertyTraceResult = await GetPropertyTraceByPropertyTraceId(PropertyTraceId);
                if (PropertyTraceResult.HasError)
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = PropertyTraceResult.Messages,
                        Data = PropertyTraceResult.ExceptionMessage
                    };
                }

                PropertyTraceDto? PropertyTraceDto = PropertyTraceResult.Data;
                if (PropertyTraceDto == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "PropertyTrace not found",
                        Data = null
                    };
                }

                PropertyTrace PropertyTraceMapper = _mapper.Map<PropertyTrace>(PropertyTraceDto);

                unitofwork.GetRepository<PropertyTrace>().Remove(PropertyTraceMapper);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "PropertyTrace not deleted",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListPropertyTrace);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "PropertyTrace successfully deleted",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error deleting PropertyTrace: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }


    }
}