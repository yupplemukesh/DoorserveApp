using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace doorserve.Models
{
    public class Email_send_code
    {
        private int sendmail_update(string TOaddrss, string PR,string UserName)
        {
            string SmptUsername;
            string SmptPassword;
            string From;
            string Host;
            AlternateView View;
            LinkedResource resource;
            StringBuilder msgText = new StringBuilder();
            SmptUsername = "ros-admin@aforeserve.co.in";
            SmptPassword = "1234@qwer";
            Host = "smtp.rediffmailpro.com";
            From = "ros-admin@aforeserve.co.in";
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(SmptUsername, SmptPassword);
            SmtpClient client = new SmtpClient(Host);
            client.UseDefaultCredentials = false;
            client.Credentials = credential;
            client.EnableSsl = true;
            client.Port = 587;
            client.ServicePoint.MaxIdleTime = 1;
            // sgText.Append("<font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal;>Dear Pradip, </font><font font-family=Arial ; font-size=8px; font-weight=bold; font-weight=normal;></font><br/>   <font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal;>You Purchase Order Authentication request has approve"&PR&"")
            // msgText.Append("<font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal;>Dear Pradip,<br/>   <font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal;>Your Purchase Order Authentication Request" & pr&" has approved by TRC-Head")
            msgText.Append(("<font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal;>Dear Sir/Madam, </font><br/" +
                "><font font-family=Arial ; font-size=8px; font-weight=bold; font-weight=normal;>Your Password Of doorserve CRM " +
                "is "
                            + (PR + ".  </font>")));
            msgText.Append("<br/>Your UserName:-<font font-size=9px; font-weight=bold; font-weight=normal;>" +UserName+" </font>");
            //  msgText.Append("<font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal;>Dear </font><font font-family=Arial ; font-size=8px; font-weight=bold; font-weight=normal;>" & EmpName & ",</font><br/>   <font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal;>your account has been locked. Please contact system administrator.")
            msgText.Append("<br/><br/>With regards,<br/><font font-size=9px; font-weight=bold; font-weight=normal;>Aforeserve.Com" +
                " Ltd</font> Admin<br/>");
            // msgText.Append("<img src=cid:Image1 />")
            msgText.Append("<br/>  <font font-family=Arial ; font-size=10px; font-weight=bold; font-weight=normal; colo" +
                "r=Red>This is an auto-generated email. Please do not reply directly to this email.</font>");
            // create an alternate view for your mail
            View = AlternateView.CreateAlternateViewFromString(msgText.ToString(), null, "text/html");
            // link the resource to embedd
            //  resource = New LinkedResource((Server.MapPath("~\Images\aforeseradmin.png")))
            // name the resource
            //  resource.ContentId = "Image1"
            // add the resource to the alternate view
            // View.LinkedResources.Add(resource)
            MailMessage email = new MailMessage(From, TOaddrss);
            email.Bcc.Add("subodh@aforeserve.co.in");
            email.Bcc.Add("pankaj.tiwari@aforeserve.co.in");
            email.AlternateViews.Add(View);
            email.Subject = ("Your doorserve CRM Password is -\'"
                        + (PR + "\'"));
            email.IsBodyHtml = true;
            try
            {
                client.Send(email);
                email.Dispose();
                return 1;
            }
            catch (System.Exception)
            {
                return 0;
            }

        }
    }
}