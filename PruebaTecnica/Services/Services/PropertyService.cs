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
    public class PropertyService : IPropertyService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitofwork;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper _mapper;

        public PropertyService(IMapper mapper, IConfiguration _configuration, IUnitOfWork _unitofwork, IMemoryCache _memoryCache)
        {
            configuration = _configuration;
            unitofwork = _unitofwork;
            memoryCache = _memoryCache;
            _mapper = mapper;
        }




        /// <inheritdoc />
        public async Task<ResultModel<PropertyDto[]>> PropertyList()
        {
            ResultModel<PropertyDto[]> ResultModel = new ResultModel<PropertyDto[]>();

            return await memoryCache.GetOrCreateAsync(CachingList.ListProperty, async cacheEntry =>
            {
                Cache cacheSettings = configuration.GetSection("Cache").Get<Cache>() ?? new Cache { ExpirationCacheInHours = 1 };

                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(cacheSettings.ExpirationCacheInHours));
                cacheEntry.SetPriority(CacheItemPriority.Normal);


                var result = new ResultModel<PropertyDto[]>();

                try
                {
                    List<Property> Property = (await unitofwork.GetRepository<Property>().Get()).ToList();


                    result.HasError = false;
                    result.Data = Property.ToArray().Any() ? _mapper.Map<PropertyDto[]>(Property) : Array.Empty<PropertyDto>();
                    result.Messages = "Property listed successfully";
                    return result;
                }
                catch (Exception ex)
                {
                    return new ResultModel<PropertyDto[]>
                    {
                        HasError = true,
                        Messages = "Technical error listing Property",
                        Data = Array.Empty<PropertyDto>(),
                        ExceptionMessage = ex.ToString()
                    };
                }
            });
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyAdd(PropertyDto PropertyDto)
        {
            try
            {
                Property Property = _mapper.Map<Property>(PropertyDto);
                Property.IdProperty = null;

                unitofwork.GetRepository<Property>().Add(Property);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "Property not created",
                        Data = string.Empty
                    };
                }

                memoryCache.Remove(CachingList.ListProperty);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "Property successfully created",
                    Data = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error saving Property: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<PropertyDto>> GetPropertyByPropertyId(int id)
        {
            try
            {
                ResultModel<PropertyDto[]> listResult = await PropertyList();

                if (listResult == null || listResult.HasError)
                {
                    return new ResultModel<PropertyDto>
                    {
                        HasError = true,
                        Messages = listResult?.Messages ?? "Error retrieving user list",
                        Data = null,
                        ExceptionMessage = listResult?.ExceptionMessage
                    };
                }

                PropertyDto? PropertyDto = listResult.Data?.FirstOrDefault(x => x.IdProperty == id);

                return new ResultModel<PropertyDto>
                {
                    HasError = false,
                    Messages = PropertyDto != null ? "User found" : "User not found",
                    Data = PropertyDto
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<PropertyDto>
                {
                    HasError = true,
                    Messages = $"Technical error searching user: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyUpdate(PropertyDto PropertyModel)
        {
            try
            {
                Property? Property = (await unitofwork.GetRepository<Property>().Get(x => x.IdProperty == PropertyModel.IdProperty)).FirstOrDefault();

                if (Property == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "Property not found",
                        Data = null
                    };
                }

                Property.Name = PropertyModel.Name;
                Property.Address = PropertyModel.Address;
                Property.Price = PropertyModel.Price;
                Property.CodeInternal = PropertyModel.CodeInternal;
                Property.Year = PropertyModel.Year;
                Property.IdOwner = PropertyModel.IdOwner;

                unitofwork.GetRepository<Property>().Update(Property);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "Property not updated",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListProperty);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "Property successfully updated",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error updating Property: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }


        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyDelete(int PropertyId)
        {
            try
            {
                ResultModel<PropertyDto> PropertyResult = await GetPropertyByPropertyId(PropertyId);
                if (PropertyResult.HasError)
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = PropertyResult.Messages,
                        Data = PropertyResult.ExceptionMessage
                    };
                }

                PropertyDto? PropertyDto = PropertyResult.Data;
                if (PropertyDto == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "Property not found",
                        Data = null
                    };
                }

                Property PropertyMapper = _mapper.Map<Property>(PropertyDto);

                unitofwork.GetRepository<Property>().Remove(PropertyMapper);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "Property not deleted",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListProperty);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "Property successfully deleted",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error deleting Property: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        public async Task<ResultModel<PropertyDto[]>> SearchPropertys(SearchDto searchDto)
        {
            var result = new ResultModel<PropertyDto[]>();

            try
            {
                List<Property> properties = (await unitofwork.GetRepository<Property>().Get()).ToList();

                if (!string.IsNullOrWhiteSpace(searchDto.Name))
                {
                    properties = properties
                        .Where(p => p.Name != null && p.Name.Contains(searchDto.Name, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(searchDto.Address))
                {
                    properties = properties
                        .Where(p => p.Address != null && p.Address.Contains(searchDto.Address, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (searchDto.PriceMin.HasValue)
                {
                    properties = properties
                        .Where(p => p.Price >= searchDto.PriceMin.Value)
                        .ToList();
                }

                if (searchDto.PriceMax.HasValue)
                {
                    properties = properties
                        .Where(p => p.Price <= searchDto.PriceMax.Value)
                        .ToList();
                } 

                result.HasError = false;
                result.Data = properties.Any() ? _mapper.Map<PropertyDto[]>(properties) : Array.Empty<PropertyDto>();
                result.Messages = properties.Any()
                    ? "Properties found successfully"
                    : "No properties match the search criteria";
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Data = Array.Empty<PropertyDto>();
                result.Messages = "Technical error searching properties";
                result.ExceptionMessage = ex.ToString();
            }

            return result;
        }
    }
}