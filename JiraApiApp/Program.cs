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

namespace JiraApiApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("url")) };
            List<IssueModel> resault = new List<IssueModel>();

            string base64Credentials = GetEncodedCredentials();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage respons = httpClient.GetAsync("rest/api/2/search?jql=project=SD&maxResults=-1&fields=lastViewed,resolutiondate").Result;

            if (respons.IsSuccessStatusCode)
            {
                ResponseModel res = JsonConvert.DeserializeObject<ResponseModel>(respons.Content.ReadAsStringAsync().Result);

                resault = res.IssuesModels.Where(x => x.Fields.ResolutionDate == null).ToList();

                foreach(IssueModel issue in resault)
                {
                    Console.WriteLine($"{issue.Key}  {issue.Fields.LastViewed?.ToString("dd/MM/yy")}");
                }
            }

            Console.ReadLine();
        }

        static string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", 
                ConfigurationManager.AppSettings.Get("username"), 
                ConfigurationManager.AppSettings.Get("password"));

            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }
    }
}
