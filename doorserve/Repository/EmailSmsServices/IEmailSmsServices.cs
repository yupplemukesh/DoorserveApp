using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using doorserve.Models;
using doorserve.Models.Template;

namespace doorserve.Repository.EmailSmsServices
{
   public  interface IEmailSmsServices
    {

      Task<ResponseModel> Send(List<TemplateModel> templates, List<CheckBox> WildCards, SessionModel session);
    }
}
