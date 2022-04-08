using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosAPI.Models
{
    public class Rating
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }
        public string UserName { get; set; }
        public string FileId { get; set; }
        public float Rate { get; set; }
        public DateTime DateCreated { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class RequestRating
    {
        public string UserName { get; set; }
        public string FileId { get; set; }
    }


}
