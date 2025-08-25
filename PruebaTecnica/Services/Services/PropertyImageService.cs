
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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Services.Resources;

namespace Services.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitofwork;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper _mapper;

        public PropertyImageService(IMapper mapper, IConfiguration _configuration, IUnitOfWork _unitofwork, IMemoryCache _memoryCache)
        {
            configuration = _configuration;
            unitofwork = _unitofwork;
            memoryCache = _memoryCache;
            _mapper = mapper;
        }



        /// <inheritdoc />
        public async Task<ResultModel<PropertyImageDto[]>> PropertyImageList()
        {
            ResultModel<PropertyImageDto[]> ResultModel = new ResultModel<PropertyImageDto[]>();

            return await memoryCache.GetOrCreateAsync(CachingList.ListPropertyImage, async cacheEntry =>
            {
                Cache cacheSettings = configuration.GetSection("Cache").Get<Cache>() ?? new Cache { ExpirationCacheInHours = 1 };

                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(cacheSettings.ExpirationCacheInHours));
                cacheEntry.SetPriority(CacheItemPriority.Normal);


                var result = new ResultModel<PropertyImageDto[]>();

                try
                {
                    List<PropertyImage> Result = (await unitofwork.GetRepository<PropertyImage>().Get()).ToList();


                    result.HasError = false;
                    result.Data = Result.ToArray().Any() ? _mapper.Map<PropertyImageDto[]>(Result) : Array.Empty<PropertyImageDto>();
                    result.Messages = "PropertyImage listed successfully";
                    return result;
                }
                catch (Exception ex)
                {
                    return new ResultModel<PropertyImageDto[]>
                    {
                        HasError = true,
                        Messages = "Technical error listing PropertyImage",
                        Data = Array.Empty<PropertyImageDto>(),
                        ExceptionMessage = ex.ToString()
                    };
                }
            });
        }




        /// <inheritdoc />
        public async Task<ResultModel<PropertyImageDto[]>> PropertyImageListByPropertyImageId(int PropertyId)
        {
            ResultModel<PropertyImageDto[]> ResultModel = new ResultModel<PropertyImageDto[]>();

            var result = new ResultModel<PropertyImageDto[]>();

            try
            {
                List<PropertyImage> Owner = (await unitofwork.GetRepository<PropertyImage>().Get(x => x.IdProperty == PropertyId)).ToList();


                result.HasError = false;
                result.Data = Owner.ToArray().Any() ? _mapper.Map<PropertyImageDto[]>(Owner) : Array.Empty<PropertyImageDto>();
                result.Messages = "PropertyImage listed successfully";
                return result;
            }
            catch (Exception ex)
            {
                return new ResultModel<PropertyImageDto[]>
                {
                    HasError = true,
                    Messages = "Technical error listing PropertyImage",
                    Data = Array.Empty<PropertyImageDto>(),
                    ExceptionMessage = ex.ToString()
                };
            }

        }


        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyImageAdd(PropertyImageDto PropertyImageDto)
        {
            try
            {
                var ResultFileValidation = ValidateFile(PropertyImageDto.File);
                if (ResultFileValidation.HasError)
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = ResultFileValidation.Messages,
                        Data = string.Empty
                    };
                }

                PropertyImage PropertyImage = _mapper.Map<PropertyImage>(PropertyImageDto);
                PropertyImage.IdPropertyImage = null;


                using (var memoryStream = new MemoryStream())
                {
                    await PropertyImageDto.File.CopyToAsync(memoryStream);

                    PropertyImage.Photo = memoryStream.ToArray();
                    PropertyImage.ContentType = PropertyImageDto.File.ContentType;
                }

                unitofwork.GetRepository<PropertyImage>().Add(PropertyImage);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "PropertyImage not created",
                        Data = string.Empty
                    };
                }

                memoryCache.Remove(CachingList.ListOwners);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "PropertyImage successfully created",
                    Data = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error saving PropertyImage: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<PropertyImageDto>> GetPropertyImageByPropertyImageId(int id)
        {
            try
            {
                ResultModel<PropertyImageDto[]> listResult = await PropertyImageList();

                if (listResult == null || listResult.HasError)
                {
                    return new ResultModel<PropertyImageDto>
                    {
                        HasError = true,
                        Messages = listResult?.Messages ?? "Error retrieving PropertyImage list",
                        Data = null,
                        ExceptionMessage = listResult?.ExceptionMessage
                    };
                }

                PropertyImageDto? PropertyImageDto = listResult.Data?.FirstOrDefault(x => x.IdPropertyImage == id);

                return new ResultModel<PropertyImageDto>
                {
                    HasError = false,
                    Messages = PropertyImageDto != null ? "PropertyImage found" : "PropertyImage not found",
                    Data = PropertyImageDto
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<PropertyImageDto>
                {
                    HasError = true,
                    Messages = $"Technical error searching PropertyImage: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyImageUpdate(PropertyImageDto PropertyImageModel)
        {
            try
            {
                PropertyImage? PropertyImage = (await unitofwork.GetRepository<PropertyImage>().Get(x => x.IdPropertyImage == PropertyImageModel.IdPropertyImage)).FirstOrDefault();

                if (PropertyImage == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "PropertyImage not found",
                        Data = null
                    };
                }

                PropertyImage.IdProperty = PropertyImageModel.IdProperty;
                PropertyImage.Enabled = PropertyImageModel.Enabled;

                unitofwork.GetRepository<PropertyImage>().Update(PropertyImage);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "PropertyImage not updated",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListOwners);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "PropertyImage successfully updated",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error updating PropertyImage: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }


        /// <inheritdoc />
        public async Task<ResultModel<string>> PropertyImageDelete(int PropertyImageId)
        {
            try
            {
                ResultModel<PropertyImageDto> PropertyImageResult = await GetPropertyImageByPropertyImageId(PropertyImageId);
                if (PropertyImageResult.HasError)
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = PropertyImageResult.Messages,
                        Data = PropertyImageResult.ExceptionMessage
                    };
                }

                PropertyImageDto? PropertyImageDto = PropertyImageResult.Data;
                if (PropertyImageDto == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "PropertyImage not found",
                        Data = null
                    };
                }

                PropertyImage PropertyImageMapper = _mapper.Map<PropertyImage>(PropertyImageDto);

                unitofwork.GetRepository<PropertyImage>().Remove(PropertyImageMapper);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "PropertyImage not deleted",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListOwners);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "PropertyImage successfully deleted",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error deleting PropertyImage: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }


        private ResultModel<string> ValidateFile(IFormFile? File)
        {
            if (File == null)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = "Debe seleccionar un documento valido",
                    Data = string.Empty
                };
            }

            string[] mimeTypes = new string[] { ".jpeg", ".jpg", ".png" };
            if (!mimeTypes.Contains(Path.GetExtension(File.FileName)))
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"La extension del documento [{Path.GetExtension(File.FileName)}] no esta permitida",
                    Data = string.Empty
                };
            }

            double Size_Kb = (File.Length / 1024.0);
            if (Size_Kb > 20000)// 20 Megas max
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = "Tamaño del documento supera maximo permitido",
                    Data = string.Empty
                };
            }

            return new ResultModel<string>
            {
                HasError = false,
                Messages = "Documento Valido",
                Data = string.Empty
            };

        }


    }
}