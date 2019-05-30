using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogoFogo.Models;
using TogoFogo.Models.Template;

namespace TogoFogo.Repository.EmailSmsServices
{
   public  interface IEmailSmsServices
    {

      Task<string> Send(List<TemplateModel> templates, List<CheckBox> WildCards, SessionModel session);
    }
}
