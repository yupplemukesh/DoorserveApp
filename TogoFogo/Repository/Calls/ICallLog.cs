using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.ClientData;
namespace TogoFogo.Repository
{
    public interface ICallLog:IDisposable
    {
        Task<ResponseModel> NewCallLog(UploadedExcelModel newCall);
        void Save();
    }
}
