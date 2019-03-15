using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public interface IEmployee : IDisposable
    {
        Task<List<EmployeeModel>> GetAllEmployees(Guid ? serviceCenterId, Guid? providerId);
        Task<EmployeeModel> GetEmployeeById(Guid employeeId);
        Task<ResponseModel> AddUpdateDeleteEmployee(EmployeeModel employee);    
        void Save();
    }
}
