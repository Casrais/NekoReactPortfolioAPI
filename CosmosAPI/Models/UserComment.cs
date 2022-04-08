using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosAPI.Models
{
    public class UserComment
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public Guid PostId { get; set; }
        public Guid User { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public int Approved { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
