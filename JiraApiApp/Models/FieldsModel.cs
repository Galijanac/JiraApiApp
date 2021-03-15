using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraApiApp.Models
{
    public class FieldsModel
    {
        [JsonProperty("lastViewed")]
        public DateTime? LastViewed { get; set; }
        [JsonProperty("resolutiondate")]
        public DateTime? ResolutionDate { get; set; }
    }
}
