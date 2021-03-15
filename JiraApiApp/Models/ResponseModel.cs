using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraApiApp.Models
{
    public class ResponseModel
    {
        [JsonProperty("expand")]
        public string Expand { get; set; }
        [JsonProperty("startAt")]
        public string StartAt { get; set; }
        [JsonProperty("maxResults")]
        public string MaxResults { get; set; }
        [JsonProperty("total")]
        public string Total { get; set; }
        [JsonProperty("issues")]
        public List<IssueModel> IssuesModels { get; set; }
    }
}
