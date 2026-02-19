using Company_Management_System.Dtos.Deparment;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WebApi_Demo.Controllers;
using WebApi_Demo.interfaces;
using WebApi_Demo.Models;
using WebApi_Demo.Repository;

namespace CompanyApi.XUnit.ControllerTest.DepartmentsActions
{
	public class DepartmentTest
	{
		private readonly Mock<IDepartmentRepository> _departmentRepositoryMock;
		public DepartmentTest()
		{
			_departmentRepositoryMock = new();
		}
		[Fact]
		public async Task GetAllAsync_Should_be_NotNull_NotEmpty()
		{
			// Arrange
			var mockList = new List<DepartmentDto>
			 {
				new DepartmentDto { Id = 1, Name = "HR", DepartmentManager = "Sara" },
				new DepartmentDto { Id = 2, Name = "IT", DepartmentManager = "Mahmoud" }
			};
			var mockResponse = new GeneralResponse<IEnumerable<DepartmentDto>>(true, "Success", mockList);

			_departmentRepositoryMock.Setup(r => r.GetAllAsync())
				.ReturnsAsync(mockResponse);

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var resultAction = await controller.GetAllAsync();

			// Assert
			var okResult = resultAction.Result as OkObjectResult;
			okResult.Should().NotBeNull("Expected OkObjectResult from controller");

			var generalResponse = okResult.Value as GeneralResponse<IEnumerable<DepartmentDto>>;
			generalResponse.Should().NotBeNull("Expected Value to be GeneralResponse<IEnumerable<DepartmentDto>>");
			generalResponse.Success.Should().BeTrue();
			generalResponse.Data.Should().NotBeNull();
			generalResponse.Data.Should().NotBeEmpty("Expected the list to contain at least one department");
		}
		[Theory]
		[InlineData(1)]
		public async Task GetById_Should_Be_NotFound_Return_Status_Code_404(int id)
		{
			// Arrange
			var mockItem = new DepartmentDto
			{
				Id = 1,
				Name = "CS",
				DepartmentManager = "Mahmoud"
			};

			var mockResponse = new GeneralResponse<DepartmentDto>(false, "Department not found", mockItem);

			_departmentRepositoryMock.Setup(r => r.GetByIdAsync(id))
				.ReturnsAsync(mockResponse);

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var resultAction = await controller.GetDepartmentById(id);

			// Assert
			var okResult = resultAction.Result as OkObjectResult;
			okResult.Should().NotBeNull();

			var generalResponse = okResult.Value as GeneralResponse<DepartmentDto>;
			generalResponse.Should().NotBeNull();
			generalResponse.Success.Should().BeFalse();
		}

		[Fact]
		public async Task GetById_Should_be_Success_Return_StatusCode_200()
		{
			//Arrange
			var mockItem = new DepartmentDto
			{
				Id = 1,
				Name = "CS",
				DepartmentManager = "Mahmoud"
			};
			var mockResponse = new GeneralResponse<DepartmentDto>(true, "Success", mockItem);
			_departmentRepositoryMock.Setup(r => r.GetByIdAsync(mockItem.Id)).ReturnsAsync(mockResponse);

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			//Act
			var resultAction = await controller.GetDepartmentById(mockItem.Id);

			// Assert
			var okResult = resultAction.Result as OkObjectResult;
			okResult.Should().NotBeNull("Expected OkObjectResult from controller");
			okResult.StatusCode.Should().Be(200);

			var generalResponse = okResult.Value as GeneralResponse<DepartmentDto>;
			generalResponse.Should().NotBeNull();
			generalResponse.Success.Should().BeTrue();

			generalResponse.Data.Should().NotBeNull();
			generalResponse.Data.Id.Should().Be(mockItem.Id);
			generalResponse.Data.Name.Should().Be(mockItem.Name);
		}
		[Fact]
		public async Task Create_Should_be_Success_Return_StatusCode_200()
		{
			// Arrange

			var newDepartment = new DepartmentFormDto
			{
				Name = "CS",
				DepartmentManager = "Mahmoud"
			};
			_departmentRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<DepartmentFormDto>())).ReturnsAsync(new GeneralResponse<int>(true, "Department Added Successfully", 1));

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var resultAction = await controller.CreateAsync(newDepartment);
			// Assert
			var okResult = resultAction.Result as OkObjectResult;
			okResult.Should().NotBeNull("Expected OkObjectResult from controller");
			okResult.StatusCode.Should().Be(200);
			var responseValue = okResult.Value as GeneralResponse<DepartmentFormDto>;
			responseValue.Should().NotBeNull();
			responseValue.Message.Should().Be("Department Created Successfully");
			responseValue.Data.Should().NotBeNull();
			responseValue.Data.Name.Should().Be(newDepartment.Name);

		}

		[Fact]
		public async Task Create_Should_Return_BadRequest_When_ModelState_Invalid()
		{
			// Arrange
			var departmentDto = new DepartmentFormDto {Name = "", DepartmentManager = "Mahmoud" };

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);
			controller.ModelState.AddModelError("Name", "Name is required");

			// Act
			var result = await controller.CreateAsync(departmentDto);

			// Assert
			var badRequest = result.Result as BadRequestObjectResult;
			badRequest.StatusCode.Should().Be(400);
		}

		[Fact]
		public async Task Update_Should_Return_NoContent_When_Success()
		{
			// Arrange
			var departmentDto = new DepartmentFormDto { Name = "CS", DepartmentManager = "Mahmoud" };
			int departmentId = 1;

			_departmentRepositoryMock
				.Setup(r => r.UpdateAsync(departmentId, It.IsAny<DepartmentFormDto>()))
				.ReturnsAsync(new GeneralResponse<int>(true, "Department Updated Successfully", departmentId));

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var result = await controller.UpdateAsync(departmentId, departmentDto);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}
		[Fact]
		public async Task Update_Should_Return_BadRequest_When_Department_Already_Exists()
		{
			// Arrange
			var departmentDto = new DepartmentFormDto { Name = "CS", DepartmentManager = "Mahmoud" };
			int departmentId = 1;

			_departmentRepositoryMock
				.Setup(r => r.UpdateAsync(departmentId, It.IsAny<DepartmentFormDto>()))
				.ReturnsAsync(new GeneralResponse<int>(false, "Department Already Exists", 0));

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var result = await controller.UpdateAsync(departmentId, departmentDto);

			// Assert
			var badRequest = result as BadRequestObjectResult;
			badRequest.Should().NotBeNull();
			badRequest.Value.Should().Be("Department Already Exists");
		}

		[Fact]
		public async Task Update_Should_Return_BadRequest_When_Department_NotFound()
		{
			// Arrange
			var departmentDto = new DepartmentFormDto { Name = "CS", DepartmentManager = "Mahmoud" };
			int departmentId = 99;

			_departmentRepositoryMock
				.Setup(r => r.UpdateAsync(departmentId, It.IsAny<DepartmentFormDto>()))
				.ReturnsAsync(new GeneralResponse<int>(false, "Department not found", 0));

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var result = await controller.UpdateAsync(departmentId, departmentDto);

			// Assert
			var badRequest = result as BadRequestObjectResult;
			badRequest.Should().NotBeNull();
			badRequest.Value.Should().Be("Department not found");
		}
		[Fact]
		public async Task Update_Should_Return_BadRequest_When_ModelState_Invalid()
		{
			// Arrange
			var departmentDto = new DepartmentFormDto { Name = "", DepartmentManager = "Mahmoud" };
			int departmentId = 1;

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);
			controller.ModelState.AddModelError("Name", "Name is required");

			// Act
			var result = await controller.UpdateAsync(departmentId, departmentDto);

			// Assert
			var badRequest = result as BadRequestObjectResult;
			badRequest.Should().NotBeNull();
			badRequest.Value.Should().BeOfType<SerializableError>();
		}

		[Fact]
		public async Task Delete_Should_Return_Ok_When_Success()
		{
			// Arrange
			int departmentId = 1;

			_departmentRepositoryMock
				.Setup(r => r.DeleteAsync(departmentId))
				.ReturnsAsync(new GeneralResponse<bool>(true, "Department Deleted Successfully", true));

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var result = await controller.DeleteAsync(departmentId);

			// Assert
			var okResult = result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult.StatusCode.Should().Be(200);

			var response = okResult.Value as GeneralResponse<bool>;
			response.Should().NotBeNull();
			response.Success.Should().BeTrue();
			response.Message.Should().Be("Department Deleted Successfully");
			response.Data.Should().BeTrue();
		}

		[Fact]
		public async Task Delete_Should_Return_BadRequest_When_Department_NotFound()
		{
			// Arrange
			int departmentId = 99;

			_departmentRepositoryMock
				.Setup(r => r.DeleteAsync(departmentId))
				.ReturnsAsync(new GeneralResponse<bool>(false, "Department not found", false));

			var controller = new DepartmentsController(_departmentRepositoryMock.Object);

			// Act
			var result = await controller.DeleteAsync(departmentId);

			// Assert
			var badRequest = result as BadRequestObjectResult;
			badRequest.Should().NotBeNull();
			badRequest.StatusCode.Should().Be(400);
			badRequest.Value.Should().Be("Department not found");
		}

	}
}
