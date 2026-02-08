using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi_Demo.Models;
public class Department
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? DepartmentManager { get; set; }
   // [JsonIgnore]
    public virtual List<Employee>? Employees { get; set; }
}
