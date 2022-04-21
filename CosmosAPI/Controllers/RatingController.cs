using CosmosAPI.Models;
using CosmosAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CosmosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RatingController : ControllerBase
    {

        private readonly ICosmosDbService _cosmosDbService;
        public RatingController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("UserFileRating")]
        public string Post(RequestRating rating)
        {
            var results = _cosmosDbService.GetRatingsAsync("SELECT top 1 f.id, f.FileId, f.UserName, f.DateCreated, f.Rate FROM Identity_FileRating f where f.FileId = '" + rating.FileId + "' and f.UserName = '" + rating.UserName.ToUpper() + "'").Result;
            return results;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public string Get(string id)
        {
            Guid GuidID = Guid.Parse(id);
            var results = _cosmosDbService.GetRatingsAsync("SELECT top 1 f.id, f.FileId, f.UserName, f.DateCreated, f.Rate FROM Identity_FileRating f where f.FileId = '" + id + "'").Result;
            return results;
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut]
        public string Put([FromBody] Rating value)
        {
            string response = Get(value.id.ToString());
            value.DateCreated = DateTime.Today;
            value.UserName = value.UserName.ToUpper();
            if (response == "" | response == null)
            {
                value.id = Guid.NewGuid();
                response = value.id.ToString();
            }
            if (response != "" & response != null)
            {
                try
                {
                    string stringId = value.id.ToString();
                    value.id = Guid.Parse(value.id.ToString());
                    _cosmosDbService.UpdateRatingAsync(value.id, value);
                    return "Successfully updated file " + stringId;
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine(ex);
                    return "Error: did not update file.";
                }

            }
            else
            {
                return "Error: File not found.";
            }
        }
    }
}
