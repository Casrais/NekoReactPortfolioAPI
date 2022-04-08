using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosAPI.Models
{
    public class Files
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }
        public PostIds[] PostId { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string URL { get; set; }
        public string LightBoxURL { get; set; }
        public string FileType { get; set; }
        public MediumIds[] Medium { get; set; }
        public CategoryIds[] Category { get; set; }
        public string Excerpt { get; set; }
        public CreatorIds[] CreatedBy { get; set; }
        public float Rating { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Medium
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }
        public string MediumType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    //public class ManyFiles
    //{
    //    public Files[] Files { get; set; }

    //    public override string ToString()
    //    {
    //        return JsonConvert.SerializeObject(this);
    //    }
    //}

    public class Category
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }
        public string CategoryType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CreatedBy
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }
        public string Creator { get; set; }
        public string CreatorDesc { get; set; }
        public string CreatorURL { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class ManyItems
    {
        public object[] Items { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }




    public class CreatorIds
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class PostIds
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class CategoryIds
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }



    public class MediumIds
    {
        [JsonProperty(PropertyName = "id")]
        public Guid id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
