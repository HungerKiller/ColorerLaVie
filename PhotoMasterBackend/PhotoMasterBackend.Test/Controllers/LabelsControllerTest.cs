using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PhotoMasterBackend.Controllers;
using PhotoMasterBackend.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PhotoMasterBackend.Test.Controllers
{
    [ExcludeFromCodeCoverage]
    public class LabelsControllerTest
    {
        [Fact]
        public void PostAsync_Failed_LabelIsNull()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var response = controller.PostAsync(null);

            // Assert
            var result = response.Result.Result as StatusCodeResult;
            result.StatusCode.ShouldBe(400);
        }

        [Fact]
        public void PostAsync_Failed_LabelExisted()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            var labels = new List<Models.Label>() { new Models.Label { Name = "LabelA" }, new Models.Label { Name = "LabelB" } };
            mockLabelRepo.Setup(x => x.GetLabelsAsync()).Returns(Task.FromResult(labels.AsEnumerable()));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Name = "LabelA" };
            var response = controller.PostAsync(label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label '{label.Name}' already exists.");
        }

        [Fact]
        public void PostAsync_Failed_LabelContainsWhiteSpace()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            var labels = new List<Models.Label>() { new Models.Label { Name = "LabelA" }, new Models.Label { Name = "LabelB" } };
            mockLabelRepo.Setup(x => x.GetLabelsAsync()).Returns(Task.FromResult(labels.AsEnumerable()));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Name = "Label A" };
            var response = controller.PostAsync(label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label should not contain character whitespace.");
        }

        [Fact]
        public void PostAsync_Failed_LabelLessThanLength5()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMmapper = new Mock<IMapper>();

            var labels = new List<Models.Label>() { new Models.Label { Name = "LabelA" }, new Models.Label { Name = "LabelB" } };
            mockLabelRepo.Setup(x => x.GetLabelsAsync()).Returns(Task.FromResult(labels.AsEnumerable()));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMmapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Name = "La" };
            var response = controller.PostAsync(label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label's length should be greater than 5 characters.");
        }

        [Fact]
        public void PostAsync_Succeed()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            //var mockMapper = new Mock<IMapper>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new Mappings.MappingProfile())));

            var labelModel = new Models.Label { Name = "NewLabel" };
            var labels = new List<Models.Label>() { new Models.Label { Name = "LabelA" }, new Models.Label { Name = "LabelB" } };
            mockLabelRepo.Setup(x => x.GetLabelsAsync()).Returns(Task.FromResult(labels.AsEnumerable()));
            mockLabelRepo.Setup(x => x.AddLabelAsync(It.IsAny<Models.Label>())).Returns(Task.FromResult(labelModel));
            var labelDTO = new DTOs.Label { Name = "NewLabel" };
            //mockMapper.Setup(x => x.Map<Models.Label, DTOs.Label>(It.IsAny<Models.Label>())).Returns(labelDTO);

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mapper, mockLabelRepo.Object);

            // Act
            var response = controller.PostAsync(labelDTO);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(201);
            var returnedLabel = result.Value as DTOs.Label;
            returnedLabel.Name.ShouldBe(labelDTO.Name);
        }
    }
}
