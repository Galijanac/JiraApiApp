using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JiraApiApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace JiraApiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("url")) };
            List<IssueModel> resault = new List<IssueModel>();
            string message = string.Empty;

            string base64Credentials = GetEncodedCredentials();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage respons = httpClient.GetAsync($"rest/api/2/search?jql=project={ConfigurationManager.AppSettings.Get("projectCode")}&maxResults=-1&fields=lastViewed,resolutiondate").Result;

            if (respons.IsSuccessStatusCode)
            {
                ResponseModel res = JsonConvert.DeserializeObject<ResponseModel>(respons.Content.ReadAsStringAsync().Result);

                resault = res.IssuesModels.Where(x => x.Fields.ResolutionDate == null).ToList();

                foreach(IssueModel issue in resault)
                {
                    message += $"{issue.Key}  {issue.Fields.LastViewed?.ToString("dd/MM/yy")} \n";
                }
            }

            SendEmail(message);
        }

        static string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", 
                ConfigurationManager.AppSettings.Get("username"), 
                ConfigurationManager.AppSettings.Get("password"));

            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        static void SendEmail(string message)
        {
            string to = ConfigurationManager.AppSettings.Get("mailTo");
            string from = ConfigurationManager.AppSettings.Get("mailFrom");
            MailMessage mailMessage = new MailMessage(from, to);
            mailMessage.Subject = "Unresolved tickets Jira";
            mailMessage.Body = message;

            SmtpClient client = SetSmtpClient();

            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
            }

        }

        public static SmtpClient SetSmtpClient()
        {
            SmtpClient client = new SmtpClient() 
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("mailFrom"),
                                     ConfigurationManager.AppSettings.Get("mailFromPass")),
                Timeout = 30000
            };

            return client;
        }
    }
}
