﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using TogoFogo.Models;
using TogoFogo.Models.Gateway;
using TogoFogo.Models.Template;
using TogoFogo.Repository.EmailSmsServices;

namespace TogoFogo.Repository
{
    public class EmailsmsServices: IEmailSmsServices
    {
        public readonly TogoFogo.Repository.EmailSmsTemplate.ITemplate _template;
        public readonly TogoFogo.Repository.EmailHeaderFooters.IEmailHeaderFooters _headerFooters;
        public readonly TogoFogo.Repository.SMSGateway.IGateway _gateway;
        public EmailsmsServices()
        {
         
            _headerFooters = new TogoFogo.Repository.EmailHeaderFooters.EmailHeaderFooters();
            _gateway = new TogoFogo.Repository.SMSGateway.Gateway();
        }
        public async Task<string> Send(TemplateModel template, string ToEmail,string EmailFrom,string UserName,string ToMobile)
        {
            var getwaymodel = await Getgateway(Convert.ToInt32(template.GatewayId));
            bool flag = false;
            if (template.ActionTypeName.ToLower() == "actionbased")
            {
                if (template.MessageTypeName == "SMTP Gateway")
                {
                    var headerFooter = await getHeaderfooter(template.EmailHeaderFooterId);
                    var emailBody = headerFooter.HeaderHTML + template.EmailBody + headerFooter.FooterHTML;
                    template.EmailBody = emailBody.Replace("[USER NAME]", UserName);
                    MailMessage mail = new MailMessage();
                    mail.To.Add(ToEmail);
                    mail.Subject = template.Subject;
                    if(!string.IsNullOrEmpty(template.BccEmails))
                    mail.Bcc.Add(template.BccEmails);
                    mail.From = new MailAddress(EmailFrom);
                    mail.Body = template.EmailBody;
                    mail.IsBodyHtml = true;
                    if(template.PriorityTypeId==70)
                    mail.Priority = MailPriority.High;
                    else if (template.PriorityTypeId == 71)
                        mail.Priority = MailPriority.Normal;
                    else
                        mail.Priority = MailPriority.Low;
                    flag = SendEmail(mail, getwaymodel);
                }
                else if(template.MessageTypeName == "SMS Gateway")
                {
                    template.MessageText = template.MessageText.Replace("[User Name]", UserName);
                    template.PhoneNumber = ToMobile;
                  var res=  SendSms(template, getwaymodel);
                }

            }
            return "hello";
        }

        public bool  SendEmail(MailMessage mail,GatewayModel gateway)
        {

            SmtpClient smtp = new SmtpClient();
            smtp.Host = gateway.SmtpServerName;
            smtp.Port =Convert.ToInt32( gateway.PortNumber);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(gateway.SmtpUserName, gateway.SmtpPassword);
            smtp.EnableSsl = gateway.SSLEnabled;

            bool flag = false;
            try
            {
                smtp.Send(mail);
                flag = true;
            }
            catch(Exception ex)
            {
                flag = false;
            }
            return flag;
        }


        private string SendSms(TemplateModel template, GatewayModel gatway)
        {
            string responseString = "";

            string authKey = gatway.OTPApikey;
            //Multiple mobiles numbers separated by comma
            string mobileNumber = template.PhoneNumber;
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderId = gatway.SenderID;
            template.MessageText = template.MessageText;
            //Your message to send, Add URL encoding here.
            string message = HttpUtility.UrlEncode(template.MessageText);

            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&sender={0}", senderId);
            sbPostData.AppendFormat("&route={0}", "default");

            try
            {
                //Call Send SMS API
                string sendSMSUri = gatway.URL;
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                 responseString = reader.ReadToEnd();
                //Close the response
                reader.Close();
                response.Close();
            }
            catch (SystemException ex)
            {

            }
            return responseString;

        }
        public  async Task<GatewayModel> Getgateway( int gatewayId)
        {

            return await _gateway.GetGatewayById(gatewayId);

        }
     

        private async  Task<EmailHeaderFooterModel> getHeaderfooter(int? HeaderFooterId)
        {

            return await _headerFooters.GetEmailHeaderFooterById(HeaderFooterId);


        }

     
      
    }

   

}