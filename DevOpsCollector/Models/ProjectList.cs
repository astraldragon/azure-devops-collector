using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevOpsCollector.Models
{
    public class ProjectListResponse
    {
        public int Count { get; set; }
        [JsonProperty("value")]
        public IList<Project> Projects { get; set; }
    }
}