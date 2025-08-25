
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

namespace PruebaTecnicaApiTest.PropertyTests
{
    public class PropertyControllerTests
    {
        private readonly Mock<IPropertyService> _serviceMock;
        private readonly PropertyController _controller;

        public PropertyControllerTests()
        {
            _serviceMock = new Mock<IPropertyService>();
            _controller = new PropertyController(_serviceMock.Object);
        }

        [Fact]
        public async Task PropertyList_ReturnsOkResult()
        {
            // Arrange
            var expected = new ResultModel<PropertyDto[]>
            {
                HasError = false,
                Data = new[] { new PropertyDto { IdProperty = 1, Name = "Jorge" } }
            };

            _serviceMock.Setup(s => s.PropertyList()).ReturnsAsync(expected);

            // Act
            var result = await _controller.Property();

            // Assert
            var actionResult = Assert.IsType<ActionResult<ResultModel<PropertyDto[]>>>(result);
            var value = Assert.IsType<ResultModel<PropertyDto[]>>(actionResult.Value);
            Assert.False(value.HasError);
            Assert.Single(value.Data);
        }
    }
}
