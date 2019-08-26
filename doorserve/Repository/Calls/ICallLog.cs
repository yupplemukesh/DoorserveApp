using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Filters;
using doorserve.Models;
using doorserve.Models.ClientData;
using doorserve.Models.ServiceCenter;

namespace doorserve.Repository
{
    public interface ICallLog:IDisposable
    {
        Task<ResponseModel> AddOrEditCallLog(CallDetailsModel newCall);
        Task<PreviousCallModel> GetPreviousCall(FilterModel filter);
        List<UploadedExcelModel> GetClientCalls(FilterModel filter);

        Task<List<UploadedExcelModel>> GetExclatedCalls(FilterModel filter);
        Task<List<UploadedExcelModel>> GetCancelRequestedData(FilterModel filter);
        Task<List<CallHistory>> GetCallHistory(FilterModel filter);
        List<FileDetailModel> GetFileList(FilterModel filter);

        void Save();
    }
}
