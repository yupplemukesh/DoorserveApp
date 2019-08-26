using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Models;
using doorserve.Models.Company;

namespace doorserve.Repository
{
   public interface ICompany:IDisposable
    {

        Task<List<CompanyModel>> GetCompanyDetails(Guid? CompanyId);
        Task<CompanyModel> GetCompanyDetailByCompanyId(Guid? CompanyId);
        Task<ResponseModel> AddUpdateDeleteCompany(CompanyModel company);
        Task<AgreementModel> GetAgreement(Guid? compId);
        Task<ResponseModel> AddOrEditAgreeement(AgreementModel agreement);



    }
}
