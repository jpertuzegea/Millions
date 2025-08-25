
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

namespace PruebaTecnicaApiTest.OwnerTests
{
    public class PropertyServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly IConfiguration _config;
        private readonly OwnerService _service;

        public PropertyServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _uowMock = new Mock<IUnitOfWork>();
            _cacheMock = new Mock<IMemoryCache>();

            var inMemorySettings = new Dictionary<string, string> {
                {"Cache:ExpirationCacheInHours", "1"}
            };
            _config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

            _service = new OwnerService(_mapperMock.Object, _config, _uowMock.Object, _cacheMock.Object);
        }


        [Fact]
        public async Task OwnerList_ReturnsData_WhenOwnersExist()
        {
            // Arrange
            var owners = new List<Owner> { new Owner { IdOwner = 1, Name = "Jorge", Address = "Bogotá" } };

            var repoMock = new Mock<IRepository<Owner>>();
            repoMock.Setup(r => r.Get())
                    .ReturnsAsync(owners);

            _uowMock.Setup(u => u.GetRepository<Owner>()).Returns(repoMock.Object);

            _mapperMock.Setup(m => m.Map<OwnerModel[]>(It.IsAny<List<Owner>>()))
                       .Returns(new[] { new OwnerModel { IdOwner = 1, Name = "Jorge" } });


            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new OwnerService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.OwnerList();

            // Assert
            Assert.False(result.HasError);
            Assert.Single(result.Data);
            Assert.Equal("Jorge", result.Data[0].Name);
        }


        [Fact]
        public async Task OwnerAdd_ReturnsSuccess_WhenValidFile()
        {
            // Arrange
            var dto = new OwnerModel
            {
                Name = "Carlos",
                File = new FormFile(new MemoryStream(new byte[10]), 0, 10, "file", "foto.jpg")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                }
            };

            var repoMock = new Mock<IRepository<Owner>>();
            _uowMock.Setup(u => u.GetRepository<Owner>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            _mapperMock.Setup(m => m.Map<Owner>(It.IsAny<OwnerModel>()))
                       .Returns(new Owner());

            // Act
            var result = await _service.OwnerAdd(dto);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("Owner successfully created", result.Messages);
        }

        [Fact]
        public async Task GetOwnerByOwnerId_ReturnsOwner_WhenExists()
        {
            // Arrange
            var ownerEntity = new Owner { IdOwner = 1, Name = "Ana" };

            var repoMock = new Mock<IRepository<Owner>>();
            repoMock.Setup(r => r.Get())
                    .ReturnsAsync(new List<Owner> { ownerEntity });

            _uowMock.Setup(u => u.GetRepository<Owner>()).Returns(repoMock.Object);

            _mapperMock.Setup(m => m.Map<OwnerModel[]>(It.IsAny<List<Owner>>()))
                       .Returns(new[] { new OwnerModel { IdOwner = 1, Name = "Ana" } });


            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new OwnerService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.GetOwnerByOwnerId(1);

            // Assert
            Assert.False(result.HasError);
            Assert.NotNull(result.Data);
            Assert.Equal("Ana", result.Data.Name);
        }

        [Fact]
        public async Task OwnerUpdate_ReturnsSuccess_WhenOwnerExists()
        {
            // Arrange
            var ownerEntity = new Owner { IdOwner = 1, Name = "Pedro" };
            var repoMock = new Mock<IRepository<Owner>>();

            repoMock.Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<Owner, bool>>>()))
                    .ReturnsAsync(new List<Owner> { ownerEntity });

            _uowMock.Setup(u => u.GetRepository<Owner>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            var dto = new OwnerModel { IdOwner = 1, Name = "Pedro Updated" };

            // Act
            var result = await _service.OwnerUpdate(dto);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("Owner successfully updated", result.Messages);
        }

        [Fact]
        public async Task OwnerDelete_ReturnsSuccess_WhenOwnerExists()
        {
            // Arrange
            var ownerEntity = new Owner { IdOwner = 1, Name = "Mario" };

            var repoMock = new Mock<IRepository<Owner>>();
            repoMock.Setup(r => r.Get())
                    .ReturnsAsync(new List<Owner> { ownerEntity });

            _uowMock.Setup(u => u.GetRepository<Owner>()).Returns(repoMock.Object);
            _uowMock.Setup(u => u.SaveChanges()).Returns(true);

            _mapperMock.Setup(m => m.Map<OwnerModel[]>(It.IsAny<List<Owner>>()))
                       .Returns(new[] { new OwnerModel { IdOwner = 1, Name = "Mario" } });

            _mapperMock.Setup(m => m.Map<Owner>(It.IsAny<OwnerModel>()))
                       .Returns(ownerEntity);

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new OwnerService(_mapperMock.Object, _config, _uowMock.Object, memoryCache);

            // Act
            var result = await service.OwnerDelete(1);

            // Assert
            Assert.False(result.HasError);
            Assert.Equal("Owner successfully deleted", result.Messages);
        }
    }
}
