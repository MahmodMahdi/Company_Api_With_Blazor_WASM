using Company_Management_System.Dtos.Deparment;
using Company_Management_System.Mapping;
using Microsoft.EntityFrameworkCore;
using WebApi_Demo.interfaces;
using WebApi_Demo.Models;

namespace WebApi_Demo.Repository
{
    public class DepartmentRepository : IDepartmentRepository

    {
        private readonly ApplicationEntity context;
        public DepartmentRepository(ApplicationEntity Context)
        {
            context = Context;
        }
        public async Task<GeneralResponse<IEnumerable<DepartmentDto>>> GetAllAsync()
        {
            var departments = await context.Department.OrderBy(x => x.Name).ToListAsync();
            return new GeneralResponse<IEnumerable<DepartmentDto>>(true,"Employees retrieved successfully", departments.Select(d => d.ToDto()).ToList());
        }
        public async Task<GeneralResponse<DepartmentWithEmployeesDto>> GetDepartmentWithEmployeesByIdAsync(int id)
        {
            var department = await context.Department.AsNoTracking().Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);
            if (department == null) return new GeneralResponse<DepartmentWithEmployeesDto>(false,"Department not found",null);
            var result = new DepartmentWithEmployeesDto
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,
                DepartmentManager = department.DepartmentManager,
                Employees = department.Employees!.Select(d => new AllEmployeeDto { EmployeeId = d.Id, EmployeeName = d.Name }).ToList()
            };
			return new GeneralResponse<DepartmentWithEmployeesDto>(true,"Department with employees retreived successfully",result);
        }
        public async Task<GeneralResponse<DepartmentDto>> GetByIdAsync(int id)
        {
            var department = await context.Department.FirstOrDefaultAsync(d => d.Id == id);
            if(department == null)
            {
                return new GeneralResponse<DepartmentDto>(false, "Department not found", null);
			}
            return new GeneralResponse<DepartmentDto>(true,"Department retreived successfully" , department?.ToDto());
        }
        public async Task<GeneralResponse<int>> CreateAsync(DepartmentFormDto newDepartment)
        {
            var department = new Department();
            newDepartment.ToModel(department);
            await context.Department.AddAsync(department);
            await context.SaveChangesAsync();
            return new GeneralResponse<int>(true, "Department Added Successfully", department.Id);
        }
        public async Task<GeneralResponse<int>> UpdateAsync(int id, DepartmentFormDto departmentdto)
        {
            var oldDepartment = await context.Department.SingleOrDefaultAsync(c => c.Id == id);
            if (oldDepartment != null)
            {
                var exist = await context.Department.FirstOrDefaultAsync(x=>x.Name ==  departmentdto.Name&&x.Id !=id);
                if (exist != null) return new GeneralResponse<int>(false, "Department Already Exists", 0);

                departmentdto.ToModel(oldDepartment);
				await context.SaveChangesAsync();
                return new GeneralResponse<int>(true, "Department Updated Successfully", id);
			}
            return new GeneralResponse<int>(false, "Department not found", 0);
		}
        public async Task<GeneralResponse<bool>> DeleteAsync(int id)
        {
            var department = await context.Department.FirstOrDefaultAsync(c => c.Id == id);
            if (department == null)
            {
                return new GeneralResponse<bool>(false, "Department not found", false);
            }
             context.Department.Remove(department);
            await context.SaveChangesAsync();
            return new GeneralResponse<bool>(true, "Department Deleted Successfully",true);
  
        }
    }
}
