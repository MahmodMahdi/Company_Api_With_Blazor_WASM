using Company_Management_System.Dtos.Deparment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi_Demo.Models;
using WebApi_Demo.Repository;

namespace CompanyApi.XUnit.RepositoryTest.Departments
{
	public class DepartmentsTest
	{
		[Fact]
		public async Task GetAllAsync_ShouldWork()
		{
			var options = new DbContextOptionsBuilder<ApplicationEntity>()
				.UseInMemoryDatabase("TestDb1")
				.Options;

			using var context = new ApplicationEntity(options);

			context.Department.Add(new Department
			{
				Name = "IT",
				DepartmentManager = "Mahmoud"
			});

			await context.SaveChangesAsync();

			var repo = new DepartmentRepository(context);

			var result = await repo.GetAllAsync();

			Assert.True(result.Success);
		}
		[Fact]
		public async Task GetByIdAsync_ShouldWork()
		{
			var options = new DbContextOptionsBuilder<ApplicationEntity>()
				.UseInMemoryDatabase("TestDb2")
				.Options;
			using var context = new ApplicationEntity(options);
			context.Department.Add(new Department
			{
				Name = "HR",
				DepartmentManager = "Sara"
			});
			await context.SaveChangesAsync();
			var repo = new DepartmentRepository(context);
			var result = await repo.GetByIdAsync(1);
			Assert.True(result.Success);
		}
		[Fact]
		public async Task GetDepartmentWithEmployeesByIdAsync_ShouldWork()
		{
			var options = new DbContextOptionsBuilder<ApplicationEntity>()
				.UseInMemoryDatabase("TestDb5")
				.Options;
			using var context = new ApplicationEntity(options);
			var repo = new DepartmentRepository(context);
			context.Department.Add(new Department
			{
				Id = 1,
				Name = "IT",
				DepartmentManager = "Mahmoud",
				Employees = new List<Employee>
				{
					new Employee { Id = 1, Name = "Ahmed" },
					new Employee { Id = 2, Name = "Mona" }
				}
			});
			await context.SaveChangesAsync();
			var result = await repo.GetDepartmentWithEmployeesByIdAsync(1);
			Assert.True(result.Success);
			Assert.Equal(2, result.Data.Employees.Count);
		}
		[Fact]
		public async Task CreateAsync_ShouldWork()
		{
			var options = new DbContextOptionsBuilder<ApplicationEntity>()
				.UseInMemoryDatabase("TestDb3")
				.Options;
			using var context = new ApplicationEntity(options);
			var repo = new DepartmentRepository(context);
			var result = await repo.CreateAsync(new DepartmentFormDto
			{
				Name = "Finance",
				DepartmentManager = "Ali"
			});
			Assert.True(result.Success);
			Assert.Equal(1, result.Data);
		}
		[Fact]
		public async Task UpdateAsync_ShouldWork()
		{
			//هنا بنجهز إعدادات قاعدة البيانات لاختبارنا.
			// بنعمل Builder لإعداد الـ DbContext اللي اسمه ApplicationEntity.
			// بنستخدم قاعدة بيانات في الذاكرة(In - Memory) عشان الاختبارات ما تمسش قاعدة البيانات الحقيقية.
			// بيطلع كائن الإعدادات اللي ممكن نديه للـ DbContext بعد كده
			var options = new DbContextOptionsBuilder<ApplicationEntity>()
				.UseInMemoryDatabase("TestDb4")
				.Options;
			using var context = new ApplicationEntity(options);
			var repo = new DepartmentRepository(context);
			context.Department.Add(new Department
			{
				Id = 1,
				Name = "HR",
				DepartmentManager = "Sara"
			});
			await context.SaveChangesAsync();
			var result = await repo.UpdateAsync(1, new DepartmentFormDto { Name = "CS", DepartmentManager = "Mahmoud" });
			Assert.True(result.Success);
			var updatedDept = await context.Department.FindAsync(1);
			Assert.Equal("CS", updatedDept.Name);
			Assert.Equal("Mahmoud", updatedDept.DepartmentManager);
		}
		[Fact]
		public async Task DeleteAsync_ShouldWork()
		{
			var options = new DbContextOptionsBuilder<ApplicationEntity>()
				.UseInMemoryDatabase("TestDb6")
				.Options;
			using var context = new ApplicationEntity(options);
			var repo = new DepartmentRepository(context);
			context.Department.Add(new Department
			{
				Id = 1,
				Name = "HR",
				DepartmentManager = "Sara"
			});
			await context.SaveChangesAsync();
			var result = await repo.DeleteAsync(1);
			Assert.True(result.Success);
			var deletedDept = await context.Department.FindAsync(1);
			Assert.Null(deletedDept);
		}
	}
}
