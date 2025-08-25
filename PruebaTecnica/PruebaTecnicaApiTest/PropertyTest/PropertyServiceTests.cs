
//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using AutoMapper;
using Commons.Dtos.Domain;
using Infraestructure.Entities;
using Infraestructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Services.Services;

namespace PruebaTecnicaApiTest.PropertyTests
{
    public class PropertyImageServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly IConfiguration _config;

        public PropertyImageServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();

            var inMemorySettings = new Dictionary<string, string> {
                {"Cache:ExpirationCacheInHours", "1"}
            };
            _config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
        }

        [Fact]
        public async Task PropertyList_ReturnsData_WhenPropertiesExist()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { IdProperty = 1, Name = "Casa", Address = "Bogotá" }
            };

            var repoMock = new Mock<IRepository<Property>>();
            repoMock.Setup(r => r.Get())
                    .ReturnsAsync(properties);

            _uowMock.Setup(u => u.GetRepository<Property>()).Returns(repoMock.Object);

            _mapperMock.Setup(m => m.Map<PropertyDto[]>(It.IsAny<List<Property>>()))
                       .Returns(new[] { new PropertyDto { IdProperty = 1, Name = "Casa" } });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyList();

            // Assert
            Assert.False(result.HasError);
            Assert.Single(result.Data);
            Assert.Equal("Casa", result.Data[0].Name);
        }

        [Fact]
        public async Task PropertyAdd_ReturnsSuccess_WhenValid()
        {
            // Arrange
            var dto = new PropertyDto { Name = "Apartamento" };

            var repoMock = new Mock<IRepository<Property>>();
            _uowMock.Setup(u => u.GetRepository<Property>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            _mapperMock.Setup(m => m.Map<Property>(It.IsAny<PropertyDto>()))
                       .Returns(new Property());

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyAdd(dto);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("Property successfully created", result.Messages);
        }

        [Fact]
        public async Task GetPropertyByPropertyId_ReturnsProperty_WhenExists()
        {
            // Arrange
            var propertyEntity = new Property { IdProperty = 1, Name = "Finca" };

            var repoMock = new Mock<IRepository<Property>>();
            repoMock.Setup(r => r.Get())
                    .ReturnsAsync(new List<Property> { propertyEntity });

            _uowMock.Setup(u => u.GetRepository<Property>()).Returns(repoMock.Object);

            _mapperMock.Setup(m => m.Map<PropertyDto[]>(It.IsAny<List<Property>>()))
                       .Returns(new[] { new PropertyDto { IdProperty = 1, Name = "Finca" } });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.GetPropertyByPropertyId(1);

            // Assert
            Assert.False(result.HasError);
            Assert.NotNull(result.Data);
            Assert.Equal("Finca", result.Data.Name);
        }

        [Fact]
        public async Task PropertyUpdate_ReturnsSuccess_WhenExists()
        {
            // Arrange
            var propertyEntity = new Property { IdProperty = 1, Name = "Lote" };
            var repoMock = new Mock<IRepository<Property>>();

            repoMock.Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<Property, bool>>>()))
                    .ReturnsAsync(new List<Property> { propertyEntity });

            _uowMock.Setup(u => u.GetRepository<Property>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            var dto = new PropertyDto { IdProperty = 1, Name = "Lote Actualizado" };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyUpdate(dto);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("Property successfully updated", result.Messages);
        }

        [Fact]
        public async Task PropertyDelete_ReturnsSuccess_WhenExists()
        {
            // Arrange
            var propertyEntity = new Property { IdProperty = 1, Name = "Oficina" };

            var repoMock = new Mock<IRepository<Property>>();
            repoMock.Setup(r => r.Get())
                    .ReturnsAsync(new List<Property> { propertyEntity });

            _uowMock.Setup(u => u.GetRepository<Property>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            _mapperMock.Setup(m => m.Map<PropertyDto[]>(It.IsAny<List<Property>>()))
                       .Returns(new[] { new PropertyDto { IdProperty = 1, Name = "Oficina" } });

            _mapperMock.Setup(m => m.Map<Property>(It.IsAny<PropertyDto>()))
                       .Returns(propertyEntity);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyDelete(1);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("Property successfully deleted", result.Messages);
        }
    }
}