using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;

namespace doorserve.Repository
{
    public interface IEmployee : IDisposable
    {
        Task<List<EmployeeModel>> GetAllEmployees(FilterModel filter);
        Task<EmployeeModel> GetEmployeeById(Guid? employeeId);
        Task<ResponseModel> AddUpdateDeleteEmployee(EmployeeModel employee);
        Task<EmployeeModel> GetPinCode(string pin);
        void Save();
    }
}
