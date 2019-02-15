﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;

namespace TogoFogo.Repository
{
    public interface IContactPerson: IDisposable
    {
        Task<ResponseModel> AddUpdateContactDetails(ContactPersonModel contact);
        void Save();

    }
}
 