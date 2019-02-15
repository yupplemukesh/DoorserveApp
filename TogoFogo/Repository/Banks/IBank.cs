﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public interface IBank: IDisposable
    {
        Task<ResponseModel>AddUpdateBankDetails(BankDetailModel bank);
        void Save();

    }
}
 