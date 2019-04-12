using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.Company;

namespace TogoFogo.Repository
{
   public interface ICompany:IDisposable
    {

        Task<List<CompanyModel>> GetCompanyDetails();
        Task<CompanyModel> GetCompanyDetailByCompanyId(Guid CompanyId);
        Task<ResponseModel> AddUpdateDeleteCompany(CompanyModel company);
        Task<AgreementModel> GetAgreement(Guid compId);
        Task<ResponseModel> AddOrEditAgreeement(AgreementModel agreement);



    }
}
