using Company.Shared.Responses;
using Company_Management_System.Dtos.Employee;
using Company_Management_System.Mapping;
using Microsoft.EntityFrameworkCore;
using WebApi_Demo.interfaces;
using WebApi_Demo.Models;

namespace WebApi_Demo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationEntity context;
        public EmployeeRepository(ApplicationEntity context)
        {
            this.context = context;
        }
        public async Task<GeneralResponse<PagedResult<EmployeeDto>>> GetAllAsync(int pageNumber ,int pageSize,string? search)
        {
            if (pageNumber < 1)
                return new GeneralResponse<PagedResult<EmployeeDto>>(false, "Current page must be greater than 1");
            else if (pageSize < 1 || pageSize > 100)
				return new GeneralResponse<PagedResult<EmployeeDto>>(false, "Page Size must be between 1 and 100");
			
            IQueryable<Employee> query = context.Employees.AsNoTracking().Include(e => e.Department).OrderBy(x=>x.Name);
			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(e =>
					e.Name.ToLower().Contains(search.ToLower()));
			}
			var dtoQuery = query.Select(e => e.ToDto());

			var pagedResult = await PagedResult<EmployeeDto>
				.GetPaginated(dtoQuery, pageNumber, pageSize);
			return new GeneralResponse<PagedResult<EmployeeDto>>(true,"Employees loaded successfully",pagedResult);
		}

        public async Task<GeneralResponse<EmployeeDto>> GetByIdAsync(int id)
        {
            var employee = await context.Employees.AsNoTracking().Include(x => x.Department).FirstOrDefaultAsync(emp => emp.Id == id);
			if (employee == null)
			{
				return new GeneralResponse<EmployeeDto>(false,"Employee not found",null);
			}

			return new GeneralResponse<EmployeeDto>( true,"Employee retreived successfully",employee.ToDto());
        }
        public async Task<GeneralResponse<int>> CreateAsync(EmployeeFormDto employeeDto)
        {
			var existDept = await context.Department.AnyAsync(x => x.Id == employeeDto.departmentId);
            if (!existDept)
                return new GeneralResponse<int>(false, "Department not exists", 0);

			var employee = new Employee();
            employeeDto.ToModel(employee);
           
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
            return new GeneralResponse<int>(true, "Employee Added Successfully", employee.Id);
        }
        public async Task<GeneralResponse<int>> UpdateAsync(int id ,EmployeeFormDto employeeDto)
        {
            var oldEmployee = await context.Employees.FindAsync(id);
            if (oldEmployee == null)
                return new GeneralResponse<int>(false, "Employee not exists", 0);

			var existDept = await context.Department.AnyAsync(x => x.Id == employeeDto.departmentId);
            if (!existDept)
                return new GeneralResponse<int>(false, "Department not exists", 0);

			employeeDto.ToModel(oldEmployee);

            await context.SaveChangesAsync();
            return new GeneralResponse<int>(true, "Employee Updated Successfully", id);
        }
        public async Task<GeneralResponse<bool>> DeleteAsync(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
            {
                return new GeneralResponse<bool>(false, "Employee not exists", false);
            }
             context.Employees.Remove(employee);
            await context.SaveChangesAsync();
            return new GeneralResponse<bool>(true, "Employee Deleted Successfully", true);
        }
    }
}
