//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------


using AutoMapper;
using Commons.Dtos.Domain;
using Infraestructure.Entities;

namespace Services.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Owner -> OwnerDto
            CreateMap<Owner, OwnerModel>()
                .ForMember(dest => dest.File, opt => opt.Ignore()); // No se puede mapear directo byte[] a IFormFile


            // OwnerDto -> Owner
            CreateMap<OwnerModel, Owner>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.Properties, opt => opt.Ignore()); // Si no necesitas mapear las propiedades




            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name));

            CreateMap<PropertyDto, Property>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.PropertyImages, opt => opt.Ignore())
                .ForMember(dest => dest.PropertyTraces, opt => opt.Ignore());


            // DTO -> Entidad
            CreateMap<PropertyImageDto, PropertyImage>()
                 .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.FileName,
                    opt => opt.MapFrom(src => src.File != null ? src.File.FileName : src.FileName))
                .ForMember(dest => dest.ContentType,
                    opt => opt.MapFrom(src => src.File != null ? src.File.ContentType : src.ContentType));

            // Entidad -> DTO
            CreateMap<PropertyImage, PropertyImageDto>()
                .ForMember(dest => dest.File, opt => opt.Ignore()); // IFormFile no se puede mapear desde DB


            // DTO -> Entity
            CreateMap<PropertyTraceDto, PropertyTrace>()
                .ForMember(dest => dest.Property, opt => opt.Ignore());
            // ignoramos la navegación porque viene del Id

            // Entity -> DTO
            CreateMap<PropertyTrace, PropertyTraceDto>()
                .ForMember(dest => dest.IdPropertyName, opt => opt.MapFrom(src => src.Property != null ? src.Property.Name : null));





        }
    }
}
