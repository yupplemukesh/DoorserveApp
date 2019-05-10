
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TogoFogo.Filters;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public class Employee : IEmployee
    {

        private readonly ApplicationDbContext _context;
        public Employee()
        {
            _context = new ApplicationDbContext();
        }
        public async Task<List<EmployeeModel>> GetAllEmployees(FilterModel filterModel )
        {

            var sp = new List<SqlParameter>();
            var param = new SqlParameter("@CenterId", ToDBNull(filterModel.RefKey));
            sp.Add(param);
            param = new SqlParameter("@providerId", ToDBNull(filterModel.ProviderId));
            sp.Add(param);
            param = new SqlParameter("@CompanyId", ToDBNull(filterModel.CompId));
            sp.Add(param);
            return await _context.Database.SqlQuery<EmployeeModel>("USPGetAllEmployees @CenterId,@providerId,@CompanyId", sp.ToArray()                  
                ).ToListAsync();
        }
        public async Task<EmployeeModel> GetEmployeeById(Guid employeeId)
        {
            var employee = new EmployeeModel();
            SqlParameter param= new SqlParameter("@EmployeeId", employeeId);
            using (var connection = _context.Database.Connection)
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "USPGetEmployeeById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(param);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var folder = "/UploadedImages/Employees/";
                    employee =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<EmployeeModel>(reader)
                            .SingleOrDefault();
                     employee.EMPPhotoUrl = folder+"/DP/" + employee.EMPPhoto;
                   
                    employee.ConAdhaarFileUrl   =     folder + "/adhr/" + employee.ConAdhaarFileName;
                    employee.ConPanFileUrl      =     folder + "/PanCards/" + employee.ConPanFileName;
                    employee.ConVoterIdFileUrl  =     folder + "/VoterIds/" + employee.ConVoterIdFileName;

                    reader.NextResult();
                    employee.Vehicle =
                        ((IObjectContextAdapter)_context)
                            .ObjectContext
                            .Translate<VehicleModel>(reader)
                            .SingleOrDefault();
                }
            }
            return employee;
        }
        public async Task<ResponseModel> AddUpdateDeleteEmployee(EmployeeModel employee)
        {                      
            List<SqlParameter> sp = new List<SqlParameter>();
            SqlParameter param = new SqlParameter("@EMPID", ToDBNull(employee.EmpId));          
            sp.Add(param);
            param = new SqlParameter("@EMPCode", ToDBNull(employee.EmpCode));
            sp.Add(param);
            param = new SqlParameter("@EMPFNAME", ToDBNull(employee.ConFirstName));
            sp.Add(param);
            param = new SqlParameter("@EMPLName", ToDBNull(employee.ConLastName));
            sp.Add(param);
            param = new SqlParameter("@EMPPhoto", ToDBNull(employee.EMPPhoto));
            sp.Add(param);
            param = new SqlParameter("@EMPMobileNo", ToDBNull(employee.ConMobileNumber));
            sp.Add(param);
            param = new SqlParameter("@EMPEmail", ToDBNull(employee.ConEmailAddress));
            sp.Add(param);
            param = new SqlParameter("@EMPPAN", ToDBNull(employee.ConPanNumber));
            sp.Add(param);
            param = new SqlParameter("@EMPPANFILENAME", ToDBNull(employee.ConPanFileName));
            sp.Add(param);
            param = new SqlParameter("@EMPVOTERID", ToDBNull(employee.ConVoterId));
            sp.Add(param);
            param = new SqlParameter("@EMPVOTERIDFILENAME", ToDBNull(employee.ConVoterIdFileName));
            sp.Add(param);
            param = new SqlParameter("@EMPADHAAR", ToDBNull(employee.ConAdhaarNumber));
            sp.Add(param);
            param = new SqlParameter("@EMPADHAARFILENAME", ToDBNull(employee.ConAdhaarFileName));
            sp.Add(param);
            param = new SqlParameter("@contactId", ToDBNull(employee.ContactId));
            sp.Add(param);
            param = new SqlParameter("@DesignationId", ToDBNull(employee.DesignationId));
            sp.Add(param);
            param = new SqlParameter("@DepartmentId", ToDBNull(employee.DepartmentId));
            sp.Add(param);
            param = new SqlParameter("@EMPDOJ", ToDBNull(employee.EMPDOJ));
            sp.Add(param);
            param = new SqlParameter("@EMPDOB", ToDBNull(employee.EMPDOB));
            sp.Add(param);
            param = new SqlParameter("@USER", ToDBNull(employee.UserId));
            sp.Add(param);
            param = new SqlParameter("@IsActive", (object)employee.IsActive);
            sp.Add(param);
            param = new SqlParameter("@IsPickUp", (object)employee.IsPickUp);
            sp.Add(param);
            param = new SqlParameter("@AddressTypeId", ToDBNull(employee.AddressTypeId));
            sp.Add(param);
            param = new SqlParameter("@CityId", ToDBNull(employee.CityId));
            sp.Add(param);           
            param = new SqlParameter("@StateId", ToDBNull(employee.StateId));
            sp.Add(param);
            param = new SqlParameter("@CountryId", ToDBNull(employee.CountryId));
            sp.Add(param);
            param = new SqlParameter("@PinCode", ToDBNull(employee.PinNumber));
            sp.Add(param);
            param = new SqlParameter("@Address", ToDBNull(employee.Address));
            sp.Add(param);
            param = new SqlParameter("@Locality", ToDBNull(employee.Locality));
            sp.Add(param);
            param = new SqlParameter("@NearByLocation", ToDBNull(employee.NearLocation));
            sp.Add(param);
            param = new SqlParameter("@RefKey", ToDBNull(employee.RefKey));
            sp.Add(param);
            param = new SqlParameter("@Action", (object)employee.Action);
            sp.Add(param);
            param = new SqlParameter("@VHType", ToDBNull(employee.Vehicle.VHTypeId));
            sp.Add(param);
            param = new SqlParameter("@VHID", ToDBNull(employee.Vehicle.VHId));
            sp.Add(param);
            param = new SqlParameter("@VHNumber", ToDBNull(employee.Vehicle.VHNumber));
            sp.Add(param);
            param = new SqlParameter("@VHModel", ToDBNull(employee.Vehicle.VHModel));
            sp.Add(param);
            param = new SqlParameter("@VehicleBrand", ToDBNull(employee.Vehicle.VehicleBrand));
            sp.Add(param);
            param = new SqlParameter("@DrivingLicense", ToDBNull(employee.Vehicle.DrivingLicense));
            sp.Add(param);
            param = new SqlParameter("@InsuranceExpairyDate", ToDBNull(employee.Vehicle.InsuranceExpairyDate));
            sp.Add(param);
            param = new SqlParameter("@EmployeeTypeId", ToDBNull(employee.EngineerTypeId));
            sp.Add(param);
            param = new SqlParameter("@ISUSER", ToDBNull(employee.IsUser));
            sp.Add(param);
            param = new SqlParameter("@DefautPwd", ToDBNull(employee.Password));
            sp.Add(param);
            param = new SqlParameter("@CompanyId", ToDBNull(employee.CompanyId));
            sp.Add(param);
            param = new SqlParameter("@ProviderId", ToDBNull(employee.ProviderId));
            sp.Add(param);
            var sql = "USPInsertUpdateEMPDetails @EMPID,@EMPCode,@EMPFNAME,@EMPLName,@EMPPhoto,@EMPMobileNo ,@EMPEmail ,@EMPPAN ,@EMPPANFILENAME,@EMPVOTERID," +
                        "@EMPVOTERIDFILENAME,@EMPADHAAR,@EMPADHAARFILENAME,@contactId,@DesignationId, @DepartmentId ,@EMPDOJ , @EMPDOB , @USER,@IsActive" +
                        ",@IsPickUp,@AddressTypeId,@CityId,@StateId,@CountryId,@PinCode,@Address,@Locality,@NearByLocation,@RefKey,@Action,@VHType,@VHID, @VHNumber,@VHModel,@VehicleBrand,@DrivingLicense,@InsuranceExpairyDate,@EmployeeTypeId," +
                        "@ISUSER,@DefautPwd,@CompanyId,@ProviderId";
            var res = await _context.Database.SqlQuery<ResponseModel>(sql, sp.ToArray()).SingleOrDefaultAsync();
            if (res.ResponseCode == 0)
                res.IsSuccess = true;
            else
                res.IsSuccess = false;
            return res;
        }
      
        private  object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
              
    }
}