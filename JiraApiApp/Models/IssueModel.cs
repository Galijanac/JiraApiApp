using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraApiApp.Models
{
    public class IssueModel
    {
        [JsonProperty("expand")]
        public string Expand { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("self")]
        public string Self { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("fields")]
        public FieldsModel Fields { get; set; }
    }
}
