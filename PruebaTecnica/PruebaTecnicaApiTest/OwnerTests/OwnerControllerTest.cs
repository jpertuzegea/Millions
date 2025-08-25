
//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Agosto 2025</date>
//-----------------------------------------------------------------------

using Commons.Dtos.Configurations;
using Commons.Dtos.Domain;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PruebaTecnicaApi.Controllers;

namespace PruebaTecnicaApiTest.OwnerTests
{
    public class OwnerControllerTests
    {
        private readonly Mock<IOwnerService> _serviceMock;
        private readonly OwnerController _controller;

        public OwnerControllerTests()
        {
            _serviceMock = new Mock<IOwnerService>();
            _controller = new OwnerController(_serviceMock.Object);
        }

        [Fact]
        public async Task OwnerList_ReturnsOkResult()
        {
            // Arrange
            var expected = new ResultModel<OwnerModel[]>
            {
                HasError = false,
                Data = new[] { new OwnerModel { IdOwner = 1, Name = "Jorge" } }
            };

            _serviceMock.Setup(s => s.OwnerList()).ReturnsAsync(expected);

            // Act
            var result = await _controller.Owner();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ResultModel<OwnerModel[]>>>(result);
            var value = Assert.IsType<ResultModel<OwnerModel[]>>(actionResult.Value);
            Assert.False(value.HasError);
            Assert.Single(value.Data);
        }
    }
}
