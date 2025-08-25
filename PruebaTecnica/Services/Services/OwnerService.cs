//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using AutoMapper;
using Azure;
using Commons.Dtos.Configurations;
using Commons.Dtos.Domain;
using Infraestructure.Entities;
using Infraestructure.Interfaces;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Resources;
using System.Drawing;

namespace Services.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork unitofwork;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper _mapper;

        public OwnerService(IMapper mapper, IConfiguration _configuration, IUnitOfWork _unitofwork, IMemoryCache _memoryCache)
        {
            configuration = _configuration;
            unitofwork = _unitofwork;
            memoryCache = _memoryCache;
            _mapper = mapper;
        }



        /// <inheritdoc />
        public async Task<ResultModel<OwnerModel[]>> OwnerList()
        {
            ResultModel<OwnerModel[]> ResultModel = new ResultModel<OwnerModel[]>();

            return await memoryCache.GetOrCreateAsync(CachingList.ListOwners, async cacheEntry =>
            {
                Cache cacheSettings = configuration.GetSection("Cache").Get<Cache>() ?? new Cache { ExpirationCacheInHours = 1 };

                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(cacheSettings.ExpirationCacheInHours));
                cacheEntry.SetPriority(CacheItemPriority.Normal);


                var result = new ResultModel<OwnerModel[]>();

                try
                {
                    List<Owner> Owner = (await unitofwork.GetRepository<Owner>().Get()).ToList();


                    result.HasError = false;
                    result.Data = Owner.ToArray().Any() ? _mapper.Map<OwnerModel[]>(Owner) : Array.Empty<OwnerModel>();
                    result.Messages = "Owner listed successfully";
                    return result;
                }
                catch (Exception ex)
                {
                    return new ResultModel<OwnerModel[]>
                    {
                        HasError = true,
                        Messages = "Technical error listing Owner",
                        Data = Array.Empty<OwnerModel>(),
                        ExceptionMessage = ex.ToString()
                    };
                }
            });
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> OwnerAdd(OwnerModel ownerDto)
        {
            try
            {
                var ResultFileValidation = ValidateFile(ownerDto.File);
                if (ResultFileValidation.HasError)
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = ResultFileValidation.Messages,
                        Data = string.Empty
                    };
                }

                Owner owner = _mapper.Map<Owner>(ownerDto);
                owner.IdOwner = null;


                using (var memoryStream = new MemoryStream())
                {
                    await ownerDto.File.CopyToAsync(memoryStream);

                    owner.Photo = memoryStream.ToArray();
                    owner.ContentType = ownerDto.File.ContentType;
                }

                unitofwork.GetRepository<Owner>().Add(owner);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "Owner not created",
                        Data = string.Empty
                    };
                }

                memoryCache.Remove(CachingList.ListOwners);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "Owner successfully created",
                    Data = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error saving Owner: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<OwnerModel>> GetOwnerByOwnerId(int id)
        {
            try
            {
                ResultModel<OwnerModel[]> listResult = await OwnerList();

                if (listResult == null || listResult.HasError)
                {
                    return new ResultModel<OwnerModel>
                    {
                        HasError = true,
                        Messages = listResult?.Messages ?? "Error retrieving user list",
                        Data = null,
                        ExceptionMessage = listResult?.ExceptionMessage
                    };
                }

                OwnerModel? ownerDto = listResult.Data?.FirstOrDefault(x => x.IdOwner == id);

                return new ResultModel<OwnerModel>
                {
                    HasError = false,
                    Messages = ownerDto != null ? "User found" : "User not found",
                    Data = ownerDto
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<OwnerModel>
                {
                    HasError = true,
                    Messages = $"Technical error searching user: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }

        /// <inheritdoc />
        public async Task<ResultModel<string>> OwnerUpdate(OwnerModel ownerModel)
        {
            try
            {
                Owner? Owner = (await unitofwork.GetRepository<Owner>().Get(x => x.IdOwner == ownerModel.IdOwner)).FirstOrDefault();

                if (Owner == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "Owner not found",
                        Data = null
                    };
                }

                Owner.Name = ownerModel.Name;
                Owner.Address = ownerModel.Address;
                Owner.Birthday = ownerModel.Birthday;

                unitofwork.GetRepository<Owner>().Update(Owner);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "Owner not updated",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListOwners);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "Owner successfully updated",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error updating Owner: {ex.Message}",
                    ExceptionMessage = ex.ToString(),
                    Data = null
                };
            }
        }


        /// <inheritdoc />
        public async Task<ResultModel<string>> OwnerDelete(int ownerId)
        {
            try
            {
                ResultModel<OwnerModel> OwnerResult = await GetOwnerByOwnerId(ownerId);
                if (OwnerResult.HasError)
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = OwnerResult.Messages,
                        Data = OwnerResult.ExceptionMessage
                    };
                }

                OwnerModel? ownerDto = OwnerResult.Data;
                if (ownerDto == null)
                {
                    return new ResultModel<string>
                    {
                        HasError = false,
                        Messages = "Owner not found",
                        Data = null
                    };
                }

                Owner ownerMapper = _mapper.Map<Owner>(ownerDto);

                unitofwork.GetRepository<Owner>().Remove(ownerMapper);

                if (!unitofwork.SaveChanges())
                {
                    return new ResultModel<string>
                    {
                        HasError = true,
                        Messages = "Owner not deleted",
                        Data = null
                    };
                }

                memoryCache.Remove(CachingList.ListOwners);

                return new ResultModel<string>
                {
                    HasError = false,
                    Messages = "Owner successfully deleted",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ResultModel<string>
                {
                    HasError = true,
                    Messages = $"Technical error deleting Owner: {ex.Message}",
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
            if (!mimeTypes.Contains(Path.GetExtension(File.FileName.ToLower())))
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