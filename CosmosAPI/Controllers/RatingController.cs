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
        public string Get(RequestRating rating)
        {
            var results = _cosmosDbService.GetRatingsAsync("SELECT top 1 f.id, f.FileId, f.UserName, f.DateCreated, f.Rate FROM Identity_FileRating f where f.FileId = '" + rating.FileId + "' and f.UserName = '" + rating.UserName.ToUpper() + "'").Result;
            return results;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public string Get(string id)
        {
            var results = _cosmosDbService.GetRatingsAsync("SELECT top 1 f.id, f.FileId, f.UserName, f.DateCreated, f.Rate FROM Identity_FileRating f where f.FileId = '" + id + "'").Result;
            return results;
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public string Put(string id, [FromBody] Rating value)
        {
            string response = Get(id);
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
                    value.id = Guid.Parse(id);
                    _cosmosDbService.UpdateRatingAsync(Guid.Parse(id), value);
                    return "Successfully updated file " + id;
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine(ex);
                    return "Error: did not update file " + id;
                }

            }
            else
            {
                return "Error: File " + id + " not found.";
            }
        }
    }
}
