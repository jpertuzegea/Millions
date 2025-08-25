
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

namespace PruebaTecnicaApiTest
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
        public async Task PropertyImageList_ReturnsData_WhenExists()
        {
            // Arrange
            var images = new List<PropertyImage>
            {
                new PropertyImage { IdPropertyImage = 1, IdProperty = 10, Enabled = true }
            };

            var repoMock = new Mock<IRepository<PropertyImage>>();
            repoMock.Setup(r => r.Get()).ReturnsAsync(images);
            _uowMock.Setup(u => u.GetRepository<PropertyImage>()).Returns(repoMock.Object);

            _mapperMock.Setup(m => m.Map<PropertyImageDto[]>(It.IsAny<List<PropertyImage>>()))
                       .Returns(new[] { new PropertyImageDto { IdPropertyImage = 1, IdProperty = 10, Enabled = true } });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyImageService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyImageList();

            // Assert
            Assert.False(result.HasError);
            Assert.Single(result.Data);
            Assert.Equal(1, result.Data[0].IdPropertyImage);
        }

        [Fact]
        public async Task PropertyImageAdd_ReturnsSuccess_WhenFileIsValid()
        {
            // Arrange
            var dto = new PropertyImageDto
            {
                File = new FormFile(new MemoryStream(new byte[10]), 0, 10, "file", "foto.jpg")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                }
            };

            var repoMock = new Mock<IRepository<PropertyImage>>();
            _uowMock.Setup(u => u.GetRepository<PropertyImage>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            _mapperMock.Setup(m => m.Map<PropertyImage>(It.IsAny<PropertyImageDto>()))
                       .Returns(new PropertyImage());

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyImageService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyImageAdd(dto);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("PropertyImage successfully created", result.Messages);
        }

        [Fact]
        public async Task GetPropertyImageByPropertyImageId_ReturnsImage_WhenExists()
        {
            // Arrange
            var entity = new PropertyImage { IdPropertyImage = 1, IdProperty = 10, Enabled = true };

            var repoMock = new Mock<IRepository<PropertyImage>>();
            repoMock.Setup(r => r.Get()).ReturnsAsync(new List<PropertyImage> { entity });
            _uowMock.Setup(u => u.GetRepository<PropertyImage>()).Returns(repoMock.Object);

            _mapperMock.Setup(m => m.Map<PropertyImageDto[]>(It.IsAny<List<PropertyImage>>()))
                       .Returns(new[] { new PropertyImageDto { IdPropertyImage = 1, IdProperty = 10, Enabled = true } });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyImageService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.GetPropertyImageByPropertyImageId(1);

            // Assert
            Assert.False(result.HasError);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.IdPropertyImage);
        }

        [Fact]
        public async Task PropertyImageUpdate_ReturnsSuccess_WhenExists()
        {
            // Arrange
            var entity = new PropertyImage { IdPropertyImage = 1, IdProperty = 10, Enabled = false };
            var repoMock = new Mock<IRepository<PropertyImage>>();

            repoMock.Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<PropertyImage, bool>>>()))
                    .ReturnsAsync(new List<PropertyImage> { entity });

            _uowMock.Setup(u => u.GetRepository<PropertyImage>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            var dto = new PropertyImageDto { IdPropertyImage = 1, IdProperty = 10, Enabled = true };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyImageService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyImageUpdate(dto);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("PropertyImage successfully updated", result.Messages);
        }

        [Fact]
        public async Task PropertyImageDelete_ReturnsSuccess_WhenExists()
        {
            // Arrange
            var entity = new PropertyImage { IdPropertyImage = 1, IdProperty = 10, Enabled = true };

            var repoMock = new Mock<IRepository<PropertyImage>>();
            repoMock.Setup(r => r.Get()).ReturnsAsync(new List<PropertyImage> { entity });
            _uowMock.Setup(u => u.GetRepository<PropertyImage>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            _mapperMock.Setup(m => m.Map<PropertyImageDto[]>(It.IsAny<List<PropertyImage>>()))
                       .Returns(new[] { new PropertyImageDto { IdPropertyImage = 1, IdProperty = 10, Enabled = true } });

            _mapperMock.Setup(m => m.Map<PropertyImage>(It.IsAny<PropertyImageDto>()))
                       .Returns(entity);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new PropertyImageService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.PropertyImageDelete(1);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("PropertyImage successfully deleted", result.Messages);
        }
    }
}