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
        #region PostAsync

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
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label object from body is null.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void PostAsync_Failed_LabelIsNullOrWhiteSpace(string labelName)
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Name = labelName };
            var response = controller.PostAsync(label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label could be neither null nor whitespace.");
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
            result.Value.ShouldBe($"Label '{label.Name}' already exists, cannot create.");
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
            //mockMapper.Setup(x => x.Map<Models.Label, DTOs.Label>(It.IsAny<Models.Label>())).Returns(labelDTO);

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mapper, mockLabelRepo.Object);

            // Act
            var labelDTO = new DTOs.Label { Name = "NewLabel" };
            var response = controller.PostAsync(labelDTO);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(201);
            var returnedLabel = result.Value as DTOs.Label;
            returnedLabel.Name.ShouldBe(labelDTO.Name);
        }

        #endregion PostAsync

        #region PutAsync

        [Fact]
        public void PutAsync_Failed_LabelIdNotIdentical()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Id = 1, Name = "LabelA" };
            var response = controller.PutAsync(2, label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label id from url and body are not identical.");
        }

        [Fact]
        public void PutAsync_Failed_LabelNotFound()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            mockLabelRepo.Setup(x => x.GetLabelAsync(It.IsAny<int>())).Returns(Task.FromResult<Models.Label>(null));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Id = 1, Name = "NewLabelA" };
            var response = controller.PutAsync(1, label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(404);
            result.Value.ShouldBe($"Label with id '{label.Id}' not found.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void PutAsync_Failed_LabelIsNullOrWhiteSpace(string labelName)
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            var labelModel = new Models.Label { Id = 1, Name = "LabelA" };
            mockLabelRepo.Setup(x => x.GetLabelAsync(It.IsAny<int>())).Returns(Task.FromResult(labelModel));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Id = 1, Name = labelName };
            var response = controller.PutAsync(1, label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label could be neither null nor whitespace.");
        }

        [Fact]
        public void PutAsync_Failed_LabelExisted()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            var labelModel = new Models.Label { Id = 1, Name = "LabelA" };
            mockLabelRepo.Setup(x => x.GetLabelAsync(It.IsAny<int>())).Returns(Task.FromResult(labelModel));
            var labels = new List<Models.Label>() { labelModel, new Models.Label { Name = "LabelB" } };
            mockLabelRepo.Setup(x => x.GetLabelsAsync()).Returns(Task.FromResult(labels.AsEnumerable()));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Id = 3, Name = "LabelA" };
            var response = controller.PutAsync(3, label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label '{label.Name}' already exists, cannot update.");
        }

        [Fact]
        public void PutAsync_Failed_LabelIdentiqueAsCurrent()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mockMapper = new Mock<IMapper>();

            var labelModel = new Models.Label { Id = 1, Name = "LabelA" };
            mockLabelRepo.Setup(x => x.GetLabelAsync(It.IsAny<int>())).Returns(Task.FromResult(labelModel));
            var labels = new List<Models.Label>() { labelModel, new Models.Label { Name = "LabelB" } };
            mockLabelRepo.Setup(x => x.GetLabelsAsync()).Returns(Task.FromResult(labels.AsEnumerable()));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mockMapper.Object, mockLabelRepo.Object);

            // Act
            var label = new DTOs.Label { Id = 1, Name = "LabelA" };
            var response = controller.PutAsync(1, label);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(400);
            result.Value.ShouldBe($"Label '{label.Name}' is identical as current, no need to update.");
        }

        [Fact]
        public void PutAsync_Succeed()
        {
            var mockLabelRepo = new Mock<ILabelRepository>();
            var mockLogger = new Mock<ILogger<LabelsController>>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new Mappings.MappingProfile())));

            var labelModel = new Models.Label { Id = 1, Name = "LabelA" };
            mockLabelRepo.Setup(x => x.GetLabelAsync(It.IsAny<int>())).Returns(Task.FromResult(labelModel));
            var labels = new List<Models.Label>() { labelModel, new Models.Label { Name = "LabelB" } };
            mockLabelRepo.Setup(x => x.GetLabelsAsync()).Returns(Task.FromResult(labels.AsEnumerable()));
            var newLabelModel = new Models.Label { Id = 1, Name = "NewLabel" };
            mockLabelRepo.Setup(x => x.UpdateLabelAsync(It.IsAny<Models.Label>())).Returns(Task.FromResult(newLabelModel));

            // Arrange
            var controller = new LabelsController(mockLogger.Object, mapper, mockLabelRepo.Object);

            // Act
            var labelDTO = new DTOs.Label { Id = 1, Name = "NewLabel" };
            var response = controller.PutAsync(1, labelDTO);

            // Assert
            var result = response.Result.Result as ObjectResult;
            result.StatusCode.ShouldBe(200);
            var returnedLabel = result.Value as DTOs.Label;
            returnedLabel.Name.ShouldBe(labelDTO.Name);
        }

        #endregion PutAsync
    }
}
