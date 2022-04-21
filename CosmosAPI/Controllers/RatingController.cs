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

        //[AllowAnonymous]
        [HttpGet("{id}")]
        public string Get(string id)
        {
            try
            {
                var results = _cosmosDbService.GetRatingAsync(id).Result;
                return results;
            }
            catch { return ""; }
            
        }



        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public string Post([FromBody] Rating value)
        {
            string response;
            try
            {
                response = Get(value.id);
            }
            catch
            {
                response = "";
            }
            
            value.DateCreated = DateTime.Today;
            value.UserName = value.UserName.ToUpper();
            string newId;
            if (response == null | response == "")
            {
                newId = Guid.NewGuid().ToString();
            }
            else
            {
                newId = response.Substring(7,36);
            }
            if (newId != "")
            {
                try
                {
                    value.id = newId;
                    _cosmosDbService.UpdateRatingAsync(Guid.Parse(newId), value);
                    return value.id;
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
