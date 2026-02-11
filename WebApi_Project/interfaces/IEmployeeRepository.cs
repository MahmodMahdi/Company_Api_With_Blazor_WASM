using Company.Shared.Responses;
using Company_Management_System.Dtos.Employee;
using Microsoft.AspNetCore.Mvc;
using WebApi_Demo.Models;

namespace WebApi_Demo.interfaces
{
    public interface IEmployeeRepository
    {
        Task<GeneralResponse<PagedResult<EmployeeDto>>> GetAllAsync(int pageNumber,int pageSize,string? search);
        Task<GeneralResponse<EmployeeDto>> GetByIdAsync(int id);
        Task<GeneralResponse<int>> CreateAsync(EmployeeFormDto employeeDto);
        Task<GeneralResponse<int>> UpdateAsync(int id,EmployeeFormDto employeeDto);
        Task<GeneralResponse<bool>> DeleteAsync(int id);
    }
}
