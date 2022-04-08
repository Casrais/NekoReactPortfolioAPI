using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosAPI.Models
{
    public class Post
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }
        public string PostTitle { get; set; }
        public string PostDesc { get; set; }
        public DateTime PostDate { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
